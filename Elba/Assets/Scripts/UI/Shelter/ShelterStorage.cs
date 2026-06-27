using Inventory.Model;
using UnityEngine;

public class ShelterStorage : MonoBehaviour
{
    [SerializeField] private InventorySO storageInventory;

    public InventorySO StorageInventory => storageInventory;
}