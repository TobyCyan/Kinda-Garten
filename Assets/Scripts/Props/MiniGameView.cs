using UnityEngine;
using static Seat;

public class MiniGameView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer baseRenderer;

    public void ShowMiniGame(SeatColor color)
    {
        gameObject.SetActive(true);
        baseRenderer.sprite = SeatSpriteStore.GetTableSprite(color);
    }

    public void HideMiniGame()
    {
        gameObject.SetActive(false);
    }
}