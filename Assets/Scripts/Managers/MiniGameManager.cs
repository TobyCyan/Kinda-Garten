using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Seat;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager Instance { get; private set; }

    [SerializeField] private MiniGameView miniGameView;
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
        Init();
    }

    private void Init()
    {
        gameMasters = new List<IMiniGameMaster>(FindObjectsByType<MonoBehaviour>().OfType<IMiniGameMaster>());
        miniGameView.HideView();
        foreach (var gameMaster in gameMasters)
        {
            gameMaster.CleanUpMiniGame();
        }
    }

    public void GenerateMiniGame(SeatColor seatColor)
    {
        var gameMaster = gameMasters[Random.Range(0, gameMasters.Count)];
        if (gameMaster == null)
        {
            return;
        }

        miniGameView.ShowView(seatColor);

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
        currentGameMaster = null;

        miniGameView.HideView();
    }
}