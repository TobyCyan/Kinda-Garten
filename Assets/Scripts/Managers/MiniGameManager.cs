using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.Random;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager Instance { get; private set; }

    [SerializeField] private MiniGameView miniGameView;
    private List<IMiniGameMaster> gameMasters = new();
    private IMiniGameMaster currentGameMaster;
    private MiniGameContext currentContext;

    public event Action OnMiniGameCompleted;

    public void Init()
    {
        Instance = this;
        gameMasters = new List<IMiniGameMaster>(FindObjectsByType<MonoBehaviour>().OfType<IMiniGameMaster>());
        miniGameView.HideView();
        foreach (var gameMaster in gameMasters)
        {
            gameMaster.CleanUpMiniGame();
        }
    }

    public void GenerateMiniGame(MiniGameContext context)
    {
        if (currentGameMaster != null)
        {
            Debug.LogWarning("A mini-game is already in progress. Stop the current one first.");
            return;
        }

        var gameMaster = gameMasters[Range(0, gameMasters.Count)];
        if (gameMaster == null)
        {
            return;
        }

        GameManager.Instance.SetPlayerActiveStatus(false);
        miniGameView.ShowView(context);

        currentGameMaster = gameMaster;
        gameMaster.GenerateMiniGame();
        gameMaster.OnMiniGameCompleted += () => CompleteMiniGame(true);

        currentContext = context;
        context.MoodTimerRef.OnTimerFinished += () => CompleteMiniGame(false);
    }

    private void StopMiniGame()
    {
        if (currentGameMaster == null || currentContext == null)
        {
            return;
        }

        currentGameMaster.CleanUpMiniGame();
        currentGameMaster.OnMiniGameCompleted -= () => CompleteMiniGame(true);
        currentGameMaster = null;

        currentContext.MoodTimerRef.OnTimerFinished -= () => CompleteMiniGame(false);
        currentContext = null;

        miniGameView.HideView();
        GameManager.Instance.SetPlayerActiveStatus(true);
    }

    private void CompleteMiniGame(bool isSuccessfulAttempt)
    {
        if (isSuccessfulAttempt)
        {
            SfxManager.Instance.Play(SfxId.MiniGameSuccess);
        }
        OnMiniGameCompleted?.Invoke();
        OnMiniGameCompleted = null;
        StopMiniGame();
    }
}