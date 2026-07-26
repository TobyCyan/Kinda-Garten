using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class ScreenWidget : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private CanvasGroup screenCanvas;

    public event Action<ScreenWidget> OnScreenOpen;
    public event Action<ScreenWidget> OnScreenClose;
    void Start()
    {
        button.onClick.AddListener(ButtonClick);
    }

    public void OpenScreen()
    {
        screenCanvas.alpha = 1.0f;
        screenCanvas.blocksRaycasts = true;
        OnOpenScreen();

        OnScreenOpen?.Invoke(this);
    }

    public void CloseScreen()
    {
        screenCanvas.alpha = 0.0f;
        screenCanvas.blocksRaycasts = false;
        OnCloseScreen();

        OnScreenClose?.Invoke(this);
    }

    private void ButtonClick()
    {
        SfxManager.Instance.Play(SfxId.ButtonClick);
        OnButtonClick();
    }

    protected abstract void OnOpenScreen();
    protected abstract void OnCloseScreen();
    public abstract void OnButtonClick();
}
