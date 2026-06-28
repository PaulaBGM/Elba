using Inventory;
using Inventory.Model;
using UnityEngine;

public class ShelterStorage : MonoBehaviour
{
    [SerializeField] private InventorySO storageInventory;
    [SerializeField] private StorageController storageController;

    public InventorySO StorageInventory => storageInventory;

    private void Awake()
    {
        if (storageInventory != null)
            storageInventory.Initialize();
    }

    public void Interact(GameObject interactor)
    {
        InventoryController inventory =
            interactor.GetComponent<InventoryController>();

        if (inventory == null)
            return;

        storageController.Open(inventory.InventoryData, storageInventory);
    }
}