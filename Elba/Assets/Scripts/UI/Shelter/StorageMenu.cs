using Inventory;
using Inventory.Model;
using UnityEngine;

public class StorageMenu : MonoBehaviour, IMenu
{
    [SerializeField] private InventoryView playerView;
    [SerializeField] private InventoryView storageView;
    [SerializeField] private StorageManager storageManager;

    private InventorySO playerInventory;
    private InventorySO storageInventory;

    public bool IsOpen => gameObject.activeSelf;

    public void Initialize(InventorySO player, InventorySO storage)
    {
        playerInventory = player;
        storageInventory = storage;

        storageManager.Initialize(player, storage);

        playerView.Initialize(player);
        storageView.Initialize(storage);
    }

    public void Show()
    {
        gameObject.SetActive(true);

        playerView.Refresh();
        storageView.Refresh();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}