using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
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
    private readonly List<Vector3Int> walkableCells = new();
    private readonly List<Vector3Int> spawnableCells = new();

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
        walkableCells.Clear();
        foreach (var pos in walkable.cellBounds.allPositionsWithin)
        {
            if (walkable.HasTile(pos) & !obstacle.HasTile(pos))
            {
                walkableCells.Add(pos);
                spawnableCells.Add(pos);
            }
        }
    }

    public void Init()
    {
        if (!isActive) return;
        StartCoroutine(StartSpawning());
    }

    IEnumerator StartSpawning()
    {
        while (true)
        {
            while (GameStates.IsPaused || GameStates.IsGameFinish) { yield return null; }
            float spawnInterval = Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(spawnInterval);
            while (GameStates.IsPaused || GameStates.IsGameFinish) { yield return null; }
            try
            {
                Vector3 spawnPosition = GetSpawnCellPosition();
                SpawnTrashPile(spawnPosition);
                spawnableCells.Remove(walkable.WorldToCell(spawnPosition));
            }
            catch (Exception)
            {
                continue;
            }
        }
    }

    public bool IsCellOccupied(Vector3Int cell)
    {
        return !spawnableCells.Contains(cell);
    }

    private Vector3 GetSpawnCellPosition() 
    {
        if (spawnableCells.Count == 0)
        {
            throw new SpawnException("No spawnable cells available for trash pile spawning.");
        }
        int randomIndex = Range(0, spawnableCells.Count);
        return walkable.GetCellCenterWorld(spawnableCells[randomIndex]);
    }
    
    private void SpawnTrashPile(Vector3 position)
    {
        GameObject go = Instantiate(trashPilePrefab, position, Quaternion.identity);
        if (go.TryGetComponent(out TrashPile trashPile))
        {
            trashPile.OnHoldCompleted += () => FreeCell(position);
            trashPile.Init(trashPileCleanUpTime);
        }
    }

    private void FreeCell(Vector3 pos)
    {
        Vector3Int cell = walkable.WorldToCell(pos);
        spawnableCells.Add(cell);
    }
}
