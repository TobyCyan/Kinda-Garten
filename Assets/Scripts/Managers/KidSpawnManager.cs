using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KidSpawnManager : MonoBehaviour
{
    private List<Seat> seats = new();

    private void Start()
    {
        seats = FindObjectsByType<Seat>().ToList();
    }

    public void SpawnKidAtRandomSeat()
    {
        Seat randomSeat = GetRandomUnoccupiedSeat();
        if (randomSeat != null)
        {
            // TODO: Spawn the kid prefab at the randomSeat's position
            print($"Spawning kid at seat: {randomSeat.name}");
            randomSeat.IsOccupied = true;
        }
        else
        {
            Debug.LogWarning("No unoccupied seats available for spawning a kid.");
        }
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
