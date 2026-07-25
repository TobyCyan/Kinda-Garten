using System;
using UnityEngine;
using static UnityEngine.Random;

public class Seat : MiniGameTrigger
{
    [SerializeField] private SpriteRenderer stoolRenderer;
    [SerializeField] private SpriteRenderer tableRenderer;
    private Transform seatTransform;
    public Transform SeatTransform => seatTransform;
    public SeatColor Color { get; private set; }
    public bool IsOccupied { get; set; }
    public bool IsAlarmOccupied { get; set; } = false;

    public void TriggerMiniGame()
    {
        TriggerMiniGame(Color);
    }

    private void Awake()
    {
        seatTransform = GetComponent<Transform>();

        SeatColor color = GetRandomColor();
        Color = color;

        tableRenderer.sprite = SeatSpriteStore.GetTableSprite(color);
        stoolRenderer.sprite = SeatSpriteStore.GetStoolSprite(color);
    }

    private SeatColor GetRandomColor()
    {
        return (SeatColor)Range(0, Enum.GetValues(typeof(SeatColor)).Length);
    }

    public enum SeatColor
    {
        RED,
        BLUE,
        ORANGE,
        GREEN,
    }
}
