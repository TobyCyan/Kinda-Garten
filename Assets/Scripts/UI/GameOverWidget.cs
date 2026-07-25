using UnityEngine;

public class GameOverWidget : ScreenWidget
{
    [SerializeField] private ImageFadeWidget fadeWidget;
    public override void OnButtonClick()
    {
        Retry();
    }

    protected override void OnCloseScreen()
    {

    }

    protected override void OnOpenScreen()
    {
    }

    private async void Retry()
    {
        await fadeWidget.FadeIn();
        SceneTransitionManager.ReloadCurrentScene();
    }
}
