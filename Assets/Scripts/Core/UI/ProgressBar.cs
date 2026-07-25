using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    
    public void UpdateFill(float progress, float target)
    {
        float newFillAmount = progress / target;
        slider.value = Mathf.Clamp01(newFillAmount);
    }

    public void ResetFill()
    {
        slider.value = 0f;
    }

    public void ShowBar()
    {
        slider.gameObject.SetActive(true);
    }

    public void HideBar()
    {
        slider.gameObject.SetActive(false);
    }
}
