using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TrashPileSpawnManager : MonoBehaviour
{
    [SerializeField] private GameObject trashPilePrefab;
    [SerializeField] private float minSpawnInterval = 5f;
    [SerializeField] private float maxSpawnInterval = 10f;
    [SerializeField] private Tilemap floor;

    public void InitConfigs(float minInterval, float maxInterval)
    {
        minSpawnInterval = minInterval;
        maxSpawnInterval = maxInterval;
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
        Vector3Int randomCell = new(Random.Range(floor.cellBounds.xMin, floor.cellBounds.xMax),
                                               Random.Range(floor.cellBounds.yMin, floor.cellBounds.yMax),
                                               0);
        return floor.GetCellCenterWorld(randomCell);
    }
    
    private void SpawnTrashPile(Vector3 position)
    {
        Instantiate(trashPilePrefab, position, Quaternion.identity);
    }
}
