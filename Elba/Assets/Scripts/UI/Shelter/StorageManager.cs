using System.Collections.Generic;
using Inventory.Model;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    private InventorySO playerInventory;
    private InventorySO storageInventory;

    public void Initialize(InventorySO player, InventorySO storage)
    {
        playerInventory = player;
        storageInventory = storage;
    }

    public bool StoreItem(int playerSlot)
    {
        InventoryItem item = playerInventory.GetItemAt(playerSlot);

        if (item.IsEmpty)
            return false;

        int remainder = storageInventory.AddItem(item.item, item.quantity, item.itemState);
        int storedAmount = item.quantity - remainder;

        if (storedAmount <= 0)
            return false;

        playerInventory.RemoveItem(playerSlot, storedAmount);
        return true;
    }

    public bool TakeItem(int storageSlot)
    {
        InventoryItem item = storageInventory.GetItemAt(storageSlot);

        if (item.IsEmpty)
            return false;

        int remainder = playerInventory.AddItem(item.item, item.quantity, item.itemState);
        int movedAmount = item.quantity - remainder;

        if (movedAmount <= 0)
            return false;

        storageInventory.RemoveItem(storageSlot, movedAmount);
        return true;
    }

    public void StoreAll()
    {
        Dictionary<int, InventoryItem> items = new(playerInventory.GetCurrentInventoryState());

        foreach (var slot in items)
        {
            if (slot.Value.IsEmpty)
                continue;

            StoreItem(slot.Key);
        }
    }

    public void TakeAll()
    {
        Dictionary<int, InventoryItem> items = new(storageInventory.GetCurrentInventoryState());

        foreach (var slot in items)
        {
            if (slot.Value.IsEmpty)
                continue;

            TakeItem(slot.Key);
        }
    }
}