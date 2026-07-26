using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ImageFadeWidget : MonoBehaviour
{
    [SerializeField] private Image panel;
    [SerializeField] private float fadeOutDuration;
    [SerializeField] private float fadeInDuration;

    public async Task FadeIn()
    {
        panel.raycastTarget = true;
        await panel.DOFade(1.0f, fadeInDuration).SetUpdate(true).AsyncWaitForCompletion();
    }

    public async Task FadeOut()
    {
        await panel.DOFade(0.0f, fadeOutDuration).SetUpdate(true).AsyncWaitForCompletion();
        panel.raycastTarget = false;
    }
}
