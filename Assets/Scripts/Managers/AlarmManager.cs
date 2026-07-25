using UnityEngine;

/// <summary>
/// Controls the global alarm effect for every kid's visible mood countdown.
/// Existing and newly spawned kids read the same multiplier.
/// </summary>
public class AlarmManager : MonoBehaviour
{
    public static float MoodTimerSpeedMultiplier { get; private set; } = 1f;

    [SerializeField, Min(1f)] private float alarmSpeedMultiplier = 2f;

    public bool IsAlarmActive { get; private set; }

    [ContextMenu("Start Alarm")]
    public void StartAlarm()
    {
        IsAlarmActive = true;
        MoodTimerSpeedMultiplier = alarmSpeedMultiplier;
    }

    [ContextMenu("Stop Alarm")]
    public void StopAlarm()
    {
        IsAlarmActive = false;
        MoodTimerSpeedMultiplier = 1f;
    }

    private void OnDisable()
    {
        if (IsAlarmActive)
        {
            StopAlarm();
        }
    }

    private void OnValidate()
    {
        alarmSpeedMultiplier = Mathf.Max(1f, alarmSpeedMultiplier);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStaticState()
    {
        MoodTimerSpeedMultiplier = 1f;
    }
}
