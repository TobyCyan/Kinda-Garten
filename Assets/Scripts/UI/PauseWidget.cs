using System.Collections;
using TMPro;
using UnityEngine;

public class PauseWdiget : ScreenWidget
{
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameObject pauseContentGroup;

    const float COUNTDOWN = 3;

    public override void OnButtonClick()
    {
        // start countdown
        pauseContentGroup.SetActive(false);
        countdownText.gameObject.SetActive(true);

        StartCoroutine(CountdownToResume(false));
    }

    protected override void OnOpenScreen()
    {
        GameStates.IsPaused = true;
        Time.timeScale = 0.0f;
        pauseContentGroup.SetActive(true);
        countdownText.gameObject.SetActive(false);
    }

    protected override void OnCloseScreen()
    {

    }

    public void StartWithCountdown()
    {
        pauseContentGroup.SetActive(false);
        countdownText.gameObject.SetActive(true);
        StartCoroutine(CountdownToResume(true));
    }

    IEnumerator CountdownToResume(bool isStarting)
    {
        if (isStarting)
        {
            countdownText.text = "Ready";

            yield return new WaitForSecondsRealtime(2.0f);

            countdownText.text = "Set";

            yield return new WaitForSecondsRealtime(1.0f);

            countdownText.text = "Go";

            yield return new WaitForSecondsRealtime(0.5f);

            GameStates.IsPaused = false;
            Time.timeScale = 1.0f;
            CloseScreen();

            yield break;
        }

        float currentCountdown = COUNTDOWN;

        while (currentCountdown > 0)
        {
            countdownText.text = Mathf.Ceil(currentCountdown).ToString();

            currentCountdown -= Time.unscaledDeltaTime;

            yield return null;
        }

        countdownText.text = "Go";

        yield return new WaitForSecondsRealtime(0.5f);

        GameStates.IsPaused = false;
        Time.timeScale = 1.0f;
        CloseScreen();
    }
}
