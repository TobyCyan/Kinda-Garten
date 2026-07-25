using UnityEngine;

[CreateAssetMenu(fileName = "GameConfigs", menuName = "Scriptable Objects/GameConfigs")]
public class GameConfigs : ScriptableObject
{
    [Header("Kid Spawn Manager Configs")]
    public bool IsKidSpawnActive;
    public float MinNextSpawnTime = 1.0f;
    public float MaxNextSpawnTime = 6.0f;
    public float MinMoodTimer;
    public float MaxMoodTimer;
    public float MinCooldownTimer;
    public float MaxCooldownTimer;

    [Header("Trash Pile Spawn Manager Configs")]
    public bool IsTrashPileSpawnActive;
    public float MinNextSpawnTimeTrashPile = 5.0f;
    public float MaxNextSpawnTimeTrashPile = 10.0f;
}
