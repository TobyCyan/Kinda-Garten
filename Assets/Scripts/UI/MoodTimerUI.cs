using TMPro;
using UnityEngine;

public class MoodTimerUI : MonoBehaviour
{
    private MoodTimer moodTimer;
    [SerializeField] private TextMeshProUGUI moodTimerText;

    public void Init(MoodTimer moodTimer)
    {
        this.moodTimer = moodTimer;
        moodTimer.OnTimerUpdate += RefreshUI;
        ShowUi(true);
    }

    public void ShowUi(bool isShow)
    {
        gameObject.SetActive(isShow);
    }

    private void RefreshUI(float currentMoodTimer)
    {
        int displayedTime = Mathf.Max(0, Mathf.CeilToInt(currentMoodTimer));
        moodTimerText.text = displayedTime.ToString();
        moodTimerText.color = MoodFormatter.FormatMoodColor(displayedTime, moodTimer.BaseMoodTimer);
    }

    public void CleanUp()
    {
        if (moodTimer != null)
        {
            moodTimer.OnTimerUpdate -= RefreshUI;
        }
        moodTimer = null;
        ShowUi(false);
    }
}
