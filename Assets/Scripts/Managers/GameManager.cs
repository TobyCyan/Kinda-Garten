using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    [SerializeField] private KidSpawnManager kidSpawnManager;
    [SerializeField] private TrashPileSpawnManager trashPileSpawnManager;
    [SerializeField] private MiniGameManager miniGameManager;
    [SerializeField] private PenaltyManager penaltyManager;
    [SerializeField] private UIManager uiManager;
    private PlayerController player;

    [SerializeField] private GameConfigs gameConfigs;

    private void Awake()
    {
        Instance = this;

        player = FindAnyObjectByType<PlayerController>();
    }

    private void Start()
    {
        InitConfigs();
        InitManagers();
    }

    private void OnEnable()
    {
        penaltyManager.OnGameFailed += OnGameFailed;
    }

    private void OnDisable()
    {
        penaltyManager.OnGameFailed -= OnGameFailed;
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
        uiManager.InitConfigs();
    }

    private void InitManagers()
    {
        kidSpawnManager.Init();
        trashPileSpawnManager.Init();
        miniGameManager.Init();
        penaltyManager.Init();
        uiManager.Init();

        SfxManager.Instance.PlayOnLoop(SfxId.BackgroundMusic);
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

    private void OnGameFailed()
    {
        uiManager.OpenGameOverScreen();
    }
}
