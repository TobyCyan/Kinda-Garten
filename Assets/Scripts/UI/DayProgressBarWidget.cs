using System;
using UnityEngine;

public class DayProgressBarWidget : MonoBehaviour
{
    [SerializeField] private ProgressBar bar;
    [SerializeField] private float maxDurationDay;
    [SerializeField] private float rushHourTriggerMark;

    private float currentDuration;

    private bool hasInit = false;
    private bool hasTriggerRushHour = false;
    private bool isDayFinished = false;

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

        if (!hasInit || isDayFinished) return;

        currentDuration += Time.deltaTime;
        bar.UpdateFill(currentDuration, maxDurationDay);

        //if(!hasTriggerRushHour)
        //{
        //    if(currentDuration >= rushHourTriggerMark)
        //    {
        //        hasTriggerRushHour = true;
        //        RushHourReached?.Invoke();
        //    }
        //}

        if (currentDuration >= maxDurationDay)
        {
            DayFinish?.Invoke();
        }
    }
}
