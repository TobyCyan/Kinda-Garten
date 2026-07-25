using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TrashPileSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject trashPilePrefab;
    [SerializeField] private float minSpawnInterval = 5f;
    [SerializeField] private float maxSpawnInterval = 10f;
    [SerializeField] private Tilemap obstacle;
    [SerializeField] private Tilemap walkable;
    private readonly List<Vector3Int> walkableCells = new();

    public void InitConfigs(float minInterval, float maxInterval)
    {
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
        }
    }

    private Vector3 GetSpawnCellPosition()
    {
        int randomIndex = Random.Range(0, walkableCells.Count);
        return walkable.GetCellCenterWorld(walkableCells[randomIndex]);
    }
    
    private void SpawnTrashPile(Vector3 position)
    {
        Instantiate(trashPilePrefab, position, Quaternion.identity);
    }
}
