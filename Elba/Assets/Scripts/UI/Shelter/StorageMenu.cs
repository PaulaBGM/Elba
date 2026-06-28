using System;
using System.Collections.Generic;
using Inventory.Model;
using Inventory.UI;
using UnityEngine;

public class StorageMenu : MonoBehaviour, IMenu
{
    [Header("Views")]
    [SerializeField] private UIStorageGrid playerView;
    [SerializeField] private UIStorageGrid storageView;

    [Header("Manager")]
    [SerializeField] private StorageManager storageManager;

    private readonly List<int> playerDisplayedSlots = new();
    private readonly List<int> storageDisplayedSlots = new();

    private InventorySO playerInventory;
    private InventorySO storageInventory;

    public bool IsOpen => gameObject.activeSelf;

    public event Action<int, RectTransform> OnPlayerItemSelected;
    public event Action<int, RectTransform> OnStorageItemSelected;

    public void Initialize(InventorySO player, InventorySO storage)
    {
        playerInventory = player;
        storageInventory = storage;

        storageManager.Initialize(player, storage);

        playerView.InitializeInventoryUI(player.Size);
        storageView.InitializeInventoryUI(storage.Size);

        playerView.OnItemClicked -= HandlePlayerItemClicked;
        storageView.OnItemClicked -= HandleStorageItemClicked;

        playerView.OnItemClicked += HandlePlayerItemClicked;
        storageView.OnItemClicked += HandleStorageItemClicked;

        Refresh();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Refresh();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Refresh()
    {
        RefreshGrid(playerView, playerInventory, playerDisplayedSlots);
        RefreshGrid(storageView, storageInventory, storageDisplayedSlots);
    }

    private void RefreshGrid(UIStorageGrid grid,InventorySO inventory,List<int> displayedSlots)
    {
        if (playerInventory == null)
            Debug.LogError("Player Inventory NULL");

        if (storageInventory == null)
            Debug.LogError("Storage Inventory NULL");

        grid.ResetAllItems();

        displayedSlots.Clear();

        Dictionary<int, InventoryItem> items =inventory.GetCurrentInventoryState();

        int visualIndex = 0;

        foreach (var pair in items)
        {
            if (pair.Value.IsEmpty)
                continue;

            displayedSlots.Add(pair.Key);

            grid.UpdateData(
                visualIndex,
                pair.Value.item.ItemImage,
                pair.Value.quantity);

            visualIndex++;
        }
    }

    private void HandlePlayerItemClicked(int visualIndex)
    {
        int realSlot = playerDisplayedSlots[visualIndex];

        OnPlayerItemSelected?.Invoke(
            realSlot,
            playerView.GetItemTransform(visualIndex));
    }

    private void HandleStorageItemClicked(int visualIndex)
    {
        int realSlot = storageDisplayedSlots[visualIndex];

        OnStorageItemSelected?.Invoke(
            realSlot,
            storageView.GetItemTransform(visualIndex));
    }

    private void OnDestroy()
    {
        playerView.OnItemClicked -= HandlePlayerItemClicked;
        storageView.OnItemClicked -= HandleStorageItemClicked;
    }
}