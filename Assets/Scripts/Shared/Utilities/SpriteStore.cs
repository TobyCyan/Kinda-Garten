using System.Collections.Generic;
using UnityEngine;

public class SpriteStore
{
    protected static void EnsureSpritesLoaded(Dictionary<string, Sprite> spritesNameMap, string path)
    {
        if (spritesNameMap.Count > 0) return;

        var sprites = Resources.LoadAll<Sprite>(path);

        foreach (var sprite in sprites)
        {
            spritesNameMap[sprite.name] = sprite;
        }
    }
}
