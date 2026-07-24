using System.Collections.Generic;
using System;
using UnityEngine;
using static UnityEngine.Random;
using System.Linq;

public class Seat : MonoBehaviour
{
    [SerializeField] private SpriteRenderer stoolRenderer;
    [SerializeField] private SpriteRenderer tableRenderer;
    private Transform seatTransform;
    public Transform SeatTransform => seatTransform;
    public SeatColor Color { get; private set; }
    public bool IsOccupied { get; set; }

    private const string SPRITE_SHEET_PATH = "Sprites/Environment";

    private static readonly Dictionary<SeatColor, string> COLOR_TABLE_SPRITE_NAME_MAP = new()
    {
        { SeatColor.RED, "Table_Red" },
        { SeatColor.BLUE, "Table_Blue" },
        { SeatColor.ORANGE, "Table_Orange" },
        { SeatColor.GREEN, "Table_Green" },
    };

    private static readonly Dictionary<SeatColor, string> COLOR_STOOL_SPRITE_NAME_MAP = new()
    {
        { SeatColor.RED, "Stool_Red" },
        { SeatColor.BLUE, "Stool_Blue" },
        { SeatColor.ORANGE, "Stool_Orange" },
        { SeatColor.GREEN, "Stool_Green" },
    };

    private static readonly Dictionary<string, Sprite> spritesNameMap = new();

    private static void EnsureSpritesLoaded()
    {
        if (spritesNameMap.Count > 0) return;

        var sprites = Resources.LoadAll<Sprite>(SPRITE_SHEET_PATH);

        foreach (var sprite in sprites)
        {
            spritesNameMap[sprite.name] = sprite;
        }
    }

    private void Awake()
    {
        seatTransform = GetComponent<Transform>();

        EnsureSpritesLoaded();

        SeatColor color = GetRandomColor();
        Color = color;

        Sprite tableSprite = spritesNameMap[COLOR_TABLE_SPRITE_NAME_MAP[color]];
        tableRenderer.sprite = tableSprite;

        Sprite stoolSprite = spritesNameMap[COLOR_STOOL_SPRITE_NAME_MAP[color]];
        stoolRenderer.sprite = stoolSprite;
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
