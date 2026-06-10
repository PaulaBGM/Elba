using System;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Groups")]
    [SerializeField] private GameObject gameplayHUD;
    [SerializeField] private GameObject inventoryUI;

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

        inventoryUI.SetActive(true);
        gameplayHUD.SetActive(false);

        OnInventoryStateChanged?.Invoke(true);
    }

    public void CloseInventory()
    {
        IsInventoryOpen = false;

        inventoryUI.SetActive(false);
        gameplayHUD.SetActive(true);

        OnInventoryStateChanged?.Invoke(false);
    }
}