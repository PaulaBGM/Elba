using UnityEngine;
using Inventory.Model;
using Inventory;

public class StorageController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private StorageMenu storageMenu;
    [SerializeField] private StorageManager storageManager;
    [SerializeField] private UIFloatingStorageMenu floatingMenu;

    public void Open()
    {
        InventoryController player =
            FindFirstObjectByType<InventoryController>();

        ShelterStorage shelter =
            FindFirstObjectByType<ShelterStorage>();

        if (player == null || shelter == null)
        {
            Debug.LogError("Falta el player o el ShelterStorage.");
            return;
        }

        Open(player.InventoryData, shelter.StorageInventory);
    }

    public void Open(InventorySO playerInventory, InventorySO storageInventory)
    {
        storageManager.Initialize(playerInventory, storageInventory);
        storageMenu.Initialize(playerInventory, storageInventory);
        storageMenu.Show();
    }

    public void Close()
    {
        floatingMenu.Hide();
        storageMenu.Hide();
    }

    private void Awake()
    {
        storageMenu.OnPlayerItemSelected += HandlePlayerItem;
        storageMenu.OnStorageItemSelected += HandleStorageItem;
    }

    private void OnDestroy()
    {
        storageMenu.OnPlayerItemSelected -= HandlePlayerItem;
        storageMenu.OnStorageItemSelected -= HandleStorageItem;
    }

    private void HandlePlayerItem(int slot, RectTransform rect)
    {
        floatingMenu.Show(rect.position, "Guardar", () =>
        {
            if (storageManager.StoreItem(slot))
                storageMenu.Refresh();

            floatingMenu.Hide();
        });
    }

    private void HandleStorageItem(int slot, RectTransform rect)
    {
        floatingMenu.Show(rect.position, "Sacar", () =>
        {
            if (storageManager.TakeItem(slot))
                storageMenu.Refresh();

            floatingMenu.Hide();
        });
    }

    public void StoreAll()
    {
        storageManager.StoreAll();
        storageMenu.Refresh();
        floatingMenu.Hide();
    }

    public void TakeAll()
    {
        storageManager.TakeAll();
        storageMenu.Refresh();
        floatingMenu.Hide();
    }
}