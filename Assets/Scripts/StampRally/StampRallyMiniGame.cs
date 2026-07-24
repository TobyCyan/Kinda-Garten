using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class StampRallyMiniGame : MonoBehaviour
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

    [Header("Failure Display")]
    [Tooltip("Red cross GameObjects shown one by one after wrong choices.")]
    [SerializeField] private GameObject[] failureCrosses;

    [Header("End Game")]
    [Tooltip("The panel containing the Stamp Rally gameplay. It will be hidden after success.")]
    [SerializeField] private GameObject gameplayPanel;
    [Tooltip("An optional success panel to show after completing one sequence.")]
    [SerializeField] private GameObject successPanel;
    [SerializeField, Min(0f)] private float endGameDelay = 0.65f;

    [Header("Settings")]
    [SerializeField, Min(1)] private int allowedFailures = 2;
    [SerializeField] private float feedbackDuration = 0.65f;

    [Header("Events")]
    public UnityEvent onSequenceCompleted;

    private readonly List<int> sequence = new();
    private int currentStep;
    private int failedAttempts;
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
    }

    private void Start()
    {
        if (successPanel != null)
        {
            successPanel.SetActive(false);
        }

        GenerateNewSequence();
    }

    public void GenerateNewSequence()
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
                nextStamp = Random.Range(0, stampButtons.Length);
            }
            while (i > 0 && nextStamp == sequence[i - 1]);

            sequence.Add(nextStamp);
        }

        currentStep = 0;
        failedAttempts = 0;
        acceptingInput = true;

        DrawSequence();
        SetStatus("Click the stamps in this order!");
        UpdateFailureCrosses();
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
                StartCoroutine(CompleteSequence());
            }
            else
            {
                SetStatus("Correct! Keep going.");
            }

            return;
        }

        StartCoroutine(HandleWrongStamp(stampIndex));
    }

    private IEnumerator HandleWrongStamp(int stampIndex)
    {
        acceptingInput = false;
        ShowChosenStamp(currentStep, stampIndex);
        failedAttempts++;
        SetStatus("Wrong stamp!");
        UpdateFailureCrosses();

        yield return new WaitForSeconds(feedbackDuration);

        if (failedAttempts >= allowedFailures)
        {
            SetStatus("Two misses — here is a new sequence!");
            yield return new WaitForSeconds(feedbackDuration);
            GenerateNewSequence();
        }
        else
        {
            // First failure: keep the sequence, but restart its clicked progress.
            currentStep = 0;
            ClearChosenSlots();
            SetStatus("Try the same sequence again.");
            acceptingInput = true;
        }
    }

    private IEnumerator CompleteSequence()
    {
        acceptingInput = false;
        SetStampButtonsInteractable(false);
        SetStatus("Sequence complete!");
        onSequenceCompleted?.Invoke();

        yield return new WaitForSeconds(endGameDelay);

        if (successPanel != null)
        {
            successPanel.SetActive(true);
        }

        if (gameplayPanel != null)
        {
            gameplayPanel.SetActive(false);
        }
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

    private void UpdateFailureCrosses()
    {
        if (failureCrosses == null)
        {
            return;
        }

        for (int i = 0; i < failureCrosses.Length; i++)
        {
            if (failureCrosses[i] != null)
            {
                failureCrosses[i].SetActive(i < failedAttempts);
            }
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
