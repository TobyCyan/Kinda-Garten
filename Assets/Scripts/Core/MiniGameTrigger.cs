using UnityEngine;
using static Seat;

public class MiniGameTrigger : MonoBehaviour
{
    protected void TriggerMiniGame(SeatColor seatColor)
    {
        if (MiniGameManager.Instance == null)
        {
            Debug.LogError("MiniGameManager instance is not set.");
            return;
        }
        MiniGameManager.Instance.GenerateMiniGame(seatColor);
    }
}
