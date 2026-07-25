using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StampRallyMiniGame : MonoBehaviour, IMiniGameMaster
{
    [Header("Top Row - Sequence To Follow")]
    [Tooltip("These Images always show the generated sequence.")]
    [FormerlySerializedAs("sequenceSlots")]
    [SerializeField] private Image[] sequenceDisplaySlots;

    [Header("Bottom Row - Player Choices")]
    [Tooltip("These Images fill up as the player clicks stamps.")]
    [SerializeField] private Image[] chosenStampSlots;

    [Tooltip("The clickable stamp buttons. Each button's Image must have its stamp sprite.")]
    [SerializeField] private Button[] stampButtons;

    [Header("Optional UI")]
    [SerializeField] private TMP_Text statusText;

    [Header("Mini Game UI")]
    [Tooltip("The root containing the Stamp Rally UI. Keep the master object outside this root.")]
    [FormerlySerializedAs("gameplayPanel")]
    [SerializeField] private GameObject miniGameRoot;

    [Header("Feedback")]
    [SerializeField, Min(0f)] private float wrongAttemptMessageDuration = 0.75f;

    public event System.Action OnMiniGameCompleted;

    private readonly List<int> sequence = new();
    private int currentStep;
    private bool acceptingInput;

    private void Awake()
    {
        if (sequenceDisplaySlots == null ||
            chosenStampSlots == null ||
            sequenceDisplaySlots.Length == 0)
        {
            Debug.LogError("Stamp Rally needs at least one sequence and chosen slot.", this);
            enabled = false;
            return;
        }

        if (sequenceDisplaySlots.Length != chosenStampSlots.Length)
        {
            Debug.LogError(
                $"Stamp Rally has {sequenceDisplaySlots.Length} sequence slots but " +
                $"{chosenStampSlots.Length} chosen slots. Both arrays must have the same size.",
                this);
            enabled = false;
            return;
        }

        for (int i = 0; i < stampButtons.Length; i++)
        {
            int stampIndex = i;
            stampButtons[i].onClick.AddListener(() => SelectStamp(stampIndex));
        }

        if (miniGameRoot != null && miniGameRoot != gameObject)
        {
            miniGameRoot.SetActive(false);
        }
    }

    public void GenerateMiniGame()
    {
        StopAllCoroutines();

        if (miniGameRoot != null)
        {
            miniGameRoot.SetActive(true);
        }

        SetStampButtonsInteractable(true);
        GenerateNewSequence();
    }

    public void CleanUpMiniGame()
    {
        StopAllCoroutines();
        acceptingInput = false;
        SetStampButtonsInteractable(false);
        sequence.Clear();
        ClearChosenSlots();

        if (miniGameRoot != null)
        {
            miniGameRoot.SetActive(false);
        }
    }

    private void GenerateNewSequence()
    {
        if (stampButtons.Length < 2 ||
            sequenceDisplaySlots.Length == 0 ||
            chosenStampSlots.Length == 0)
        {
            Debug.LogError("Stamp Rally needs at least 2 stamp buttons and 1 sequence slot.", this);
            return;
        }

        sequence.Clear();

        for (int i = 0; i < sequenceDisplaySlots.Length; i++)
        {
            int nextStamp;

            // Prevent the same stamp from appearing twice in a row.
            do
            {
                nextStamp = UnityEngine.Random.Range(0, stampButtons.Length);
            }
            while (i > 0 && nextStamp == sequence[i - 1]);

            sequence.Add(nextStamp);
        }

        currentStep = 0;
        acceptingInput = true;

        DrawSequence();
        SetStatus("Click the stamps in this order!");
    }

    private void SelectStamp(int stampIndex)
    {
        if (!acceptingInput)
        {
            return;
        }

        if (stampIndex == sequence[currentStep])
        {
            ShowChosenStamp(currentStep, stampIndex);
            currentStep++;

            if (currentStep >= sequence.Count)
            {
                StartCoroutine(CompleteMiniGame());
            }
            else
            {
                SetStatus("Correct! Keep going.");
            }

            return;
        }

        StartCoroutine(HandleWrongAttempt(stampIndex));
    }

    private IEnumerator HandleWrongAttempt(int stampIndex)
    {
        acceptingInput = false;
        ShowChosenStamp(currentStep, stampIndex);
        SetStatus("Wrong attempt!");

        yield return new WaitForSeconds(wrongAttemptMessageDuration);

        GenerateNewSequence();
    }

    private IEnumerator CompleteMiniGame()
    {
        acceptingInput = false;
        SetStampButtonsInteractable(false);
        SetStatus("Sequence complete!");

        yield return new WaitForSeconds(wrongAttemptMessageDuration);

        if (miniGameRoot != null)
        {
            miniGameRoot.SetActive(false);
        }

        OnMiniGameCompleted?.Invoke();
    }

    private void SetStampButtonsInteractable(bool interactable)
    {
        for (int i = 0; i < stampButtons.Length; i++)
        {
            stampButtons[i].interactable = interactable;
        }
    }

    private void DrawSequence()
    {
        for (int i = 0; i < sequenceDisplaySlots.Length; i++)
        {
            bool isUsed = i < sequence.Count;
            sequenceDisplaySlots[i].gameObject.SetActive(isUsed);

            if (!isUsed)
            {
                continue;
            }

            // The stamp button's Image supplies the matching sprite.
            sequenceDisplaySlots[i].sprite = stampButtons[sequence[i]].image.sprite;
            sequenceDisplaySlots[i].preserveAspect = true;
            sequenceDisplaySlots[i].enabled = true;
        }

        ClearChosenSlots();
    }

    private void ShowChosenStamp(int slotIndex, int stampIndex)
    {
        chosenStampSlots[slotIndex].sprite = stampButtons[stampIndex].image.sprite;
        chosenStampSlots[slotIndex].preserveAspect = true;
        chosenStampSlots[slotIndex].enabled = true;
    }

    private void ClearChosenSlots()
    {
        for (int i = 0; i < chosenStampSlots.Length; i++)
        {
            bool isUsed = i < sequence.Count;
            chosenStampSlots[i].gameObject.SetActive(isUsed);
            chosenStampSlots[i].sprite = null;
            chosenStampSlots[i].enabled = false;
        }
    }

    private void SetStatus(string message)
    {
        if (statusText != null)
        {
            statusText.text = message;
        }
    }

    private void OnDestroy()
    {
        for (int i = 0; i < stampButtons.Length; i++)
        {
            stampButtons[i].onClick.RemoveAllListeners();
        }
    }
}
