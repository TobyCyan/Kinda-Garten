using UnityEngine;
using UnityEngine.UI;

public abstract class ScreenWidget : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private CanvasGroup screenCanvas;

    void Start()
    {
        button.onClick.AddListener(ButtonClick);
    }

    public void OpenScreen()
    {
        screenCanvas.alpha = 1.0f;
        screenCanvas.blocksRaycasts = true;
        OnOpenScreen();
    }

    public void CloseScreen()
    {
        screenCanvas.alpha = 0.0f;
        screenCanvas.blocksRaycasts = false;
        OnCloseScreen();
    }

    private void ButtonClick()
    {
        OnButtonClick();
    }

    protected abstract void OnOpenScreen();
    protected abstract void OnCloseScreen();
    public abstract void OnButtonClick();
}
