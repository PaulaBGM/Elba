using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;

public class Item : MonoBehaviour, IInteractable
{
    [field: SerializeField]
    public ItemSO InventoryItem { get; private set; }

    [field: SerializeField]
    public int Quantity { get; set; } = 1;

    [Header("Interaction")]
    [SerializeField] private Transform interactionAnchor;

    [Header("References")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D itemCollider;

    [Header("Feedback")]
    [SerializeField] private float duration = 0.3f;

    public Transform InteractionAnchor => interactionAnchor;

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
        if (spriteRenderer != null)
            spriteRenderer.sprite = InventoryItem.ItemImage;
    }

    public void Interact(GameObject interactor)
    {
        // Ya no se usa para guardar automáticamente.
        // El PlayerInteractionSystem se encarga de cogerlo en las manos.
    }

    public void Store(GameObject interactor)
    {
        InventoryController inventory =
            interactor.GetComponent<InventoryController>();

        if (inventory == null)
            return;

        inventory.InventoryData.AddItem(
            InventoryItem,
            Quantity);

        Destroy(gameObject);
    }

    public void Consume(GameObject interactor)
    {
        if (InventoryItem is not EdibleItemSO edible)
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

    public void ShowWorldRepresentation(Vector3 worldPosition)
    {
        transform.position = worldPosition;

        if (spriteRenderer != null)
            spriteRenderer.enabled = true;

        if (itemCollider != null)
            itemCollider.enabled = true;
    }

    public List<ActionData> GetActions()
    {
        return InventoryItem is EdibleItemSO
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