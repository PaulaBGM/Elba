using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private UIInventoryPage inventoryUI;
        [SerializeField] private InventorySO inventoryData;
        [SerializeField] private InputReader input;
        [SerializeField] private AudioClip dropClip;
        [SerializeField] private AudioSource audioSource;

        private ItemType currentTab = ItemType.Misc;

        public List<InventoryItem> initialItems = new();

        public InventorySO InventoryData => inventoryData;

        public event Action<InventoryItem> OnItemUsed;
        public event Action<InventoryItem> OnItemDropped;
        public event Action OnInventoryOpened;
        public event Action OnInventoryClosed;

        private void OnEnable()
        {
            if (input != null)
                input.OnInventory += ToggleInventory;
        }

        private void OnDisable()
        {
            if (input != null)
                input.OnInventory -= ToggleInventory;
        }
        private void Start()
        {
            if (input == null)
                input = FindFirstObjectByType<InputReader>();

            PrepareUI();
            PrepareInventoryData();
        }

        private void PrepareInventoryData()
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;

            foreach (InventoryItem item in initialItems)
            {
                if (!item.IsEmpty)
                    inventoryData.AddItem(item);
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            inventoryUI.ResetAllItems();

            foreach (var item in inventoryState)
            {
                inventoryUI.UpdateData(
                    item.Key,
                    item.Value.item.ItemImage,
                    item.Value.quantity);
            }
        }

        private void PrepareUI()
        {
            inventoryUI.InitializeInventoryUI(inventoryData.Size);

            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryUI.OnSwapItems += HandleSwapItems;
            inventoryUI.OnStartDragging += HandleDragging;
            inventoryUI.OnItemActionRequested += HandleItemActionRequest;
            inventoryUI.OnTabChanged += HandleTabChanged;
        }

        private void HandleTabChanged(ItemType type)
        {
            currentTab = type;
            UpdateInventoryUIFiltered();
        }

        private void UpdateInventoryUIFiltered()
        {
            inventoryUI.ResetAllItems();

            List<InventoryItem> items = inventoryData.GetItemsByType(currentTab);

            for (int i = 0; i < items.Count; i++)
            {
                inventoryUI.UpdateData(
                    i,
                    items[i].item.ItemImage,
                    items[i].quantity);
            }
        }

        private void HandleItemActionRequest(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);

            if (inventoryItem.IsEmpty)
                return;

            inventoryUI.ShowItemAction(itemIndex);

            if (inventoryItem.item is IItemAction action)
            {
                inventoryUI.AddAction(
                    action.ActionName,
                    () => PerformAction(itemIndex));
            }

            if (inventoryItem.item is IDestroyableItem)
            {
                inventoryUI.AddAction(
                    "Drop",
                    () => DropItem(itemIndex, inventoryItem.quantity));
            }
        }

        private void DropItem(int itemIndex, int quantity)
        {
            InventoryItem item = inventoryData.GetItemAt(itemIndex);

            OnItemDropped?.Invoke(item);

            inventoryData.RemoveItem(itemIndex, quantity);

            inventoryUI.ResetSelection();

            if (audioSource != null && dropClip != null)
                audioSource.PlayOneShot(dropClip);
        }

        public void PerformAction(int itemIndex)
        {
            InventoryItem item = inventoryData.GetItemAt(itemIndex);

            if (item.IsEmpty)
                return;

            if (item.item is IItemAction action)
            {
                bool success = action.PerformAction(
                    gameObject,
                    item.itemState);

                if (success)
                {
                    OnItemUsed?.Invoke(item);

                    if (item.item is IDestroyableItem)
                    {
                        inventoryData.RemoveItem(itemIndex, 1);
                    }

                    if (audioSource != null && action.actionSFX != null)
                        audioSource.PlayOneShot(action.actionSFX);

                    if (inventoryData.GetItemAt(itemIndex).IsEmpty)
                        inventoryUI.ResetSelection();
                }
            }
        }

        private void HandleDragging(int itemIndex)
        {
            InventoryItem item = inventoryData.GetItemAt(itemIndex);

            if (item.IsEmpty)
                return;

            inventoryUI.CreateDraggedItem(
                item.item.ItemImage,
                item.quantity);
        }

        private void HandleSwapItems(int a, int b)
        {
            inventoryData.SwapItems(a, b);
        }

        private void HandleDescriptionRequest(int index)
        {
            InventoryItem item = inventoryData.GetItemAt(index);

            if (item.IsEmpty)
            {
                inventoryUI.ResetSelection();
                return;
            }

            inventoryUI.UpdateDescription(
                index,
                item.item.ItemImage,
                item.item.Name,
                PrepareDescription(item));
        }

        private string PrepareDescription(InventoryItem item)
        {
            StringBuilder sb = new();

            sb.AppendLine(item.item.Description);

            for (int i = 0; i < item.itemState.Count; i++)
            {
                sb.AppendLine(
                    $"{item.itemState[i].itemParameter.ParameterName}: " +
                    $"{item.itemState[i].value} / {item.item.DefaultParametersList[i].value}");
            }

            return sb.ToString();
        }

        private void ToggleInventory()
        {
            if (!inventoryUI.gameObject.activeSelf)
            {
                inventoryUI.Show();

                UpdateInventoryUI(inventoryData.GetCurrentInventoryState());

                OnInventoryOpened?.Invoke();
            }
            else
            {
                inventoryUI.Hide();

                OnInventoryClosed?.Invoke();
            }
        }

        public int AddItem(ItemSO item, int quantity)
        {
            return inventoryData.AddItem(item, quantity);
        }

        public bool ContainsItem(ItemSO item)
        {
            foreach (InventoryItem inventoryItem in inventoryData.GetCurrentInventoryState().Values)
            {
                if (inventoryItem.item == item)
                    return true;
            }

            return false;
        }

        public int GetItemCount(ItemSO item)
        {
            int count = 0;

            foreach (InventoryItem inventoryItem in inventoryData.GetCurrentInventoryState().Values)
            {
                if (inventoryItem.item == item)
                    count += inventoryItem.quantity;
            }

            return count;
        }

        public bool HasItems(List<RecipeIngredient> ingredients)
        {
            foreach (RecipeIngredient ingredient in ingredients)
            {
                if (GetItemCount(ingredient.item) < ingredient.amount)
                    return false;
            }

            return true;
        }

        public void ConsumeIngredients(List<RecipeIngredient> ingredients)
        {
            foreach (RecipeIngredient ingredient in ingredients)
            {
                int remaining = ingredient.amount;

                Dictionary<int, InventoryItem> inventoryState =
                    inventoryData.GetCurrentInventoryState();

                foreach (var slot in inventoryState)
                {
                    if (remaining <= 0)
                        break;

                    if (slot.Value.item != ingredient.item)
                        continue;

                    int amountToRemove =
                        Mathf.Min(
                            remaining,
                            slot.Value.quantity);

                    inventoryData.RemoveItem(
                        slot.Key,
                        amountToRemove);

                    remaining -= amountToRemove;
                }
            }
        }
    }
}