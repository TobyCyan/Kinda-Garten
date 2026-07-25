using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Test-only button that starts the alarm once.
/// </summary>
public class AlarmTest : MonoBehaviour
{
    [SerializeField] private AlarmManager alarmManager;
    [SerializeField] private Button startAlarmButton;

    private void Awake()
    {
        if (startAlarmButton != null)
        {
            startAlarmButton.onClick.AddListener(StartAlarm);
        }
    }

    public void StartAlarm()
    {
        if (alarmManager == null)
        {
            Debug.LogError("Assign AlarmManager to the alarm test script.", this);
            return;
        }

        alarmManager.StartAlarm();

        if (startAlarmButton != null)
        {
            startAlarmButton.interactable = false;
        }
    }

    private void OnDestroy()
    {
        if (startAlarmButton != null)
        {
            startAlarmButton.onClick.RemoveListener(StartAlarm);
        }
    }
}
