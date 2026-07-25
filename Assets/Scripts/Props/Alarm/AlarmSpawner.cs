using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AlarmSpawner : MonoBehaviour
{
    [SerializeField] private Alarm alarmPrefab;
    
    [Header("Spawn Settings")]
    [SerializeField] private float minNextSpawnTime = 10.0f;
    [SerializeField] private float maxNextSpawnTime = 25.0f;

    private List<Seat> seats = new List<Seat>();

    private void Start()
    {
        seats = FindObjectsByType<Seat>(FindObjectsSortMode.None).ToList();
        StartCoroutine(SpawnAlarmCoroutine());
    }

    IEnumerator SpawnAlarmCoroutine()
    {
        while (true)
        {
            float spawnInterval = Random.Range(minNextSpawnTime, maxNextSpawnTime);
            yield return new WaitForSeconds(spawnInterval);
            SpawnAlarmAtRandomSeat();
        }
    }

    public void SpawnAlarmAtRandomSeat()
    {
        List<Seat> availableSeats = seats.Where(seat => !seat.IsAlarmOccupied).ToList();

        if (availableSeats.Count == 0) 
        {
            Debug.LogWarning("No available seats to spawn an alarm.");
            return;
        }

        int randomIndex = Random.Range(0, availableSeats.Count);
        Seat randomSeat = availableSeats[randomIndex];

        randomSeat.IsAlarmOccupied = true;

        Alarm alarmInstance = Instantiate(alarmPrefab, randomSeat.SeatTransform.position, Quaternion.identity, randomSeat.SeatTransform);
        
        alarmInstance.Init(randomSeat);

        if (AlarmManager.Instance != null)
        {
            AlarmManager.Instance.StartAlarm();
        }
    }
}