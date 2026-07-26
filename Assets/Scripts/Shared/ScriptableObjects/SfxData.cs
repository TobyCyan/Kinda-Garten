using UnityEngine;

[CreateAssetMenu(fileName = "SfxData", menuName = "Scriptable Objects/SfxData")]
public class SfxData : ScriptableObject
{
    public SfxId id;
    public AudioClip clip;
}

public enum SfxId
{
    ButtonClick,
    CrashOut,
    MiniGameSuccess,
    MiniGameFail,
    Penalty,
    LevelSuccess,
    GameOver,
}
