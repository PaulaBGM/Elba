using Inventory.UI;
using UnityEngine;

public class InventoryMenu : MenuBase
{
    [SerializeField] private UIInventoryPage inventoryPage;

    public override void Show()
    {
        base.Show();
        inventoryPage.ResetUI();
    }

    public override void Hide()
    {
        inventoryPage.ResetUI();
        base.Hide();
    }
}