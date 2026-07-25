using System.Collections;
using TMPro;
using UnityEngine;

public class PauseWdiget : ScreenWidget
{
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameObject pauseContentGroup;

    private bool isPaused = false;

    public bool IsPaused => isPaused;
    const float COUNTDOWN = 3;

    public override void OnButtonClick()
    {
        // start countdown
        pauseContentGroup.SetActive(false);
        countdownText.gameObject.SetActive(true);

        StartCoroutine(CountdownToResume());
    }

    protected override void OnOpenScreen()
    {
        Time.timeScale = 0.0f;
        isPaused = true;
        pauseContentGroup.SetActive(true);
        countdownText.gameObject.SetActive(false);
    }

    protected override void OnCloseScreen()
    {

    }

    IEnumerator CountdownToResume()
    {
        float currentCountdown = COUNTDOWN;

        while (currentCountdown > 0)
        {
            countdownText.text = Mathf.Ceil(currentCountdown).ToString();

            currentCountdown -= Time.unscaledDeltaTime;

            yield return null;
        }

        countdownText.text = "Go";

        yield return new WaitForSecondsRealtime(0.5f);

        Time.timeScale = 1.0f;
        isPaused = false;
        CloseScreen();
    }
}
