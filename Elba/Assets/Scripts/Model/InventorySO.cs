using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class InventorySO : ScriptableObject
    {
        [SerializeField]
        private List<InventoryItem> inventoryItems;

        [field: SerializeField]
        public int Size { get; private set; } = 10;

        public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;

        public void Initialize()
        {
            inventoryItems = new List<InventoryItem>();

            for (int i = 0; i < Size; i++)
            {
                inventoryItems.Add(InventoryItem.GetEmptyItem());
            }
        }

        public int AddItem(ItemSO item, int quantity, List<ItemParameter> itemState = null)
        {
            if (!item.IsStackable)
            {
                while (quantity > 0 && !IsInventoryFull())
                {
                    quantity -= AddItemToFirstFreeSlot(item, 1, itemState);
                }

                InformAboutChange();
                return quantity;
            }

            quantity = AddStackableItem(item, quantity);
            InformAboutChange();
            return quantity;
        }

        private int AddItemToFirstFreeSlot(ItemSO item, int quantity, List<ItemParameter> itemState = null)
        {
            InventoryItem newItem = new InventoryItem
            {
                item = item,
                quantity = quantity,
                itemState = new List<ItemParameter>(
                    itemState == null ? item.DefaultParametersList : itemState)
            };

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                {
                    inventoryItems[i] = newItem;
                    return quantity;
                }
            }

            return 0;
        }

        private bool IsInventoryFull()
        {
            return !inventoryItems.Any(item => item.IsEmpty);
        }

        private int AddStackableItem(ItemSO item, int quantity)
        {
            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (inventoryItems[i].IsEmpty)
                    continue;

                if (inventoryItems[i].item.ItemID == item.ItemID)
                {
                    int spaceLeft = inventoryItems[i].item.MaxStackSize - inventoryItems[i].quantity;

                    if (quantity > spaceLeft)
                    {
                        inventoryItems[i].ChangeQuantity(inventoryItems[i].item.MaxStackSize);

                        quantity -= spaceLeft;
                    }
                    else
                    {
                        inventoryItems[i].ChangeQuantity(inventoryItems[i].quantity + quantity);

                        return 0;
                    }
                }
            }

            while (quantity > 0 && !IsInventoryFull())
            {
                int newAmount = Mathf.Clamp(quantity, 0, item.MaxStackSize);
                quantity -= newAmount;
                AddItemToFirstFreeSlot(item, newAmount);
            }

            return quantity;
        }

        public void RemoveItem(int itemIndex, int amount)
        {
            if (inventoryItems.Count <= itemIndex)
                return;

            if (inventoryItems[itemIndex].IsEmpty)
                return;

            int remaining = inventoryItems[itemIndex].quantity - amount;

            if (remaining <= 0)
                inventoryItems[itemIndex] = InventoryItem.GetEmptyItem();
            else
                inventoryItems[itemIndex].ChangeQuantity(remaining);

            InformAboutChange();
        }

        public void AddItem(InventoryItem item)
        {
            AddItem(item.item, item.quantity);
        }

        public Dictionary<int, InventoryItem> GetCurrentInventoryState()
        {
            Dictionary<int, InventoryItem> result = new();

            for (int i = 0; i < inventoryItems.Count; i++)
            {
                if (!inventoryItems[i].IsEmpty)
                    result[i] = inventoryItems[i];
            }

            return result;
        }

        public InventoryItem GetItemAt(int index)
        {
            return inventoryItems[index];
        }

        public void SwapItems(int indexA, int indexB)
        {
            InventoryItem temp = inventoryItems[indexA];
            inventoryItems[indexA] = inventoryItems[indexB];
            inventoryItems[indexB] = temp;

            InformAboutChange();
        }

        public List<InventoryItem> GetItemsByCategory(ItemCategory category)
        {
            List<InventoryItem> result = new();

            foreach (InventoryItem item in inventoryItems)
            {
                if (item.IsEmpty)
                    continue;

                if ((item.item.Categories & category) != 0)
                {
                    result.Add(item);
                }
            }

            return result;
        }

        private void InformAboutChange()
        {
            OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
        }
        public void AddInventoryItem(InventoryItem item)
        {
            AddItem(item.item, item.quantity, item.itemState);
        }
    }
    
}