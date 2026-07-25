using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 20.0f;
    [SerializeField] private InputActionReference moveActionReference;
    [SerializeField] private InputActionReference holdActionReference;
    [SerializeField] private Tilemap floor;
    [SerializeField] private Tilemap seat;

    private Vector3 targetWorldPos;
    private bool isMoving = false;

    [SerializeField] private ProgressBar progressBar;
    private bool isHolding = false;
    private IHoldInteractable currentHoldInteractable;
    

    private void Start()
    {
        Vector3Int currentCell = floor.WorldToCell(transform.position);
        targetWorldPos = floor.GetCellCenterWorld(currentCell);
        transform.position = targetWorldPos;
        if (progressBar != null)
        {
            progressBar.HideBar();
        }
    }

    private void Update()
    {
        if (isHolding)
        {
            currentHoldInteractable?.DoWhileHold();
        }

        if (!isMoving) { return; }

        transform.position = Vector3.MoveTowards(transform.position, targetWorldPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetWorldPos) < 0.001f)
        {
            transform.position = targetWorldPos;
            isMoving = false;
        }
    }

    private void Move(Vector3Int gridDirection)
    {
        if (isMoving) { return; }

        Vector3Int currentCell = floor.WorldToCell(transform.position);
        Vector3Int targetCell = currentCell + gridDirection;

        if (CanMove(targetCell))
        {
            targetWorldPos = floor.GetCellCenterWorld(targetCell);
            isMoving = true;
        }
    }

    private bool CanMove(Vector3Int targetCell)
    {
        if (seat != null && seat.HasTile(targetCell))
        {
            return false;
        }

        if (floor != null && !floor.HasTile(targetCell))
        {
            return false;
        }

        return true;
    }

    private void OnEnable()
    {
        moveActionReference.action.Enable();
        moveActionReference.action.performed += OnMovePerformed;

        holdActionReference.action.Enable();
        holdActionReference.action.performed += OnHoldPerformed;
        holdActionReference.action.canceled += OnHoldCanceled;
    }

    private void OnDisable()
    {
        moveActionReference.action.performed -= OnMovePerformed;
        moveActionReference.action.Disable();

        holdActionReference.action.performed -= OnHoldPerformed;
        holdActionReference.action.canceled -= OnHoldCanceled;
        holdActionReference.action.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext ctx)
    {
        if (isMoving) { return; }

        Vector2 rawInput = ctx.ReadValue<Vector2>();

        int inputX = Mathf.RoundToInt(rawInput.x);
        int inputY = Mathf.RoundToInt(rawInput.y);

        if (inputX != 0 && inputY != 0)
        {
            if (Mathf.Abs(rawInput.x) > Mathf.Abs(rawInput.y))
            {
                inputY = 0;
            }
            else
            {
                inputX = 0;
            }
        }

        if (inputX == 0 && inputY == 0) { return; }

        Move(new Vector3Int(inputX, inputY, 0));
    }

    private void OnHoldPerformed(InputAction.CallbackContext ctx)
    {
        if (currentHoldInteractable != null)
        {
            isHolding = true;
            progressBar.ShowBar();
        }
    }

    private void OnHoldCanceled(InputAction.CallbackContext ctx)
    {
        isHolding = false;
        progressBar.HideBar();
        currentHoldInteractable?.DoOnRelease();
    }

    private void HoldCleanUp()
    {
        if (currentHoldInteractable != null)
        {
            isHolding = false;
            progressBar.HideBar();
            currentHoldInteractable.DoOnRelease();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IHoldInteractable>(out var holdInteractable))
        {
            // TODO: Show UI prompt for holding interaction (e.g., "Hold E to interact")
            currentHoldInteractable = holdInteractable;
            currentHoldInteractable.OnHoldProgressUpdated += progressBar.UpdateFill;
            currentHoldInteractable.OnHoldCompleted += HoldCleanUp;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IHoldInteractable>(out var holdInteractable))
        {
            // TODO: Hide UI prompt for holding interaction
            if (currentHoldInteractable == holdInteractable)
            {
                currentHoldInteractable.OnHoldProgressUpdated -= progressBar.UpdateFill;
                currentHoldInteractable.OnHoldCompleted -= HoldCleanUp;
                currentHoldInteractable = null;
            }
        }
    }
}
