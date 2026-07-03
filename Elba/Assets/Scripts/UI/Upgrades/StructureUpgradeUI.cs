using Inventory;
using Inventory.Model;
using UnityEngine;
using UnityEngine.UI;

public class StructureUpgradeUI : MonoBehaviour
{
    public static StructureUpgradeUI Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject root;
    [SerializeField] private InputReader input;
    [SerializeField] private StructureDescriptionUI descriptionUI;

    [Header("Buttons")]
    [SerializeField] private Button craftButton;
    [SerializeField] private Button cancelButton;

    private IUpgradeableStructure currentStructure;
    private InventoryController currentInventory;

    private void Awake()
    {
        Instance = this;

        if (root != null)
            root.SetActive(false);

        if (input == null)
            input = FindFirstObjectByType<InputReader>();

        if (craftButton != null)
            craftButton.onClick.AddListener(Upgrade);

        if (cancelButton != null)
            cancelButton.onClick.AddListener(Close);
    }

    private void OnEnable()
    {
        if (input != null)
            input.OnInventory += HandleInventoryPressed;
    }

    private void OnDisable()
    {
        if (input != null)
            input.OnInventory -= HandleInventoryPressed;
    }

    private void HandleInventoryPressed()
    {
        if (root != null && root.activeSelf)
            Close();
    }

    public void Open(IUpgradeableStructure structure, InventoryController inventory)
    {
        currentStructure = structure;
        currentInventory = inventory;

        root.SetActive(true);

        UIManager.Instance.IsExternalMenuOpen = true;
        Time.timeScale = 0f;

        Refresh();
    }

    public void Close()
    {
        currentStructure = null;
        currentInventory = null;

        if (descriptionUI != null)
            descriptionUI.ResetDescription();

        if (UIManager.Instance != null)
            UIManager.Instance.IsExternalMenuOpen = false;

        if (root != null)
            root.SetActive(false);

        Time.timeScale = 1f;
    }

    private void Refresh()
    {
        if (currentStructure == null || currentInventory == null)
        {
            Close();
            return;
        }

        StructureUpgradeSO upgrade = currentStructure.NextUpgrade;

        if (upgrade == null)
        {
            Close();
            return;
        }

        descriptionUI.ResetDescription();
        descriptionUI.SetDescription(upgrade.Title, upgrade.Description);

        foreach (RecipeIngredient ingredient in upgrade.Requirements)
        {
            int owned = currentInventory.GetItemCount(ingredient.item);

            descriptionUI.AddIngredient(
                ingredient.item.ItemImage,
                ingredient.item.Name,
                owned,
                ingredient.amount);
        }

        if (craftButton != null)
            craftButton.interactable = currentStructure.CanUpgrade();
    }

    private void Upgrade()
    {
        if (currentStructure == null)
            return;

        if (!currentStructure.Upgrade())
            return;

        bool finishedGame = currentStructure.NextUpgrade == null;

        Close();

        if (finishedGame)
        {
            GameEndUI.Instance.Show();
        }
    }
}