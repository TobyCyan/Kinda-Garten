using UnityEngine;

[CreateAssetMenu(fileName = "GameConfigs", menuName = "Scriptable Objects/GameConfigs")]
public class GameConfigs : ScriptableObject
{
    [Header("Kid Spawn Manager Configs")]
    public bool IsKidSpawnActive;
    public float MinNextSpawnTime = 1.0f;
    public float MaxNextSpawnTime = 6.0f;
    public float MinMoodTimer = 5.0f;
    public float MaxMoodTimer = 10.0f;
    public float MinCooldownTimer = 3.0f;
    public float MaxCooldownTimer = 5.0f;

    [Header("Trash Pile Spawn Manager Configs")]
    public bool IsTrashPileSpawnActive;
    public float MinNextSpawnTimeTrashPile = 5.0f;
    public float MaxNextSpawnTimeTrashPile = 10.0f;
}
