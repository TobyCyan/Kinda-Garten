using UnityEngine;

public class WinWidget : ScreenWidget
{
    [SerializeField] private ImageFadeWidget fadeWidget;
    public override void OnButtonClick()
    {
        GoToNextDay();
    }

    protected override void OnCloseScreen()
    {

    }

    protected override void OnOpenScreen()
    {

    }

    private async void GoToNextDay()
    {
        await fadeWidget.FadeIn();
        SceneTransitionManager.LoadNextScene();
    }
}
