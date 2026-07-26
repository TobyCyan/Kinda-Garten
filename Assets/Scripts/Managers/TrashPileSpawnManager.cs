using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static UnityEngine.Random;

public class TrashPileSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject trashPilePrefab;
    [SerializeField] private Tilemap obstacle;
    [SerializeField] private Tilemap walkable;

    private float minSpawnInterval = 5f;
    private float maxSpawnInterval = 10f;
    private float trashPileCleanUpTime = 1.0f;

    private bool isActive;

    private readonly List<Vector3Int> spawnableCells = new();
    private readonly HashSet<Vector3Int> occupiedCells = new();

    public void InitConfigs(bool isActive, float minInterval, float maxInterval, float trashPileCleanUpTime)
    {
        this.isActive = isActive;
        if (!isActive) return;

        minSpawnInterval = minInterval;
        maxSpawnInterval = maxInterval;
        this.trashPileCleanUpTime = trashPileCleanUpTime;

        InitTileMapInfo();
    }

    private void InitTileMapInfo()
    {
        spawnableCells.Clear();
        occupiedCells.Clear();

        foreach (var pos in walkable.cellBounds.allPositionsWithin)
        {
            if (walkable.HasTile(pos) && !obstacle.HasTile(pos))
            {
                spawnableCells.Add(pos);
            }
        }
    }

    public void Init()
    {
        if (!isActive) return;
        StartCoroutine(StartSpawning());
    }

    private IEnumerator StartSpawning()
    {
        while (true)
        {
            while (GameStates.IsPaused || GameStates.IsGameFinish)
                yield return null;

            float spawnInterval = Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(spawnInterval);

            while (GameStates.IsPaused || GameStates.IsGameFinish)
                yield return null;

            if (spawnableCells.Count == 0)
                continue;

            Vector3Int cell = GetSpawnCell();
            SpawnTrashPile(cell);
        }
    }

    public bool IsCellOccupied(Vector3Int cell)
    {
        return occupiedCells.Contains(cell);
    }

    private Vector3Int GetSpawnCell()
    {
        if (spawnableCells.Count == 0)
            throw new SpawnException("No spawnable cells available for trash pile spawning.");

        int randomIndex = Range(0, spawnableCells.Count);
        return spawnableCells[randomIndex];
    }

    private void SpawnTrashPile(Vector3Int cell)
    {
        if (occupiedCells.Contains(cell))
            return;

        spawnableCells.Remove(cell);
        occupiedCells.Add(cell);

        Vector3 position = walkable.GetCellCenterWorld(cell);
        GameObject go = Instantiate(trashPilePrefab, position, Quaternion.identity);

        if (go.TryGetComponent(out TrashPile trashPile))
        {
            trashPile.OnHoldCompleted += () => FreeCell(cell);
            trashPile.Init(trashPileCleanUpTime);
        }
        else
        {
            Debug.LogError("Trash pile prefab is missing TrashPile component.");
            FreeCell(cell);
        }
    }

    private void FreeCell(Vector3Int cell)
    {
        if (occupiedCells.Remove(cell))
        {
            if (!spawnableCells.Contains(cell))
            {
                spawnableCells.Add(cell);
            }
        }
    }
}
