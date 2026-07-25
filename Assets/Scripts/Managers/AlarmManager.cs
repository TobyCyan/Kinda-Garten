using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AlarmManager : MonoBehaviour
{
    public static AlarmManager Instance { get; private set; }
    public static float MoodTimerSpeedMultiplier { get; private set; } = 1f;

    [Header("Penalty Settings")]
    [SerializeField, Min(1f)] private float alarmSpeedMultiplier = 2f;
    
    [Header("Wave Settings")]
    [SerializeField] private float minWaveInterval = 10.0f;
    [SerializeField] private float maxWaveInterval = 25.0f;
    [SerializeField] private int minAlarmsPerWave = 1;
    [SerializeField] private int maxAlarmsPerWave = 2;

    private int activeAlarmCount = 0;
    public bool IsAlarmActive => activeAlarmCount > 0;

    private List<Alarm> allAlarms = new List<Alarm>();
    private Coroutine ringCoroutine;

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

    public void RegisterAlarm(Alarm alarm)
    {
        allAlarms.Add(alarm);
        
        if (ringCoroutine == null)
        {
            ringCoroutine = StartCoroutine(RingAlarmWavesRoutine());
        }
    }

    private IEnumerator RingAlarmWavesRoutine()
    {
        while (true)
        {
            float waitTime = Random.Range(minWaveInterval, maxWaveInterval);
            yield return new WaitForSeconds(waitTime);

            List<Alarm> quietAlarms = allAlarms.Where(a => !a.IsRinging).ToList();

            int targetWaveSize = Random.Range(minAlarmsPerWave, maxAlarmsPerWave + 1);
            int actualWaveSize = Mathf.Min(targetWaveSize, quietAlarms.Count);

            for (int i = 0; i < actualWaveSize; i++)
            {
                int randomIndex = Random.Range(0, quietAlarms.Count);
                
                // 1. Tell the alarm to start ringing locally
                quietAlarms[randomIndex].TriggerRing();
                
                // 2. Update the manager's global state directly
                StartAlarm();
                
                quietAlarms.RemoveAt(randomIndex);
            }
        }
    }

    private void StartAlarm()
    {
        activeAlarmCount++;
        MoodTimerSpeedMultiplier = alarmSpeedMultiplier;
    }

    public void StopAlarm()
    {
        activeAlarmCount--;
        
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
        minAlarmsPerWave = Mathf.Max(1, minAlarmsPerWave);
        maxAlarmsPerWave = Mathf.Max(minAlarmsPerWave, maxAlarmsPerWave);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStaticState()
    {
        MoodTimerSpeedMultiplier = 1f;
    }
}