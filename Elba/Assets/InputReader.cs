using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    private PlayerInputActions inputActions;

    public Vector2 MoveInput { get; private set; }

    public event Action OnInteract;
    public event Action OnInventory;
    public event Action OnAttack;
    public event Action OnConsumePickup;

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

        inputActions.UI.ToggleInventory.performed += _ => OnInventory?.Invoke();
        inputActions.Player.Consume.performed +=
    _ => OnConsumePickup?.Invoke();
    }

    private void OnDisable()
    {
        inputActions.Disable();

        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        MoveInput = context.ReadValue<Vector2>();
    }
}