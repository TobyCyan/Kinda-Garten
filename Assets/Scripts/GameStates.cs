using UnityEngine;

public static class GameStates
{
    public static bool IsPaused = false;
    public static bool IsGameFinish = false;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void Reset()
    {
        IsPaused = false;
        IsGameFinish = false;
    }
}
