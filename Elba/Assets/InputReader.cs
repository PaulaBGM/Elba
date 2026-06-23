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

    public event Action OnStorePickup;
    public event Action OnDropPickup;
    public event Action OnConsumePickup;

    public event Action OnUISelect;
    public event Action OnUIRightClick;
    public event Action OnSubmitPressed;

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

        inputActions.Player.Interact.performed += OnInteractPerformed;
        inputActions.Player.Attack.performed += OnAttackPerformed;

        inputActions.UI.ToggleInventory.performed += OnInventoryPerformed;
        inputActions.UI.Select.performed += OnUISelectPerformed;
        inputActions.UI.RightClick.performed += OnUIRightClickPerformed;
        inputActions.UI.Submit.performed += OnSubmitPerformed;
        inputActions.Player.Store.performed += OnStorePerformed;
        inputActions.Player.Drop.performed += OnDropPerformed;
        inputActions.Player.Consume.performed += OnConsumePerformed;

        inputActions.UI.Point.performed += OnPoint;
        inputActions.UI.Point.canceled += OnPoint;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;

        inputActions.Player.Interact.performed -= OnInteractPerformed;
        inputActions.Player.Attack.performed -= OnAttackPerformed;

        inputActions.UI.ToggleInventory.performed -= OnInventoryPerformed;
        inputActions.UI.Select.performed -= OnUISelectPerformed;
        inputActions.UI.RightClick.performed -= OnUIRightClickPerformed;
        inputActions.UI.Submit.performed -= OnSubmitPerformed;
        inputActions.Player.Store.performed -= OnStorePerformed;
        inputActions.Player.Drop.performed -= OnDropPerformed;
        inputActions.Player.Consume.performed -= OnConsumePerformed;

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

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        OnInteract?.Invoke();
    }

    private void OnAttackPerformed(InputAction.CallbackContext context)
    {
        OnAttack?.Invoke();
    }

    private void OnConsumePerformed(InputAction.CallbackContext context)
    {
        OnConsumePickup?.Invoke();
    }

    private void OnInventoryPerformed(InputAction.CallbackContext context)
    {
        OnInventory?.Invoke();
    }

    private void OnUISelectPerformed(InputAction.CallbackContext context)
    {
        OnUISelect?.Invoke();
    }

    private void OnUIRightClickPerformed(InputAction.CallbackContext context)
    {
        OnUIRightClick?.Invoke();
    }

    private void OnSubmitPerformed(InputAction.CallbackContext context)
    {
        OnSubmitPressed?.Invoke();
    }
    private void OnStorePerformed(InputAction.CallbackContext context)
    {
        OnStorePickup?.Invoke();
    }

    private void OnDropPerformed(InputAction.CallbackContext context)
    {
        OnDropPickup?.Invoke();
    }
}