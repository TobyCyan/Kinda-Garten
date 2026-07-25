using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// One-shot start button for testing without MiniGameManager.
/// </summary>
public class CalmingMusicTest : MonoBehaviour
{
    [SerializeField] private CalmingMusicMiniGame calmingMusicMiniGame;
    [SerializeField] private Button startButton;

    private bool hasStarted;

    private void Awake()
    {
        if (startButton != null)
        {
            startButton.onClick.AddListener(StartCalmingMusic);
        }
    }

    public void StartCalmingMusic()
    {
        if (hasStarted)
        {
            return;
        }

        if (calmingMusicMiniGame == null)
        {
            Debug.LogError("Assign CalmingMusicMiniGame to the test script.", this);
            return;
        }

        hasStarted = true;
        calmingMusicMiniGame.GenerateMiniGame();

        if (startButton != null)
        {
            startButton.gameObject.SetActive(false);
        }
    }

    private void OnDestroy()
    {
        if (startButton != null)
        {
            startButton.onClick.RemoveListener(StartCalmingMusic);
        }
    }
}
