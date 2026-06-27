using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject gameplayHUD;
    [SerializeField] private InventoryMenu inventoryMenu;

    [Header("Input")]
    [SerializeField] private InputReader input;

    [Header("Systems")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerInteractionSystem interactionSystem;

    public bool IsInventoryOpen { get; private set; }

    public event Action<bool> OnInventoryStateChanged;
    public event Action OnSubmitPressed;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        inventoryMenu.Hide();

        if (input == null)
            input = FindFirstObjectByType<InputReader>();

        if (playerMovement == null)
            playerMovement = FindFirstObjectByType<PlayerMovement>();

        if (interactionSystem == null)
            interactionSystem = FindFirstObjectByType<PlayerInteractionSystem>();
    }

    private void OnEnable()
    {
        input.OnInventory += ToggleInventory;
        input.OnSubmitPressed += HandleSubmit;
    }

    private void OnDisable()
    {
        input.OnInventory -= ToggleInventory;
        input.OnSubmitPressed -= HandleSubmit;
    }

    public void ToggleInventory()
    {
        if (IsInventoryOpen)
            CloseInventory();
        else
            OpenInventory();
    }
    
    private void HandleSubmit()
    {
        OnSubmitPressed?.Invoke();
    }

    public void OpenInventory()
    {
        IsInventoryOpen = true;

        gameplayHUD.SetActive(false);

        playerMovement.SetMovementLocked(true);
        interactionSystem.enabled = false;

        inventoryMenu.Show();

        OnInventoryStateChanged?.Invoke(true);
    }

    public void CloseInventory()
    {
        IsInventoryOpen = false;

        gameplayHUD.SetActive(true);

        playerMovement.SetMovementLocked(false);
        interactionSystem.enabled = true;

        inventoryMenu.Hide();

        OnInventoryStateChanged?.Invoke(false);
    }
}