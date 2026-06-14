using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    private PlayerInputActions inputActions;

    public Vector2 MoveInput { get; private set; }
    public Vector2 MousePosition { get; private set; }

    public event Action OnInteract;
    public event Action OnInventory;
    public event Action OnAttack;
    public event Action OnConsumePickup;
    public event Action OnUISelect;
    public event Action OnUIRightClick;

    public bool SprintHeld =>
        inputActions.Player.Sprint.IsPressed();

    private void Awake()
    {
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        inputActions.Enable();

        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;

        inputActions.Player.Interact.performed += _ => OnInteract?.Invoke();
        inputActions.Player.Attack.performed += _ => OnAttack?.Invoke();
        inputActions.Player.Consume.performed += _ => OnConsumePickup?.Invoke();

        inputActions.UI.ToggleInventory.performed += _ => OnInventory?.Invoke();

        inputActions.UI.Select.performed += _ => OnUISelect?.Invoke();
        inputActions.UI.RightClick.performed += _ => OnUIRightClick?.Invoke();

        inputActions.UI.Point.performed += OnPoint;
        inputActions.UI.Point.canceled += OnPoint;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;

        inputActions.UI.Point.performed -= OnPoint;
        inputActions.UI.Point.canceled -= OnPoint;

        inputActions.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }

    private void OnPoint(InputAction.CallbackContext context)
    {
        MousePosition = context.ReadValue<Vector2>();
    }
}