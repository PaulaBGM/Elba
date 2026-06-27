using Inventory.UI;
using UnityEngine;

public class InventoryMenu : MonoBehaviour, IMenu
{
    [SerializeField] private UIInventoryPage inventoryPage;

    public bool IsOpen => gameObject.activeSelf;

    public void Show()
    {
        gameObject.SetActive(true);
        inventoryPage.ResetUI();
    }

    public void Hide()
    {
        inventoryPage.ResetUI();
        gameObject.SetActive(false);
    }
}