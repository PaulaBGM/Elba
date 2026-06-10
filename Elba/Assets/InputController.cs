using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputController : BaseSingleton<InputController>
{
    public event Action OnPausePressed;
    public event Action OnSubmitPressed;
    public event Action OnClickPressed;
    public event Action OnMapPressed;
    public event Action OnInventoryPressed;
    public event Action OnDetectionPressed;

    public event Action<Vector2> OnMovePerformed;
    public event Action<bool> OnSprintPerformed;

    private PlayerInput playerInput;

    private InputAction pauseAction;
    private InputAction submitAction;
    private InputAction clickAction;
    private InputAction mapAction;
    private InputAction inventoryAction;
    private InputAction detectionAction;

    private InputAction moveAction;
    private InputAction sprintAction;

    protected override void Awake()
    {
        base.Awake();

        playerInput = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        pauseAction = playerInput.actions["PauseMenu"];
        submitAction = playerInput.actions["Submit"];
        clickAction = playerInput.actions["Click"];
        mapAction = playerInput.actions["Map"];
        inventoryAction = playerInput.actions["Inventory"];
        detectionAction = playerInput.actions["Detection"];

        moveAction = playerInput.actions["Move"];
        sprintAction = playerInput.actions["Sprint"];

        pauseAction.performed += OnPause;
        submitAction.performed += OnSubmit;
        clickAction.performed += OnClick;
        mapAction.performed += OnMap;
        inventoryAction.performed += OnInventory;
        detectionAction.performed += OnDetection;

        moveAction.performed += OnMove;
        moveAction.canceled += OnMoveCanceled;

        sprintAction.performed += OnSprint;
        sprintAction.canceled += OnSprint;
    }

    private void OnDisable()
    {
        pauseAction.performed -= OnPause;
        submitAction.performed -= OnSubmit;
        clickAction.performed -= OnClick;
        mapAction.performed -= OnMap;
        inventoryAction.performed -= OnInventory;
        detectionAction.performed -= OnDetection;

        moveAction.performed -= OnMove;
        moveAction.canceled -= OnMoveCanceled;

        sprintAction.performed -= OnSprint;
        sprintAction.canceled -= OnSprint;
    }

    private void OnPause(InputAction.CallbackContext ctx)
    {
        OnPausePressed?.Invoke();
    }

    private void OnSubmit(InputAction.CallbackContext ctx)
    {
        OnSubmitPressed?.Invoke();
    }

    private void OnClick(InputAction.CallbackContext ctx)
    {
        OnClickPressed?.Invoke();
    }

    private void OnMap(InputAction.CallbackContext ctx)
    {
        OnMapPressed?.Invoke();
    }

    private void OnInventory(InputAction.CallbackContext ctx)
    {
        OnInventoryPressed?.Invoke();
    }

    private void OnDetection(InputAction.CallbackContext ctx)
    {
        OnDetectionPressed?.Invoke();
    }

    private void OnMove(InputAction.CallbackContext ctx)
    {
        Vector2 value = ctx.ReadValue<Vector2>();

        Debug.Log($"INPUT CONTROLLER: {value}");

        OnMovePerformed?.Invoke(value);
    }

    private void OnMoveCanceled(InputAction.CallbackContext ctx)
    {
        OnMovePerformed?.Invoke(Vector2.zero);
    }

    private void OnSprint(InputAction.CallbackContext ctx)
    {
        OnSprintPerformed?.Invoke(ctx.ReadValueAsButton());
    }
}