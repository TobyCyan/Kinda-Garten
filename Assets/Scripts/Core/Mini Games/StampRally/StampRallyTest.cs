using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Test-only click-to-start harness for Stamp Rally.
/// Do not use this alongside an active MiniGameManager.
/// </summary>
public class StampRallyTest : MonoBehaviour
{
    [SerializeField] private StampRallyGameMaster stampRally;
    [SerializeField] private Button startButton;

    private void Awake()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartStampRally);
        }
    }

    private void OnEnable()
    {
        if (stampRally != null)
        {
            stampRally.OnMiniGameCompleted += HandleMiniGameCompleted;
        }
    }

    public void StartStampRally()
    {
        if (stampRally == null)
        {
            Debug.LogError("Assign the StampRallyMiniGame component to the test script.", this);
            return;
        }

        if (startButton != null)
        {
            startButton.onClick.RemoveListener(StartStampRally);
            Destroy(startButton.gameObject);
            startButton = null;
        }

        stampRally.GenerateMiniGame();
    }

    private void HandleMiniGameCompleted()
    {
        Debug.Log("Stamp Rally completed successfully.", this);
        stampRally.CleanUpMiniGame();
    }

    private void OnDisable()
    {
        if (stampRally != null)
        {
            stampRally.OnMiniGameCompleted -= HandleMiniGameCompleted;
        }
    }
}
