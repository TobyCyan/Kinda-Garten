using TMPro;
using UnityEngine;

public class MoodTimer : MonoBehaviour
{
    [SerializeField] private TextMeshPro moodTimerText;
    [SerializeField] private GameObject moddTimerObject;

    public void SetVisiblity(bool isVisible)
    {
        moddTimerObject.SetActive(isVisible);
    }

    public void RefreshUI(float currentMoodTimer)
    {
        moodTimerText.text = currentMoodTimer.ToString("F0");
    }
}
