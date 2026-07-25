using UnityEngine;
using static Seat;

public class MiniGameView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer baseRenderer;

    public void ShowView(SeatColor seatColor)
    {
        gameObject.SetActive(true);
        baseRenderer.sprite = SeatSpriteStore.GetTableSprite(seatColor);
    }

    public void HideView()
    {
        gameObject.SetActive(false);
    }
}