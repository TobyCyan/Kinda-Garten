using UnityEngine;

/// <summary>
/// Controls the global alarm effect for every kid's visible mood countdown.
/// Existing and newly spawned kids read the same multiplier.
/// </summary>
public class AlarmManager : MonoBehaviour
{
    public static AlarmManager Instance { get; private set; }
    public static float MoodTimerSpeedMultiplier { get; private set; } = 1f;

    [SerializeField, Min(1f)] private float alarmSpeedMultiplier = 2f;
    
    // Track how many alarms are currently active
    private int activeAlarmCount = 0;

    public bool IsAlarmActive => activeAlarmCount > 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [ContextMenu("Start Alarm")]
    public void StartAlarm()
    {
        activeAlarmCount++;
        MoodTimerSpeedMultiplier = alarmSpeedMultiplier;
    }

    [ContextMenu("Stop Alarm")]
    public void StopAlarm()
    {
        activeAlarmCount--;
        
        // Only reset the multiplier if all alarms have been turned off
        if (activeAlarmCount <= 0)
        {
            activeAlarmCount = 0;
            MoodTimerSpeedMultiplier = 1f;
        }
    }

    private void OnDisable()
    {
        activeAlarmCount = 0;
        MoodTimerSpeedMultiplier = 1f;
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