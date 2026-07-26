using System;
using UnityEngine;

public class DayProgressBarWidget : MonoBehaviour
{
    [SerializeField] private ProgressBar bar;
    [SerializeField] private CanvasGroup group;
    [SerializeField] private float maxDurationDay;

    private float currentDuration;

    private bool hasInit = false;

    public Action DayFinish;
    public Action RushHourReached;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentDuration = 0;
    }

    public void Setup()
    {
        hasInit = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStates.IsPaused || GameStates.IsGameFinish) return;

        if (!hasInit) return;

        currentDuration += Time.deltaTime;
        bar.UpdateFill(currentDuration, maxDurationDay);

        if (currentDuration >= maxDurationDay)
        {
            SfxManager.Instance.Play(SfxId.LevelSuccess);
            DayFinish?.Invoke();
        }
    }

    public void HideBar()
    {
        group.alpha = 0.0f;
    }

    public void ShowBar()
    {
        group.alpha = 1.0f;
    }
}
