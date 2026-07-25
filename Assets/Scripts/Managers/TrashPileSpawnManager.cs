using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TrashPileSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject trashPilePrefab;
    [SerializeField] private Tilemap obstacle;
    [SerializeField] private Tilemap walkable;
    private float minSpawnInterval = 5f;
    private float maxSpawnInterval = 10f;
    private bool isActive;
    private readonly List<Vector3Int> walkableCells = new();
    private readonly HashSet<Vector3Int> occupiedCells = new();

    public void InitConfigs(bool isActive, float minInterval, float maxInterval)
    {
        this.isActive = isActive;
        if (!isActive) return;

        minSpawnInterval = minInterval;
        maxSpawnInterval = maxInterval;
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
            float spawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(spawnInterval);
            Vector3 spawnPosition = GetSpawnCellPosition();
            SpawnTrashPile(spawnPosition);
            occupiedCells.Add(walkable.WorldToCell(spawnPosition));
        }
    }

    public bool IsCellOccupied(Vector3Int cell)
    {
        return occupiedCells.Contains(cell);
    }

    private Vector3 GetSpawnCellPosition()
    {
        int randomIndex = Random.Range(0, walkableCells.Count);
        return walkable.GetCellCenterWorld(walkableCells[randomIndex]);
    }
    
    private void SpawnTrashPile(Vector3 position)
    {
        GameObject go = Instantiate(trashPilePrefab, position, Quaternion.identity);
        if (go.TryGetComponent(out TrashPile trashPile))
        {
            trashPile.OnHoldCompleted += () => FreeUpOccupiedCell(position);
            trashPile.Init();
        }
    }

    private void FreeUpOccupiedCell(Vector3 pos)
    {
        Vector3Int cell = walkable.WorldToCell(pos);
        occupiedCells.Remove(cell);
    }
}
