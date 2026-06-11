using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject gameplayHUD;

    [Header("Input")]
    [SerializeField] private InputReader input;

    [Header("Systems")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerInteractionSystem interactionSystem;

    public bool IsInventoryOpen { get; private set; }

    public event Action<bool> OnInventoryStateChanged;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (input == null)
            input = FindFirstObjectByType<InputReader>();

        if (playerMovement == null)
            playerMovement = FindFirstObjectByType<PlayerMovement>();

        if (interactionSystem == null)
            interactionSystem = FindFirstObjectByType<PlayerInteractionSystem>();
    }

    private void OnEnable()
    {
        if (input != null)
            input.OnInventory += HandleInventory;
    }

    private void OnDisable()
    {
        if (input != null)
            input.OnInventory -= HandleInventory;
    }

    private void HandleInventory()
    {
        ToggleInventory();
    }

    public void ToggleInventory()
    {
        if (IsInventoryOpen)
            CloseInventory();
        else
            OpenInventory();
    }

    public void OpenInventory()
    {
        IsInventoryOpen = true;

        if (gameplayHUD != null)
            gameplayHUD.SetActive(false);

        if (playerMovement != null)
            playerMovement.SetMovementLocked(true);

        if (interactionSystem != null)
            interactionSystem.enabled = false;

        OnInventoryStateChanged?.Invoke(true);
    }

    public void CloseInventory()
    {
        IsInventoryOpen = false;

        if (gameplayHUD != null)
            gameplayHUD.SetActive(true);

        if (playerMovement != null)
            playerMovement.SetMovementLocked(false);

        if (interactionSystem != null)
            interactionSystem.enabled = true;

        OnInventoryStateChanged?.Invoke(false);
    }
}