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
        moodTimerText.text = currentMoodTimer.ToString("F0");
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
