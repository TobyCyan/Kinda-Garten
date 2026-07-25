using System;
using UnityEngine;

public class Alarm : MonoBehaviour, IHoldInteractable
{
    [SerializeField] private float turnOffTime = 2.5f;
    [SerializeField] private ProgressBar progressBar;
    [SerializeField] private SpriteRenderer spriteRenderer; // Drag your sprite renderer here in the inspector
    
    private float turnOffProgress = 0f;
    public event Action OnHoldCompleted;
    
    public bool IsRinging { get; private set; } = false;
    
    void Start()
    {
        if (progressBar != null)
        {
            progressBar.HideBar();
        }
    }

    // remove the whole update method when deploy
    #region TEMPORARY TESTING
    private void Update()
    {
        // 1. When you first press Enter, trigger the initial hold setup
        if (Input.GetKeyDown(KeyCode.Return))
        {
            DoOnHold();
        }
        
        // 2. While you keep holding Enter, fill the bar
        if (Input.GetKey(KeyCode.Return))
        {
            DoWhileHold();
        }
        
        // 3. If you let go of Enter early, reset the bar
        if (Input.GetKeyUp(KeyCode.Return))
        {
            DoOnRelease();
        }
    }
    #endregion

    // Called by the AlarmManager
    public void TriggerRing()
    {
        if (IsRinging) return;

        IsRinging = true;
        
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red; // todo: remove this after verfying this is working
        }
    }

    public void DoOnHold()
    {
        if (!IsRinging) return;

        if (progressBar != null)
        {
            progressBar.ShowBar();
        }
    }

    public void DoWhileHold()
    {
        if (!IsRinging) return;

        turnOffProgress += Time.deltaTime;
        
        if (progressBar != null)
        {
            progressBar.UpdateFill(turnOffProgress, turnOffTime);
        }
        
        if (turnOffProgress >= turnOffTime)
        {
            OnHoldCompleted?.Invoke();
            
            IsRinging = false;
            turnOffProgress = 0f;
            
            if (progressBar != null)
            {
                progressBar.HideBar();
                progressBar.ResetFill();
            }

            if (spriteRenderer != null)
            {
                spriteRenderer.color = Color.white; // todo: remove this after verfying this is working
            }
            
            if (AlarmManager.Instance != null)
            {
                AlarmManager.Instance.StopAlarm();
            }
        }
    }

    public void DoOnRelease()
    {
        if (!IsRinging) return;

        if (progressBar != null)
        {
            progressBar.HideBar();
            progressBar.ResetFill();
        }
        turnOffProgress = 0f;
    }
}