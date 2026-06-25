using System.Collections.Generic;
using UnityEngine;
using Inventory;
using Inventory.Model;

public class Item : MonoBehaviour, IInteractable
{
    [Header("Item")]
    [SerializeField] private ItemSO inventoryItem;

    public ItemSO InventoryItem => inventoryItem;

    [field: SerializeField]
    public int Quantity { get; set; } = 1;

    [Header("Interaction")]
    [SerializeField] private Transform interactionAnchor;

    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D itemCollider;

    public Transform InteractionAnchor => interactionAnchor;

    [SerializeField]
    private bool cupFilled;
    public bool IsCupFilled => cupFilled;

    private static readonly List<ActionData> edibleActions =
        new()
        {
            new("Guardar", "R"),
            new("Consumir", "F"),
            new("Tirar", "Q")
        };

    private static readonly List<ActionData> normalActions =
        new()
        {
            new("Guardar", "R"),
            new("Tirar", "Q")
        };

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (itemCollider == null)
            itemCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        RefreshVisual();
    }
    public bool IsCup()
    {
        return inventoryItem is CupItemSO;
    }
    public void FillCup()
    {
        Debug.Log("[Cup] Intentando llenar vaso.");

        if (!IsCup())
        {
            Debug.Log("[Cup] El objeto no es un vaso.");
            return;
        }

        if (cupFilled)
        {
            Debug.Log("[Cup] El vaso ya estaba lleno.");
            return;
        }

        cupFilled = true;

        Debug.Log("[Cup] Vaso llenado correctamente.");

        RefreshVisual();
    }

    private void RefreshVisual()
    {
        if (spriteRenderer == null || inventoryItem == null)
            return;

        if (inventoryItem is CupItemSO cup)
        {
            Debug.Log($"[Cup] RefreshVisual | Filled = {cupFilled}");

            spriteRenderer.sprite =
                cupFilled
                ? cup.FullSprite
                : inventoryItem.ItemImage;

            return;
        }

        spriteRenderer.sprite =
            inventoryItem.ItemImage;
    }

    public void Interact(GameObject interactor)
    {
        // El PlayerInteractionSystem se encarga de recogerlo.
    }

    public void SetItem(ItemSO newItem)
    {
        if (newItem == null)
            return;

        inventoryItem = newItem;

        RefreshVisual();
    }

    public void Store(GameObject interactor)
    {
        InventoryController inventory =
            interactor.GetComponent<InventoryController>();

        if (inventory == null)
            return;

        inventory.InventoryData.AddItem(
            inventoryItem,
            Quantity);

        Destroy(gameObject);
    }

    public void Consume(GameObject interactor)
    {
        if (inventoryItem is CupItemSO cup)
        {
            Debug.Log($"[Cup] Consumir | Filled = {cupFilled}");

            if (!cupFilled)
            {
                Debug.Log("[Cup] El vaso está vacío.");
                return;
            }

            PlayerStatsSystem stats =
                interactor.GetComponent<PlayerStatsSystem>();

            if (stats != null)
            {
                Debug.Log($"[Cup] Recuperando {cup.ThirstRecovered} de sed.");

                stats.ModifyStat(
                    StatType.Thirst,
                    cup.ThirstRecovered);
            }

            cupFilled = false;

            Debug.Log("[Cup] El vaso vuelve a estar vacío.");

            RefreshVisual();

            return;
        }

        if (inventoryItem is not EdibleItemSO edible)
            return;

        edible.PerformAction(interactor);

        Destroy(gameObject);
    }

    public void HideWorldRepresentation()
    {
        if (spriteRenderer != null)
            spriteRenderer.enabled = false;

        if (itemCollider != null)
            itemCollider.enabled = false;
    }

    public void ShowWorldRepresentation(Vector3 position)
    {
        transform.position = position;

        if (spriteRenderer != null)
            spriteRenderer.enabled = true;

        if (itemCollider != null)
            itemCollider.enabled = true;
    }

    public bool CanCook()
    {
        return inventoryItem != null &&
               inventoryItem.canCook &&
               inventoryItem.cookedResult != null;
    }

    public void Cook()
    {
        if (!CanCook())
            return;

        SetItem(inventoryItem.cookedResult);
    }

    public List<ActionData> GetActions()
    {
        return inventoryItem is EdibleItemSO
            ? edibleActions
            : normalActions;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (interactionAnchor == null)
            return;

        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(
            interactionAnchor.position,
            0.1f);
    }
#endif
}