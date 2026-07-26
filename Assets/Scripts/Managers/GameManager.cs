using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private KidSpawnManager kidSpawnManager;
    [SerializeField] private TrashPileSpawnManager trashPileSpawnManager;
    [SerializeField] private MiniGameManager miniGameManager;
    private PlayerController player;

    [SerializeField] private GameConfigs gameConfigs;

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

        player = FindAnyObjectByType<PlayerController>();
        InitConfigs();
        InitManagers();
    }

    private void InitConfigs()
    {
        kidSpawnManager.InitConfigs(gameConfigs.IsKidSpawnActive,
                    gameConfigs.MinNextSpawnTime, gameConfigs.MaxNextSpawnTime,
                    gameConfigs.MinMoodTimer, gameConfigs.MaxMoodTimer,
                    gameConfigs.MinCooldownTimer, gameConfigs.MaxCooldownTimer);
        trashPileSpawnManager.InitConfigs(gameConfigs.IsTrashPileSpawnActive,
                    gameConfigs.MinNextSpawnTimeTrashPile, gameConfigs.MaxNextSpawnTimeTrashPile,
                    gameConfigs.TrashPileCleanUpTime);
    }

    private void InitManagers()
    {
        kidSpawnManager.Init();
        trashPileSpawnManager.Init();
        miniGameManager.Init();
    }

    public bool IsCellOccupied(Vector3Int cell)
    {
        return trashPileSpawnManager.IsCellOccupied(cell);
    }

    public void SetPlayerActiveStatus(bool isActive)
    {
        if (player != null)
        {
            player.SetActive(isActive);
        }
    }
}
