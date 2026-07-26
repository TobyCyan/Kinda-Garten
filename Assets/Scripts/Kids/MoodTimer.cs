using System;
using TMPro;
using UnityEngine;

public class MoodTimer : MonoBehaviour
{
    [SerializeField] private TextMeshPro moodTimerText;
    [SerializeField] private GameObject moddTimerObject;
    public float BaseMoodTimer { get; private set; }

    public event Action<float> OnTimerUpdate;
    public event Action OnTimerFinished;

    public void Init(float baseMoodTimer)
    {
        BaseMoodTimer = baseMoodTimer;
        UpdateTimer(baseMoodTimer);
    }

    public void SetVisiblity(bool isVisible)
    {
        moddTimerObject.SetActive(isVisible);
    }

    public void UpdateTimer(float newTimer)
    {
        RefreshUI(newTimer);
    }

    // Temporary, this method is called from KidController when the mood timer finishes,
    // but ideally, all timer logic should be handled in this class.
    public void InvokeOnTimerFinished()
    {
        OnTimerFinished?.Invoke();
    }

    public void CleanUp()
    {
        OnTimerUpdate = null;
        OnTimerFinished = null;
    }

    private void RefreshUI(float currentMoodTimer)
    {
        int displayedTime = Mathf.Max(0, Mathf.CeilToInt(currentMoodTimer));
        moodTimerText.text = displayedTime.ToString();
        moodTimerText.color = MoodFormatter.FormatMoodColor(displayedTime, BaseMoodTimer);
        OnTimerUpdate?.Invoke(currentMoodTimer);
    }
}
