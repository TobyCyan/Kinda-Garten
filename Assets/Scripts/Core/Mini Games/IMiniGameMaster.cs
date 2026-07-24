using System;

/// <summary>
/// Interface for mini-game masters, which are responsible for generating and cleaning up mini-games.
/// Game masters should decide how is the mini-game considered completed and invoke the event.
/// </summary>
interface IMiniGameMaster
{
    public event Action OnMiniGameCompleted;

    void GenerateMiniGame();
    void CleanUpMiniGame();
}
