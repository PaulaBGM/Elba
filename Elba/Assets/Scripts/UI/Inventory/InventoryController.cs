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
        [SerializeField] private InventoryView inventoryView;
        [SerializeField] private InventorySO inventoryData;
        public List<InventoryItem> initialItems = new();
        public InventorySO InventoryData => inventoryData;
        public event Action<InventoryItem> OnItemUsed;
        public event Action<InventoryItem> OnItemDropped;
        public event Action OnInventoryOpened;
        public event Action OnInventoryClosed;

        private void OnEnable()
        {
            if (UIManager.Instance != null)
                UIManager.Instance.OnInventoryStateChanged += HandleInventoryState;
        }
        private void OnDisable()
        {
            if (UIManager.Instance != null)
                UIManager.Instance.OnInventoryStateChanged -= HandleInventoryState;
        }
        private void HandleInventoryState(bool opened)
        {
            if (opened)
            {
                inventoryView.Refresh();
                inventoryView.UI.ResetSelection();
                OnInventoryOpened?.Invoke();
            }
            else
            {
                inventoryView.UI.HideItemActionPanel();
                inventoryView.UI.ResetSelection();
                OnInventoryClosed?.Invoke();
            }
        }
        private void Start()
        {
            PrepareInventoryData();
            inventoryView.Initialize(inventoryData);
            inventoryView.OnItemSelected += HandleItemSelected;
            inventoryView.UI.OnSwapItems += HandleSwapItems;
            inventoryView.UI.OnStartDragging += HandleDragging;
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
            inventoryView.Refresh();
        }
        private void HandleItemActionRequest(int realSlot)
        {
            if (realSlot < 0)
            {
                inventoryView.UI.ClearActions();
                return;
            }

            InventoryItem inventoryItem = inventoryData.GetItemAt(realSlot);

            if (inventoryItem.IsEmpty)
            {
                inventoryView.UI.ClearActions();
                return;
            }

            inventoryView.UI.ClearActions();

            if (inventoryItem.item is EquippableItemSO)
            {
                inventoryView.UI.ShowActions(
                    "Equipar", () => PerformAction(realSlot),
                    "Sacar", () => HoldItem(realSlot));
                return;
            }

            if (inventoryItem.item is EdibleItemSO edible)
            {
                inventoryView.UI.ShowActions(
                    edible.ActionName, () => PerformAction(realSlot),
                    "Sacar", () => HoldItem(realSlot));
                return;
            }

            if (inventoryItem.item is ToolItemSO tool)
            {
                inventoryView.UI.ShowActions(
                    tool.ActionName, () => PerformAction(realSlot),
                    "Sacar", () => HoldItem(realSlot));
                return;
            }

            if (inventoryItem.item is MaterialItemSO)
            {
                inventoryView.UI.ShowActions(
                    "Tirar stack", () => DropStack(realSlot),
                    "Sacar", () => HoldItem(realSlot));
                return;
            }

            inventoryView.UI.ClearActions();
        }
        private void DropStack(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);

            if (inventoryItem.IsEmpty)
                return;

            for (int i = 0; i < inventoryItem.quantity; i++)
                SpawnDroppedItem(inventoryItem);

            inventoryData.RemoveItem(itemIndex, inventoryItem.quantity);
            inventoryView.UI.ClearActions();
            CloseInventoryUI();
        }
        private void CloseInventoryUI()
        {
            if (UIManager.Instance == null)
                return;

            UIManager.Instance.CloseInventory();
        }
        private void HoldItem(int itemIndex)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);

            if (inventoryItem.IsEmpty)
                return;

            PlayerInteractionSystem interaction = GetComponent<PlayerInteractionSystem>();

            if (interaction == null)
                return;

            interaction.HoldInventoryItem(inventoryItem.item);
            inventoryData.RemoveItem(itemIndex, 1);
            inventoryView.UI.ClearActions();
            CloseInventoryUI();
        }
        private void DropItem(int itemIndex, int quantity)
        {
            InventoryItem inventoryItem = inventoryData.GetItemAt(itemIndex);
            SpawnDroppedItem(inventoryItem);
            OnItemDropped?.Invoke(inventoryItem);
            inventoryData.RemoveItem(itemIndex, quantity);
            inventoryView.UI.HideItemActionPanel();
            inventoryView.UI.ResetSelection();
        }

        public void PerformAction(int itemIndex)
        {
            InventoryItem item = inventoryData.GetItemAt(itemIndex);

            if (item.IsEmpty)
                return;

            if (item.item is IItemAction action)
            {
                bool success = action.PerformAction(gameObject, item.itemState);

                if (success)
                {
                    OnItemUsed?.Invoke(item);

                    if (item.item is IDestroyableItem)
                    {
                        inventoryData.RemoveItem(itemIndex, 1);
                    }

                    inventoryView.UI.HideItemActionPanel();

                    if (inventoryData.GetItemAt(itemIndex).IsEmpty)
                        inventoryView.UI.ResetSelection();
                }
            }
        }

        private void HandleDragging(int visualIndex)
        {
            int realSlot = inventoryView.GetRealSlot(visualIndex);

            if (realSlot < 0)
                return;

            InventoryItem item = inventoryData.GetItemAt(realSlot);

            if (item.IsEmpty)
                return;

            inventoryView.UI.CreateDraggedItem( item.item.ItemImage,item.quantity);
        }

        private void HandleSwapItems(int visualA, int visualB)
        {
            int realA = inventoryView.GetRealSlot(visualA);
            int realB = inventoryView.GetRealSlot(visualB);

            if (realA < 0 || realB < 0)
                return;

            inventoryData.SwapItems(realA, realB);
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
       
        private void SpawnDroppedItem(InventoryItem inventoryItem)
        {
            if (inventoryItem.item == null)
                return;

            if (inventoryItem.item.WorldPrefab == null)
            {
                Debug.LogWarning( $"El item {inventoryItem.item.Name} no tiene WorldPrefab asignado");
                return;
            }

            Vector3 spawnPosition = transform.position + transform.right * 1.5f;
            GameObject droppedObject =  Instantiate(inventoryItem.item.WorldPrefab,spawnPosition,Quaternion.identity);
            Item worldItem =  droppedObject.GetComponent<Item>();

            if (worldItem != null)
            {
                worldItem.Quantity = 1;
            }
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

                    int amountToRemove =  Mathf.Min(remaining, slot.Value.quantity);
                    inventoryData.RemoveItem(slot.Key,amountToRemove);
                    remaining -= amountToRemove;
                }
            }
        }
        private void HandleItemSelected(int realSlot)
        {
            HandleItemActionRequest(realSlot);
        }
        public Dictionary<int, InventoryItem> GetInventory()
        {
            return inventoryData.GetCurrentInventoryState();
        }          
    }  
    }