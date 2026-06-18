using Inventory.Model;
using UnityEngine;

public class StorageUI : MonoBehaviour
{
    [SerializeField] private InventorySO playerInventory;
    [SerializeField] private InventorySO storageInventory;

    public void Open(InventorySO storage)
    {
        storageInventory = storage;

        gameObject.SetActive(true);

        Refresh();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    private void Refresh()
    {
    }
}