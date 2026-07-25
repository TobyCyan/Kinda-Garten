using System;
using UnityEngine;

public class Alarm : MonoBehaviour, IHoldInteractable
{
    [SerializeField] private float turnOffTime = 2.5f;
    [SerializeField] private ProgressBar progressBar;
    
    private float turnOffProgress = 0f;
    public event Action OnHoldCompleted;

    private Seat assignedSeat; 
    
    void Start()
    {
        if (progressBar != null)
        {
            progressBar.HideBar();
        }
    }

    public void Init(Seat seat)
    {
        assignedSeat = seat;
    }

    public void DoOnHold()
    {
        if (progressBar != null)
        {
            progressBar.ShowBar();
        }
    }

    public void DoWhileHold()
    {
        turnOffProgress += Time.deltaTime;
        progressBar.UpdateFill(turnOffProgress, turnOffTime);
        
        if (turnOffProgress >= turnOffTime)
        {
            OnHoldCompleted?.Invoke();
            
            if (AlarmManager.Instance != null)
            {
                AlarmManager.Instance.StopAlarm();
            }
            
            // Free up the seat for future alarms
            if (assignedSeat != null)
            {
                assignedSeat.IsAlarmOccupied = false;
            }
            
            Destroy(gameObject);
        }
    }

    public void DoOnRelease()
    {
        if (progressBar != null)
        {
            progressBar.HideBar();
            progressBar.ResetFill();
        }
        turnOffProgress = 0f;
    }
}