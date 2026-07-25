using UnityEngine;

public class ThankYouWidget : ScreenWidget
{
    [SerializeField] private ImageFadeWidget fadeWidget;

    public override void OnButtonClick()
    {
        ReturnHome();
    }

    protected override void OnCloseScreen()
    {

    }

    protected override void OnOpenScreen()
    {

    }

    private async void ReturnHome()
    {
        await fadeWidget.FadeIn();
        SceneTransitionManager.LoadHome();
    }
}
