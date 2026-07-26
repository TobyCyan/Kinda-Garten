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
    private float minMoodTimer;
    private float maxMoodTimer;
    private float minCooldownTimer;
    private float maxCooldownTimer;
    private bool isActive;

    private List<Seat> seats = new();

    [SerializeField] private float _spriteYOffset = -0.08f;

    private void Start()
    {
        seats = FindObjectsByType<Seat>().ToList();
    }

    public void InitConfigs(bool isActive, float minNext, float maxNext,
                float minMood, float maxMood,
                float minCooldown, float maxCooldown)
    {
        this.isActive = isActive;
        if (!isActive) return;

        minNextSpawnTime = minNext;
        maxNextSpawnTime = maxNext;
        minMoodTimer = minMood;
        maxMoodTimer = maxMood;
        minCooldownTimer = minCooldown;
        maxCooldownTimer = maxCooldown;
    }

    public void Init()
    {
        if (!isActive) return;
        StartCoroutine(SpawnKidCoroutine());
    }

    private IEnumerator SpawnKidCoroutine()
    {
        while (true)
        {
            float spawnInterval = Random.Range(minNextSpawnTime, maxNextSpawnTime);
            yield return new WaitForSeconds(spawnInterval);
            SpawnKidAtRandomSeat();
        }
    }

    private void SpawnKidAtRandomSeat()
    {
        Seat randomSeat = GetRandomUnoccupiedSeat();
        if (randomSeat != null)
        {
            var kidObject = Instantiate(kidController, randomSeat.SeatTransform);
            randomSeat.Kid = kidObject;

            var randomMoodTimer = Random.Range(minMoodTimer, maxMoodTimer + 1);
            kidObject.transform.localPosition = new Vector3(0f, _spriteYOffset, 0f);

            var randomCooldownTimer = Random.Range(minCooldownTimer, maxCooldownTimer + 1);
            kidObject.OnCooldownTimerFinished += KidObject_OnCooldownTimerFinished;
            kidObject.GetMoodTimer().OnTimerFinished += KidObject_OnMoodTimerFinished;

            kidObject.SetupData(randomMoodTimer, randomCooldownTimer);
        }
        else
        {
            Debug.LogWarning("No unoccupied seats available for spawning a kid.");
        }
    }

    private void KidObject_OnMoodTimerFinished()
    {
        PenaltyManager.Instance.AddPenalty();
    }

    private void KidObject_OnCooldownTimerFinished(KidController kidController)
    {
        var randomMoodTimer = Random.Range(minMoodTimer, maxMoodTimer + 1);
        var randomCooldownTimer = Random.Range(minCooldownTimer, maxCooldownTimer + 1);

        kidController.SetupData(randomMoodTimer, randomCooldownTimer);
    }

    private Seat GetRandomUnoccupiedSeat()
    {
        var unoccupiedSeats = seats.Where(seat => !seat.IsOccupied()).ToList();
        if (unoccupiedSeats.Count == 0)
        {
            return null;
        }
        int randomIndex = Random.Range(0, unoccupiedSeats.Count);
        return unoccupiedSeats[randomIndex];
    }
}
