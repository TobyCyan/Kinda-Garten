using System;
using UnityEngine;

public class TrashPile : MonoBehaviour, IHoldInteractable
{
    [SerializeField] private float cleanupTime = 2.5f;
    private float cleanupProgress = 0f;
    public event Action OnCleanupProgressChanged;

    public void DoWhileHold()
    {
        cleanupProgress += Time.deltaTime;
        OnCleanupProgressChanged?.Invoke();
        if (cleanupProgress >= cleanupTime)
        {
            Destroy(gameObject);
        }
    }

    public void DoOnRelease()
    {
        cleanupProgress = 0f;
    }
}