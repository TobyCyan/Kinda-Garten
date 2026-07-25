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
    }

    private async void OnPlayClicked()
    {
        playButton.onClick.RemoveAllListeners();

        await fadeWidget.FadeIn();

        SceneTransitionManager.LoadNextScene();
    }
}
