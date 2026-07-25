using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private KidSpawnManager kidSpawnManager;

    private float kidSpawnTimer = 0.0f;
    private float nextKidSpawnTime;
    const float MIN_NEXT_KID_SPAWN_TIME = 1.0f;
    const float MAX_NEXT_KID_SPAWN_TIME = 6.0f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        kidSpawnTimer = 0.0f;
    }

    private void Update()
    {
        GameLoop();
    }

    private void GameLoop()
    {
        LoopKidSpawn();
    }

    private void LoopKidSpawn()
    {
        kidSpawnTimer += Time.deltaTime;
        if (kidSpawnTimer >= nextKidSpawnTime)
        {
            kidSpawnManager.SpawnKidAtRandomSeat();
            kidSpawnTimer = 0.0f;
            nextKidSpawnTime = Random.Range(MIN_NEXT_KID_SPAWN_TIME, MAX_NEXT_KID_SPAWN_TIME);
        }
    }
}
