using UnityEngine;
using UnityEngine.UI;

public class HomeSceneWidget : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private ImageFadeWidget fadeWidget;

    private void Awake()
    {
        fadeWidget.FadeOut();
    }

    private void Start()
    {
        playButton.onClick.AddListener(OnPlayClicked);
        SfxManager.Instance.Play(SfxId.BackgroundMusic);
    }

    private async void OnPlayClicked()
    {
        SfxManager.Instance.Play(SfxId.ButtonClick);
        playButton.onClick.RemoveAllListeners();

        await fadeWidget.FadeIn();

        SceneTransitionManager.LoadNextScene();
    }
}
