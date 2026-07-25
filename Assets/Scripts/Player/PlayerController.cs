using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 20.0f;
    //[SerializeField] private Transform playerMovePoint;
    [SerializeField] private InputActionReference moveActionReference;
    [SerializeField] private Tilemap floor;
    [SerializeField] private Tilemap seat;

    private Vector3 targetWorldPos;
    private bool isMoving = false;

    private void Start() {
        Vector3Int currentCell = floor.WorldToCell(transform.position);
        targetWorldPos = floor.GetCellCenterWorld(currentCell);
        transform.position = targetWorldPos;
    }

    private void Update() {
        if (!isMoving) { return; }

        transform.position = Vector3.MoveTowards(transform.position, targetWorldPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWorldPos) < 0.001f) {
            transform.position = targetWorldPos;
            isMoving = false;
        }
    }

    private void Move(Vector2 direction) {
        if (isMoving) { return; }

        Vector3Int targetCell = floor.WorldToCell(transform.position + (Vector3)direction);

        if (CanMove(targetCell)) {
            targetWorldPos = floor.GetCellCenterWorld(targetCell);
            isMoving = true;
        }
    }

    private bool CanMove(Vector3Int targetCell) {
        if (seat != null && seat.HasTile(targetCell)) {
            return false;
        }

        if (floor != null && !floor.HasTile(targetCell)) {
            return false;
        }

        return true;
    }

    private void OnEnable() {
        moveActionReference.action.Enable();
        moveActionReference.action.performed += OnMovePerformed;
    }

    private void OnDisable() {
        moveActionReference.action.performed -= OnMovePerformed;
        moveActionReference.action.Disable();
    }

    //need change
    private void OnMovePerformed(InputAction.CallbackContext ctx) {
        if (!isMoving) {
            Move(ctx.ReadValue<Vector2>());
        }
    }
}
