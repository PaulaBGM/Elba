using Inventory.UI;
using UnityEngine;

public class InventoryMenu : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject tabs;
    [SerializeField] private GameObject inventoryPanel;

    [Header("Inventory UI")]
    [SerializeField] private UIInventoryPage inventoryPage;

    public void Show()
    {
        gameObject.SetActive(true);

        tabs.SetActive(true);
        inventoryPanel.SetActive(true);

        inventoryPage.ResetSelection();
    }

    public void Hide()
    {
        inventoryPage.HideItemActionPanel();
        inventoryPage.ResetSelection();

        inventoryPanel.SetActive(false);
        tabs.SetActive(false);

        gameObject.SetActive(false);
    }
}