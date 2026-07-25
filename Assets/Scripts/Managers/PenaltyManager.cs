using System;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Keeps track of the classroom's failed-kid count.
/// One failed kid equals one penalty. Reaching the maximum ends the run.
/// </summary>
public class PenaltyManager : MonoBehaviour
{
    public static PenaltyManager Instance { get; private set; }

    [Header("Penalty Settings")]
    [SerializeField, Min(1)] private int maximumPenalties = 3;

    [Tooltip("Assign the three cross images in order. They should start inactive.")]
    [SerializeField] private GameObject[] penaltyCrosses;

    [Header("Inspector Events")]
    [SerializeField] private UnityEvent onGameFailed;

    public int PenaltyCount { get; private set; }
    public int MaximumPenalties => maximumPenalties;
    public bool HasGameFailed { get; private set; }

    // Code can subscribe to these without needing a reference to this component.
    public event Action<int> OnPenaltyCountChanged;
    public event Action OnGameFailed;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        RefreshPenaltyDisplay();
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }

    /// <summary>
    /// Call this once when a kid's mood timer completes.
    /// This method can also be connected directly to a UnityEvent in the Inspector.
    /// </summary>
    public void AddPenalty()
    {
        if (HasGameFailed)
        {
            return;
        }

        PenaltyCount++;
        RefreshPenaltyDisplay();
        OnPenaltyCountChanged?.Invoke(PenaltyCount);

        if (PenaltyCount >= maximumPenalties)
        {
            FailGame();
        }
    }

    /// <summary>
    /// Starts a new run with no penalties.
    /// </summary>
    public void ResetPenalties()
    {
        PenaltyCount = 0;
        HasGameFailed = false;
        RefreshPenaltyDisplay();
        OnPenaltyCountChanged?.Invoke(PenaltyCount);
    }

    private void RefreshPenaltyDisplay()
    {
        if (penaltyCrosses == null)
        {
            return;
        }

        for (int i = 0; i < penaltyCrosses.Length; i++)
        {
            if (penaltyCrosses[i] != null)
            {
                penaltyCrosses[i].SetActive(i < PenaltyCount);
            }
        }
    }

    private void FailGame()
    {
        HasGameFailed = true;
        OnGameFailed?.Invoke();
        onGameFailed?.Invoke();
    }
}
