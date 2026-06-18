using Inventory.Model;
using System.Collections.Generic;
using UnityEngine;

public class StorageManager : MonoBehaviour
{
    [SerializeField] private InventorySO playerInventory;
    [SerializeField] private InventorySO storageInventory;

    public void Initialize( InventorySO player, InventorySO storage)
    {
        playerInventory = player;
        storageInventory = storage;
    }

    public void StoreItem(InventoryItem item)
    {
        storageInventory.AddInventoryItem(item);
    }

    public void TakeItem(InventoryItem item)
    {
        playerInventory.AddInventoryItem(item);
    }

    public void StoreAll()
    {
        Dictionary<int, InventoryItem> items =
            playerInventory.GetCurrentInventoryState();

        foreach (var slot in items)
        {
            storageInventory.AddInventoryItem(slot.Value);
        }

        foreach (var slot in items)
        {
            playerInventory.RemoveItem(slot.Key, slot.Value.quantity);
        }
    }
    public void TakeAll()
    {
        Dictionary<int, InventoryItem> items =
            storageInventory.GetCurrentInventoryState();

        foreach (var slot in items)
        {
            playerInventory.AddInventoryItem(slot.Value);
        }

        foreach (var slot in items)
        {
            storageInventory.RemoveItem(slot.Key, slot.Value.quantity);
        }
    }
}