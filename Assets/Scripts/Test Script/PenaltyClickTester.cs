using UnityEngine;

/// <summary>
/// Development-only helper for testing the penalty system.
/// Attach this to a GameObject with a Collider2D and click it in Play Mode.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class PenaltyClickTester : MonoBehaviour
{
    private void OnMouseDown()
    {
        if (PenaltyManager.Instance == null)
        {
            Debug.LogError(
                "PenaltyClickTester could not find a PenaltyManager in the scene.",
                this);
            return;
        }

        PenaltyManager.Instance.AddPenalty();

        Debug.Log(
            $"Penalty added: {PenaltyManager.Instance.PenaltyCount}/" +
            $"{PenaltyManager.Instance.MaximumPenalties}",
            this);
    }
}
