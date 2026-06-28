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
        [SerializeField] private InventorySO inventoryData;

        public InventorySO InventoryData => inventoryData;

        public event Action<int> OnDescriptionRequested;
        public event Action<int> OnStartDragging;
        public event Action<int, int> OnSwapItems;
        public event Action<ItemCategory> OnTabChanged;

        private void Awake()
        {
            PrepareUI();
        }

        public void Initialize(InventorySO inventory)
        {
            inventoryData = inventory;

            inventoryData.OnInventoryUpdated += UpdateInventoryUI;

            UpdateInventoryUI(inventoryData.GetCurrentInventoryState());
        }

        private void PrepareUI()
        {
            inventoryUI.InitializeInventoryUI(inventoryData.Size);

            inventoryUI.OnDescriptionRequested += index =>
                OnDescriptionRequested?.Invoke(index);

            inventoryUI.OnStartDragging += index =>
                OnStartDragging?.Invoke(index);

            inventoryUI.OnSwapItems += (a, b) =>
                OnSwapItems?.Invoke(a, b);

            inventoryUI.OnTabChanged += category =>
                OnTabChanged?.Invoke(category);
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItems();

            foreach (var pair in inventoryState)
            {
                inventoryUI.UpdateData(
                    pair.Key,
                    pair.Value.item.ItemImage,
                    pair.Value.quantity);
            }
        }

        public void Refresh()
        {
            if (inventoryData == null)
                return;

            UpdateInventoryUI(
                inventoryData.GetCurrentInventoryState());
        }
    }
}