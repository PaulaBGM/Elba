using System;
using System.Collections.Generic;
using UnityEngine;
using Inventory.Model;
using Inventory.UI;

namespace Inventory
{
    public class InventoryView : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private UIInventoryPage inventoryUI;
        [SerializeField] private Sprite damageIcon;

        private InventorySO inventoryData;
        private readonly List<int> displayedSlots = new();
        private ItemCategory currentTab = ItemCategory.All;
        public UIInventoryPage UI => inventoryUI;

        public event Action<int> OnItemSelected;

        public void Initialize(InventorySO inventory)
        {
            inventoryData = inventory;

            inventoryUI.InitializeInventoryUI(inventory.Size);
            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryData.OnInventoryUpdated += OnInventoryUpdated;
            inventoryUI.OnTabChanged += HandleTabChanged;

            Refresh();
        }

        private void OnDestroy()
        {
            if (inventoryData != null)
                inventoryData.OnInventoryUpdated -= OnInventoryUpdated;

            inventoryUI.OnTabChanged -= HandleTabChanged;
        }

        private void OnInventoryUpdated(Dictionary<int, InventoryItem> inventory)
        {
            Refresh();
        }

        private void HandleTabChanged(ItemCategory category)
        {
            currentTab = category;
            Refresh();
        }
        private void HandleDescriptionRequest(int visualIndex)
        {
            int realSlot = GetRealSlot(visualIndex);
            if (realSlot < 0)
            {
                inventoryUI.ResetSelection();
                return;
            }
            InventoryItem inventoryItem = inventoryData.GetItemAt(realSlot);
            if (inventoryItem.IsEmpty)
            {
                inventoryUI.ResetSelection();
                return;
            }
            inventoryUI.UpdateDescription(realSlot,inventoryItem.item.ItemImage,inventoryItem.item.Name,inventoryItem.item.Description);
            inventoryUI.ClearStats();

            if (inventoryItem.item is EdibleItemSO edible)
            {
                foreach (var modifier in edible.ModifiersData)
                {
                    inventoryUI.AddStat(modifier.statModifier.Icon,modifier.value);
                }
            }
            else if (inventoryItem.item is ToolItemSO tool)
            {
                inventoryUI.AddStat(damageIcon, tool.AnimalDamage);
            }
            OnItemSelected?.Invoke(realSlot);
        }
        public void Refresh()
        {
            if (inventoryData == null)
                return;

            inventoryUI.ResetAllItems();
            displayedSlots.Clear();
            Dictionary<int, InventoryItem> inventory = inventoryData.GetCurrentInventoryState();
            int visualIndex = 0;

            foreach (var pair in inventory)
            {
                if (pair.Value.IsEmpty)
                    continue;

                if (currentTab != ItemCategory.All &&
                    (pair.Value.item.Categories & currentTab) == 0)
                    continue;

                displayedSlots.Add(pair.Key);
                inventoryUI.UpdateData(visualIndex,pair.Value.item.ItemImage,pair.Value.quantity);
                visualIndex++;
            }
        }

        public int GetRealSlot(int visualIndex)
        {
            if (visualIndex < 0 || visualIndex >= displayedSlots.Count)
                return -1;

            return displayedSlots[visualIndex];
        }
    }
}