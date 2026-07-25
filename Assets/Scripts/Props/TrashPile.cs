using System;
using UnityEngine;

public class TrashPile : MonoBehaviour, IHoldInteractable
{
    [SerializeField] private float cleanupTime = 2.5f;
    [SerializeField] private ProgressBar progressBar;
    private float cleanupProgress = 0f;
    public event Action<float, float> OnHoldProgressUpdated;
    public event Action OnHoldCompleted;
    
    void Start()
    {
        if (progressBar != null)
        {
            progressBar.HideBar();
        }
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
        cleanupProgress += Time.deltaTime;
        progressBar.UpdateFill(cleanupProgress, cleanupTime);
        if (cleanupProgress >= cleanupTime)
        {
            OnHoldCompleted?.Invoke();
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
        cleanupProgress = 0f;
    }
}