using System;
using UnityEngine;
using static UnityEngine.Random;

public class Seat : MiniGameTrigger
{
    [SerializeField] private SpriteRenderer stoolRenderer;
    [SerializeField] private SpriteRenderer tableRenderer;
    private Collider2D seatCollider;
    private Transform seatTransform;
    public Transform SeatTransform => seatTransform;
    public SeatColor Color { get; private set; }
    public bool IsOccupied { get; set; }

    private void Awake()
    {
        seatTransform = GetComponent<Transform>();
        seatCollider = GetComponent<Collider2D>();

        SeatColor color = GetRandomColor();
        Color = color;

        tableRenderer.sprite = SeatSpriteStore.GetTableSprite(color);
        stoolRenderer.sprite = SeatSpriteStore.GetStoolSprite(color);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"Seat {Color} collided with {collision.gameObject.name}");
        if (collision.CompareTag("Player"))
        {
            TriggerMiniGame(Color);
        }
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
