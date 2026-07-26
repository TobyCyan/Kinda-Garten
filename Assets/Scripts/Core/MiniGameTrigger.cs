using System;
using UnityEngine;

public class MiniGameTrigger : Selectable
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
