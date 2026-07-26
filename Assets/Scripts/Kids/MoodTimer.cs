using System;
using TMPro;
using UnityEngine;

public class MoodTimer : MonoBehaviour
{
    [SerializeField] private TextMeshPro moodTimerText;
    [SerializeField] private GameObject moddTimerObject;

    public event Action<float> OnTimerUpdate;

    public void SetVisiblity(bool isVisible)
    {
        moddTimerObject.SetActive(isVisible);
    }

    public void RefreshUI(float currentMoodTimer)
    {
        moodTimerText.text = currentMoodTimer.ToString("F0");
        OnTimerUpdate?.Invoke(currentMoodTimer);
    }
}
