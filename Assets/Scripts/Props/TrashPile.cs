using System;
using UnityEngine;

public class TrashPile : MonoBehaviour, IHoldInteractable
{
    [SerializeField] private float cleanupTime = 2.5f;
    private float cleanupProgress = 0f;
    public event Action<float, float> OnHoldProgressUpdated;
    public event Action OnHoldCompleted;

    public void DoWhileHold()
    {
        cleanupProgress += Time.deltaTime;
        OnHoldProgressUpdated?.Invoke(cleanupProgress, cleanupTime);
        if (cleanupProgress >= cleanupTime)
        {
            OnHoldCompleted?.Invoke();
            Destroy(gameObject);
        }
    }

    public void DoOnRelease()
    {
        cleanupProgress = 0f;
    }
}