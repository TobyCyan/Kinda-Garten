using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameOverWidget gameOverWidget;
    [SerializeField] private ThankYouWidget thankYouWidget;
    [SerializeField] private PauseWdiget pauseWdiget;
    [SerializeField] private WinWidget winWidget;
    [SerializeField] private ImageFadeWidget fadeWidget;
    [SerializeField] private InputActionReference pauseActionReference;
    [SerializeField] private TutorialWidget tutorialWidget;

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
        if(SceneManager.GetActiveScene().buildIndex == 3)
        {
            progressBarWidget.DayFinish += OpenThankYouScreen;
        }
        else
        {
            progressBarWidget.DayFinish += OpenWinScreen;
        }
    }

    public async void Init()
    {
        if(tutorialWidget != null)
        {
            tutorialWidget.OpenScreen();
            tutorialWidget.OnScreenClose += StartCountdown;
        }
        else
        {
            StartCountdown(null);
        }
        await fadeWidget.FadeOut();
    }
    private void StartCountdown(ScreenWidget wdiget)
    {
        pauseWdiget.OpenScreen();
        pauseWdiget.StartWithCountdown();
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

    public void HideBarWidget()
    {
        progressBarWidget.HideBar();
    }

    public void ShowBarWidget()
    {
        progressBarWidget.ShowBar();
    }
}
