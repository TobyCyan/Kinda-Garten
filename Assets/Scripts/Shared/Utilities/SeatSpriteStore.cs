using System.Collections.Generic;
using UnityEngine;
using static Seat;

public class SeatSpriteStore : SpriteStore
{
    private const string SPRITE_SHEET_PATH = "Sprites/Environment";
    private const string SPRITE_SHEET_TOP_DOWN_PATH = "Sprites/Props/Table_TopDown";

    private static readonly Dictionary<SeatColor, string> COLOR_TABLE_SPRITE_NAME_MAP = new()
    {
        { SeatColor.RED, "Table_Red" },
        { SeatColor.BLUE, "Table_Blue" },
        { SeatColor.ORANGE, "Table_Orange" },
        { SeatColor.GREEN, "Table_Green" },
    };

    private static readonly Dictionary<SeatColor, string> COLOR_TABLE_TOP_DOWN_SPRITE_NAME_MAP = new()
    {
        { SeatColor.RED, "Table_TopDown_Red" },
        { SeatColor.BLUE, "Table_TopDown_Blue" },
        { SeatColor.ORANGE, "Table_TopDown_Orange" },
        { SeatColor.GREEN, "Table_TopDown_Green" },
    };

    private static readonly Dictionary<SeatColor, string> COLOR_STOOL_SPRITE_NAME_MAP = new()
    {
        { SeatColor.RED, "Stool_Red" },
        { SeatColor.BLUE, "Stool_Blue" },
        { SeatColor.ORANGE, "Stool_Orange" },
        { SeatColor.GREEN, "Stool_Green" },
    };

    private static readonly Dictionary<string, Sprite> spritesNameMap = new();
    private static readonly Dictionary<string, Sprite> spritesTopDownNameMap = new();

    public static Sprite GetTableSprite(SeatColor color)
    {
        EnsureSpritesLoaded(spritesNameMap, SPRITE_SHEET_PATH);
        return spritesNameMap[COLOR_TABLE_SPRITE_NAME_MAP[color]];
    }

    public static Sprite GetTableTopDownSprite(SeatColor color)
    {
        EnsureSpritesLoaded(spritesTopDownNameMap, SPRITE_SHEET_TOP_DOWN_PATH);
        return spritesTopDownNameMap[COLOR_TABLE_TOP_DOWN_SPRITE_NAME_MAP[color]];
    }

    public static Sprite GetStoolSprite(SeatColor color)
    {
        EnsureSpritesLoaded(spritesNameMap, SPRITE_SHEET_PATH);
        return spritesNameMap[COLOR_STOOL_SPRITE_NAME_MAP[color]];
    }
}
