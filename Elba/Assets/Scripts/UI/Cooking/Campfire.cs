using System.Collections.Generic;
using Inventory;
using Inventory.Model;
using UnityEngine;

public class Campfire : MonoBehaviour, IInteractable, IUpgradeableStructure
{
    [Header("References")]
    [SerializeField] private Transform interactionAnchor;
    [SerializeField] private Animator fireAnimator;

    [Header("Upgrade")]
    [SerializeField] private StructureUpgradeSO nextUpgrade;

    [Header("Cooking")]
    [SerializeField] private bool startLit;

    private InventoryController currentInventory;

    private static readonly List<ActionData> emptyActions = new();

    public static bool PlayerNearLitCampfire { get; private set; }

    public Transform InteractionAnchor => interactionAnchor;
    public StructureUpgradeSO NextUpgrade => nextUpgrade;
    public bool IsLit => fireAnimator != null && fireAnimator.enabled;

    private void Awake()
    {
        if (fireAnimator == null)
            fireAnimator = GetComponentInChildren<Animator>();

        if (fireAnimator != null)
            fireAnimator.enabled = startLit;
    }

    public void OnPlaced()
    {
        startLit = true;

        if (fireAnimator != null)
            fireAnimator.enabled = true;
    }

    public List<ActionData> GetActions()
    {
        return emptyActions;
    }

    public void Interact(GameObject interactor)
    {
        currentInventory = interactor.GetComponent<InventoryController>();

        if (currentInventory == null)
            return;

        if (IsLit)
        {
            // Aquí irá tu sistema de cocina
            Debug.Log("Abrir cocina");
            return;
        }

        if (StructureUpgradeUI.Instance != null)
            StructureUpgradeUI.Instance.Open(this, currentInventory);
    }

    public bool CanUpgrade()
    {
        if (currentInventory == null || nextUpgrade == null)
            return false;

        if (nextUpgrade.Requirements == null || nextUpgrade.Requirements.Count == 0)
            return true;

        foreach (RecipeIngredient ingredient in nextUpgrade.Requirements)
        {
            if (currentInventory.GetItemCount(ingredient.item) < ingredient.amount)
                return false;
        }

        return true;
    }

    public bool Upgrade()
    {
        if (!CanUpgrade())
            return false;

        if (nextUpgrade == null || nextUpgrade.UpgradedPrefab == null)
            return false;

        currentInventory.ConsumeIngredients(nextUpgrade.Requirements);

        GameObject upgraded = Instantiate(
            nextUpgrade.UpgradedPrefab,
            transform.position,
            transform.rotation,
            transform.parent);

        upgraded.transform.localScale = transform.localScale;

        Destroy(gameObject);

        return true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsLit && other.CompareTag("Player"))
            PlayerNearLitCampfire = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            PlayerNearLitCampfire = false;
    }
}