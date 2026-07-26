using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameOverWidget gameOverWidget;
    [SerializeField] private ThankYouWidget thankYouWidget;
    [SerializeField] private PauseWdiget pauseWdiget;
    [SerializeField] private WinWidget winWidget;
    [SerializeField] private ImageFadeWidget fadeWidget;
    [SerializeField] private InputActionReference pauseActionReference;

    [SerializeField] private DayProgressBarWidget progressBarWidget;

    public event Action OnGameFinish;

    private void OnEnable()
    {
        pauseActionReference.action.Enable();
        pauseActionReference.action.performed += OpenPauseScreen;
    }

    private void OnDisable()
    {
        pauseActionReference.action.performed -= OpenPauseScreen;
        pauseActionReference.action.Disable();
    }
    public void InitConfigs()
    {
        progressBarWidget.Setup();
        progressBarWidget.DayFinish += OpenWinScreen;
    }

    public async void Init()
    {
        pauseWdiget.OpenScreen();
        pauseWdiget.StartWithCountdown();
        await fadeWidget.FadeOut();
    }

    public void OpenGameOverScreen()
    {
        GameStates.IsGameFinish = true;
        gameOverWidget.OpenScreen();
    }

    public void OpenThankYouScreen()
    {
        GameStates.IsGameFinish = true;
        thankYouWidget.OpenScreen();
    }
    public void OpenWinScreen()
    {
        GameStates.IsGameFinish = true;
        winWidget.OpenScreen();
    }

    public void OpenPauseScreen(InputAction.CallbackContext ctx)
    {
        if (GameStates.IsGameFinish || GameStates.IsPaused) return;
        pauseWdiget.OpenScreen();
    }
}
