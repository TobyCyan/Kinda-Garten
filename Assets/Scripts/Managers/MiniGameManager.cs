using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager Instance { get; private set; }

    private List<IMiniGameMaster> gameMasters = new();
    private IMiniGameMaster currentGameMaster;

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
        gameMasters = new List<IMiniGameMaster>(FindObjectsByType<MonoBehaviour>().OfType<IMiniGameMaster>());
    }

    private void Start()
    {
        GenerateMiniGame();
    }

    // Should be called when a kid is interacted with or when a player fails a mini-game
    public void GenerateMiniGame()
    {
        var gameMaster = gameMasters[Random.Range(0, gameMasters.Count)];
        if (gameMaster == null)
        {
            return;
        }
        currentGameMaster = gameMaster;
        gameMaster.GenerateMiniGame();
        gameMaster.OnMiniGameCompleted += OnMiniGameCompleted;
    }

    private void OnMiniGameCompleted()
    {
        if (currentGameMaster == null)
        {
            return;
        }
        currentGameMaster.CleanUpMiniGame();
        currentGameMaster.OnMiniGameCompleted -= OnMiniGameCompleted;
    }
}