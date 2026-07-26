using System.Collections.Generic;
using UnityEngine;

public class TrashPileSpriteStore : SpriteStore
{
    private const string SPRITE_SHEET_PATH = "Sprites/Props/Trash Piles";

    private static readonly Dictionary<string, Sprite> spritesNameMap = new();

    public static Sprite GetRandomSprite()
    {
        EnsureSpritesLoaded(spritesNameMap, SPRITE_SHEET_PATH);
        var randomIndex = Random.Range(0, spritesNameMap.Count);
        var randomSprite = new List<Sprite>(spritesNameMap.Values)[randomIndex];
        return randomSprite;
    }
}
