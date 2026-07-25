using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ImageFadeWidget : MonoBehaviour
{
    [SerializeField] private Image panel;
    [SerializeField] private float fadeOutDuration;
    [SerializeField] private float fadeInDuration;

    public Task FadeIn()
    {
        return panel.DOFade(1.0f, fadeInDuration).AsyncWaitForCompletion();
    }

    public Task FadeOut()
    {
        return panel.DOFade(0.0f, fadeOutDuration).AsyncWaitForCompletion();
    }
}
