using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AlarmSpawner : MonoBehaviour
{
    [SerializeField] private Alarm alarmPrefab;
    
    [Header("Spawn Settings")]
    [SerializeField] private int minAlarmCount = 1;
    [SerializeField] private int maxAlarmCount = 3;

    private void Start()
    {
        // FIX: Replaced obsolete FindObjectsSortMode with FindObjectsInactive
        List<Seat> availableSeats = FindObjectsByType<Seat>(FindObjectsInactive.Exclude).ToList();

        if (availableSeats.Count == 0) 
        {
            Debug.LogWarning("No seats found. Cannot spawn alarms.");
            return;
        }

        int actualMax = Mathf.Min(maxAlarmCount, availableSeats.Count);
        int actualMin = Mathf.Min(minAlarmCount, actualMax);
        int spawnCount = Random.Range(actualMin, actualMax + 1);

        for (int i = 0; i < spawnCount; i++)
        {
            int randomIndex = Random.Range(0, availableSeats.Count);
            Seat randomSeat = availableSeats[randomIndex];

            Alarm alarmInstance = Instantiate(alarmPrefab, randomSeat.SeatTransform.position, Quaternion.identity, randomSeat.SeatTransform);
            
            if (AlarmManager.Instance != null)
            {
                AlarmManager.Instance.RegisterAlarm(alarmInstance);
            }

            availableSeats.RemoveAt(randomIndex);
        }
    }

    private void OnValidate()
    {
        minAlarmCount = Mathf.Max(0, minAlarmCount);
        maxAlarmCount = Mathf.Max(minAlarmCount, maxAlarmCount);
    }
}