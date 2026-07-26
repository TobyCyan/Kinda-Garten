using System;
using UnityEngine;
using static Seat;

public class MiniGameTrigger : MonoBehaviour
{
    public void TriggerMiniGame(MiniGameContext context, Action callback)
    {
        if (MiniGameManager.Instance == null)
        {
            Debug.LogError("MiniGameManager instance is not set.");
            return;
        }
        MiniGameManager.Instance.GenerateMiniGame(context);
        MiniGameManager.Instance.OnMiniGameCompleted += callback;
    }
}
