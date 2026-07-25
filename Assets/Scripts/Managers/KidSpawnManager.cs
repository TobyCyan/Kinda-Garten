using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KidSpawnManager : MonoBehaviour
{
    [SerializeField] private KidController kidController;

    // Configs
    private float minNextSpawnTime = 1.0f;
    private float maxNextSpawnTime = 6.0f;
    [SerializeField] private float minMoodTimer;
    [SerializeField] private float maxMoodTimer;
    [SerializeField] private float minCooldownTimer;
    [SerializeField] private float maxCooldownTimer;

    private List<Seat> seats = new();

    private void Start()
    {
        seats = FindObjectsByType<Seat>().ToList();
    }

    public void InitConfigs(float minNext, float maxNext,
                float minMood, float maxMood,
                float minCooldown, float maxCooldown)
    {
        minNextSpawnTime = minNext;
        maxNextSpawnTime = maxNext;
        minMoodTimer = minMood;
        maxMoodTimer = maxMood;
        minCooldownTimer = minCooldown;
        maxCooldownTimer = maxCooldown;
    }

    public void Init()
    {
        StartCoroutine(SpawnKidCoroutine());
    }

    IEnumerator SpawnKidCoroutine()
    {
        while (true)
        {
            float spawnInterval = Random.Range(minNextSpawnTime, maxNextSpawnTime);
            yield return new WaitForSeconds(spawnInterval);
            SpawnKidAtRandomSeat();
        }
    }

    public void SpawnKidAtRandomSeat()
    {
        Seat randomSeat = GetRandomUnoccupiedSeat();
        if (randomSeat != null)
        {
            var kidObject = Instantiate(kidController, randomSeat.SeatTransform);

            var randomMoodTimer = Random.Range(minMoodTimer, maxMoodTimer + 1);
            var randomCooldownTimer = Random.Range(minCooldownTimer, maxCooldownTimer + 1);
            kidObject.OnCooldownTimerFinished += KidObject_OnCooldownTimerFinished;
            kidObject.OnMoodTimerFinished += KidObject_OnMoodTimerFinished;

            kidObject.SetupData(randomMoodTimer, randomCooldownTimer);
            randomSeat.IsOccupied = true;
        }
        else
        {
            Debug.LogWarning("No unoccupied seats available for spawning a kid.");
        }
    }

    private void KidObject_OnMoodTimerFinished()
    {

    }

    private void KidObject_OnCooldownTimerFinished(KidController kidController)
    {
        var randomMoodTimer = Random.Range(minMoodTimer, maxMoodTimer + 1);
        var randomCooldownTimer = Random.Range(minCooldownTimer, maxCooldownTimer + 1);

        kidController.SetupData(randomMoodTimer, randomCooldownTimer);
    }

    private Seat GetRandomUnoccupiedSeat()
    {
        var unoccupiedSeats = seats.Where(seat => !seat.IsOccupied).ToList();
        if (unoccupiedSeats.Count == 0)
        {
            return null;
        }
        int randomIndex = Random.Range(0, unoccupiedSeats.Count);
        return unoccupiedSeats[randomIndex];
    }
}
