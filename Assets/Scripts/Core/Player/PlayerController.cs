using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 20.0f;
    [SerializeField] private InputActionReference moveActionReference;
    [SerializeField] private InputActionReference holdActionReference;
    [SerializeField] private InputActionReference interactActionReference;
    [SerializeField] private Tilemap walkable;
    [SerializeField] private Tilemap obstacle;
    private bool isActive = true;
    private Vector3 targetWorldPos;
    private bool isMoving = false;

    private bool isHolding = false;
    private IHoldInteractable currentHoldInteractable;
    private Seat currentInteractableSeat;
    

    private void Start()
    {
        Vector3Int currentCell = walkable.WorldToCell(transform.position);
        targetWorldPos = walkable.GetCellCenterWorld(currentCell);
        transform.position = targetWorldPos;
    }

    private void Update()
    {
        if (GameStates.IsGameFinish || GameStates.IsPaused) return;

        if (!isActive) { return; }
        
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

        Vector3Int currentCell = walkable.WorldToCell(transform.position);
        Vector3Int targetCell = currentCell + gridDirection;

        if (CanMove(targetCell))
        {
            targetWorldPos = walkable.GetCellCenterWorld(targetCell);
            isMoving = true;
        }
    }

    private bool CanMove(Vector3Int targetCell)
    {
        if (obstacle != null && obstacle.HasTile(targetCell))
        {
            return false;
        }

        if (walkable != null && !walkable.HasTile(targetCell))
        {
            return false;
        }

        if (GameManager.Instance.IsCellOccupied(targetCell))
        {
            return false;
        }

        return true;
    }

    public void SetActive(bool isActive)
    {
        this.isActive = isActive;
    }

    private void OnEnable()
    {
        moveActionReference.action.Enable();
        moveActionReference.action.performed += OnMovePerformed;

        holdActionReference.action.Enable();
        holdActionReference.action.performed += OnHoldPerformed;
        holdActionReference.action.canceled += OnHoldCanceled;

        interactActionReference.action.Enable();
        interactActionReference.action.performed += OnInteractPerformed;
    }

    private void OnDisable()
    {
        moveActionReference.action.performed -= OnMovePerformed;
        moveActionReference.action.Disable();

        holdActionReference.action.performed -= OnHoldPerformed;
        holdActionReference.action.canceled -= OnHoldCanceled;
        holdActionReference.action.Disable();

        interactActionReference.action.performed -= OnInteractPerformed;
        interactActionReference.action.Disable();
    }

    private void OnInteractPerformed(InputAction.CallbackContext ctx) 
    {
        currentInteractableSeat.TriggerMiniGame();
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
        isHolding = true;
        currentHoldInteractable?.DoOnHold();
    }

    private void OnHoldCanceled(InputAction.CallbackContext ctx)
    {
        HoldCleanUp();
    }

    private void HoldCleanUp()
    {
        isHolding = false;
        currentHoldInteractable?.DoOnRelease();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IHoldInteractable>(out var holdInteractable))
        {
            // TODO: Show UI prompt for holding interaction (e.g., "Hold E to interact")
            currentHoldInteractable = holdInteractable;
            currentHoldInteractable.OnHoldCompleted += HoldCleanUp;
        }

        if (collision.TryGetComponent<Seat>(out var seat)) 
        {
            currentInteractableSeat = seat;
            currentInteractableSeat.IsInteractable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<IHoldInteractable>(out var holdInteractable))
        {
            // TODO: Hide UI prompt for holding interaction
            if (currentHoldInteractable == holdInteractable)
            {
                HoldCleanUp();
                currentHoldInteractable.OnHoldCompleted -= HoldCleanUp;
                currentHoldInteractable = null;
            }
        }

        if (collision.TryGetComponent<Seat>(out var seat)) 
        {
            currentInteractableSeat = seat;
            currentInteractableSeat.IsInteractable = false;
        }
    }
}
