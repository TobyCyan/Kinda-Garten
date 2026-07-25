using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private KidSpawnManager kidSpawnManager;
    [SerializeField] private TrashPileSpawnManager trashPileSpawnManager;

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

        InitConfigs();
        InitManagers();
    }

    private void InitConfigs()
    {
        kidSpawnManager.InitConfigs(1.0f, 6.0f, 5.0f, 10.0f, 3.0f, 5.0f);
        trashPileSpawnManager.InitConfigs(5.0f, 10.0f);
    }

    private void InitManagers()
    {
        kidSpawnManager.Init();
        trashPileSpawnManager.Init();
    }

    public bool IsCellOccupied(Vector3Int cell)
    {
        return trashPileSpawnManager.IsCellOccupied(cell);
    }
}
