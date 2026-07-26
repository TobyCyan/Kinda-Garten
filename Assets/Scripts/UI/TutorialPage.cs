using DG.Tweening;
using UnityEngine;

public class TutorialPage : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;

    public void HideGroup()
    {
        group.DOFade(0.0f, 0.25f).SetUpdate(true);
        group.blocksRaycasts = false;
    }

    public void ShowGroup()
    {
        group.DOFade(1.0f, 0.25f).SetUpdate(true);
        group.blocksRaycasts = true;
    }

}
