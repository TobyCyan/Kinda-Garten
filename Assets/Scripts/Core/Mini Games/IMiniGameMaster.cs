using System;

interface IMiniGameMaster
{
    public event Action OnMiniGameCompleted;

    void GenerateMiniGame();
    void CleanUpMiniGame();
}
