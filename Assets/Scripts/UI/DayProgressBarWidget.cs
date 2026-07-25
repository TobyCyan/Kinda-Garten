using UnityEngine;

public class DayProgressBarWidget : MonoBehaviour
{
    [SerializeField] private ProgressBar bar;

    private float currentDuration;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentDuration = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
