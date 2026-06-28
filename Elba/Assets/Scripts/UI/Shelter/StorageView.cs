using System;
using System.Collections.Generic;
using UnityEngine;
using Inventory.Model;
using Inventory.UI;

namespace Inventory
{
    public class StorageView : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private UIStorageGrid playerGrid;
        [SerializeField] private List<InventoryTab> playerTabs;

        [Header("Storage")]
        [SerializeField] private UIStorageGrid storageGrid;
        [SerializeField] private List<InventoryTab> storageTabs;

        private InventorySO playerInventory;
        private InventorySO storageInventory;

        private readonly List<int> playerDisplayedSlots = new();
        private readonly List<int> storageDisplayedSlots = new();

        private ItemCategory playerCurrentTab = ItemCategory.All;
        private ItemCategory storageCurrentTab = ItemCategory.All;

        public event Action<int, RectTransform> OnPlayerItemSelected;
        public event Action<int, RectTransform> OnStorageItemSelected;

        public void Initialize(InventorySO player, InventorySO storage)
        {
            playerInventory = player;
            storageInventory = storage;

            playerGrid.InitializeInventoryUI(playerInventory.Size);
            storageGrid.InitializeInventoryUI(storageInventory.Size);

            playerGrid.OnItemClicked += HandlePlayerItemClicked;
            storageGrid.OnItemClicked += HandleStorageItemClicked;

            foreach (var tab in playerTabs)
                tab.OnTabSelected += HandlePlayerTabChanged;

            foreach (var tab in storageTabs)
                tab.OnTabSelected += HandleStorageTabChanged;

            playerInventory.OnInventoryUpdated += HandlePlayerInventoryUpdated;
            storageInventory.OnInventoryUpdated += HandleStorageInventoryUpdated;

            RefreshPlayer();
            RefreshStorage();
        }

        private void OnDestroy()
        {
            if (playerInventory != null)
                playerInventory.OnInventoryUpdated -= HandlePlayerInventoryUpdated;

            if (storageInventory != null)
                storageInventory.OnInventoryUpdated -= HandleStorageInventoryUpdated;

            if (playerGrid != null)
                playerGrid.OnItemClicked -= HandlePlayerItemClicked;

            if (storageGrid != null)
                storageGrid.OnItemClicked -= HandleStorageItemClicked;

            foreach (var tab in playerTabs)
                tab.OnTabSelected -= HandlePlayerTabChanged;

            foreach (var tab in storageTabs)
                tab.OnTabSelected -= HandleStorageTabChanged;
        }

        private void HandlePlayerInventoryUpdated(Dictionary<int, InventoryItem> _)
        {
            RefreshPlayer();
        }

        private void HandleStorageInventoryUpdated(Dictionary<int, InventoryItem> _)
        {
            RefreshStorage();
        }

        private void HandlePlayerTabChanged(ItemCategory category)
        {
            playerCurrentTab = category;
            RefreshPlayer();
        }

        private void HandleStorageTabChanged(ItemCategory category)
        {
            storageCurrentTab = category;
            RefreshStorage();
        }

        private void HandlePlayerItemClicked(int visualIndex)
        {
            int realSlot = GetRealSlot(playerDisplayedSlots, visualIndex);

            if (realSlot < 0)
                return;

            OnPlayerItemSelected?.Invoke(realSlot, playerGrid.GetItemTransform(visualIndex));
        }

        private void HandleStorageItemClicked(int visualIndex)
        {
            int realSlot = GetRealSlot(storageDisplayedSlots, visualIndex);

            if (realSlot < 0)
                return;

            OnStorageItemSelected?.Invoke(realSlot, storageGrid.GetItemTransform(visualIndex));
        }

        public void RefreshPlayer()
        {
            if (playerInventory == null)
                return;

            playerGrid.ResetAllItems();
            playerDisplayedSlots.Clear();

            Dictionary<int, InventoryItem> inventory =
                playerInventory.GetCurrentInventoryState();

            int visualIndex = 0;

            foreach (var pair in inventory)
            {
                if (pair.Value.IsEmpty)
                    continue;

                if (playerCurrentTab != ItemCategory.All &&
                    (pair.Value.item.Categories & playerCurrentTab) == 0)
                    continue;

                playerDisplayedSlots.Add(pair.Key);

                playerGrid.UpdateData(
                    visualIndex,
                    pair.Value.item.ItemImage,
                    pair.Value.quantity);

                visualIndex++;
            }
        }

        public void RefreshStorage()
        {
            if (storageInventory == null)
                return;

            storageGrid.ResetAllItems();
            storageDisplayedSlots.Clear();

            Dictionary<int, InventoryItem> inventory =
                storageInventory.GetCurrentInventoryState();

            int visualIndex = 0;

            foreach (var pair in inventory)
            {
                if (pair.Value.IsEmpty)
                    continue;

                if (storageCurrentTab != ItemCategory.All &&
                    (pair.Value.item.Categories & storageCurrentTab) == 0)
                    continue;

                storageDisplayedSlots.Add(pair.Key);

                storageGrid.UpdateData(
                    visualIndex,
                    pair.Value.item.ItemImage,
                    pair.Value.quantity);

                visualIndex++;
            }
        }

        private int GetRealSlot(List<int> displayedSlots, int visualIndex)
        {
            if (visualIndex < 0 || visualIndex >= displayedSlots.Count)
                return -1;

            return displayedSlots[visualIndex];
        }
    }
}