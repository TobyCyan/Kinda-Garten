using System;
using TMPro;
using UnityEngine;

public class MoodTimer : MonoBehaviour
{
    [SerializeField] private TextMeshPro moodTimerText;
    [SerializeField] private GameObject moddTimerObject;
    public float BaseMoodTimer { get; private set; }

    public event Action<float> OnTimerUpdate;

    public void Init(float baseMoodTimer)
    {
        BaseMoodTimer = baseMoodTimer;
        RefreshUI(baseMoodTimer);
    }

    public void SetVisiblity(bool isVisible)
    {
        moddTimerObject.SetActive(isVisible);
    }

    public void RefreshUI(float currentMoodTimer)
    {
        moodTimerText.text = currentMoodTimer.ToString("F0");
        moodTimerText.color = MoodFormatter.FormatMoodColor(currentMoodTimer, BaseMoodTimer);
        OnTimerUpdate?.Invoke(currentMoodTimer);
    }
}
