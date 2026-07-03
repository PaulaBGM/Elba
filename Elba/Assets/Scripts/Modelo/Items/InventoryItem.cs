using System;
using System.Collections.Generic;

namespace Inventory.Model
{
    [Serializable]
    public class InventoryItem
    {
        public ItemSO item;
        public int quantity;
        public List<ItemParameter> itemState = new();

        public bool IsEmpty => item == null;

        public bool IsStackable =>
            item != null && item.IsStackable;

        public int MaxStackSize =>
            item != null ? item.MaxStackSize : 0;

        public void ChangeQuantity(int newQuantity)
        {
            quantity = newQuantity;
        }

        public void AddQuantity(int amount)
        {
            quantity += amount;
        }

        public void RemoveQuantity(int amount)
        {
            quantity -= amount;

            if (quantity < 0)
                quantity = 0;
        }

        public static InventoryItem GetEmptyItem()
        {
            return new InventoryItem
            {
                item = null,
                quantity = 0,
                itemState = new List<ItemParameter>()
            };
        }
    }
}