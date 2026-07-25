using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 20.0f;
    [SerializeField] private InputActionReference moveActionReference;
    [SerializeField] private Tilemap walkable;
    [SerializeField] private Tilemap obstacles;

    private Vector3 targetWorldPos;
    private bool isMoving = false;

    private void Start() {
        Vector3Int currentCell = walkable.WorldToCell(transform.position);
        targetWorldPos = walkable.GetCellCenterWorld(currentCell);
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

    private void Move(Vector3Int gridDirection) {
        if (isMoving) { return; }

        Vector3Int currentCell = walkable.WorldToCell(transform.position);
        Vector3Int targetCell = currentCell + gridDirection;

        if (CanMove(targetCell)) {
            targetWorldPos = walkable.GetCellCenterWorld(targetCell);
            isMoving = true;
        }
    }

    private bool CanMove(Vector3Int targetCell) {
        if (obstacles != null && obstacles.HasTile(targetCell)) {
            return false;
        }

        if (walkable != null && !walkable.HasTile(targetCell)) {
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

    private void OnMovePerformed(InputAction.CallbackContext ctx) {
        if (isMoving) { return; }

        Vector2 rawInput = ctx.ReadValue<Vector2>();

        int inputX = Mathf.RoundToInt(rawInput.x);
        int inputY = Mathf.RoundToInt(rawInput.y);

        if (inputX != 0 && inputY != 0) {
            if (Mathf.Abs(rawInput.x) > Mathf.Abs(rawInput.y)) {
                inputY = 0;
            } else {
                inputX = 0;
            }
        }

        if (inputX == 0 && inputY == 0) { return; }

        Move(new Vector3Int(inputX, inputY, 0));
    }
}
