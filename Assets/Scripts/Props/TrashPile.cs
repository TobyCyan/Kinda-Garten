using System;
using UnityEngine;

public class TrashPile : MonoBehaviour, IHoldInteractable
{
    private float cleanUpTime = 1.0f;
    [SerializeField] private ProgressBar progressBar;
    [SerializeField] private SpriteRenderer renderer;
    private float cleanupProgress = 0f;
    public event Action OnHoldCompleted;
    
    void Start()
    {
        if (progressBar != null)
        {
            progressBar.HideBar();
        }
    }

    public void Init(float cleanUpTime)
    {
        this.cleanUpTime = cleanUpTime;
        if (progressBar != null)
        {
            progressBar.HideBar();
        }

        if (renderer != null)
        {
            renderer.sprite = TrashPileSpriteStore.GetRandomSprite();
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
        progressBar.UpdateFill(cleanupProgress, cleanUpTime);
        if (cleanupProgress >= cleanUpTime)
        {
            OnHoldCompleted?.Invoke();
            OnHoldCompleted = null;
            SfxManager.Instance.Play(SfxId.TrashCleaned);
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