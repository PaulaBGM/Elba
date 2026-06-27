using UnityEngine;
using Inventory.Model;

public class PlayerInteractionSystem : MonoBehaviour
{
    [SerializeField] private InputReader input;
    [SerializeField] private InteractionPromptUI promptUI;

    [Header("Held Item")]
    [SerializeField] private SpriteRenderer heldItemRenderer;

    private IInteractable currentInteractable;
    private Item heldItem;

    public Item HeldItem => heldItem;
    public bool HasHeldItem => heldItem != null;

    private void Awake()
    {
        if (heldItemRenderer != null)
            heldItemRenderer.enabled = false;
    }

    private void OnEnable()
    {
        if (input == null)
            return;

        input.OnInteract += Interact;
        input.OnStorePickup += StoreItem;
        input.OnConsumePickup += ConsumeItem;
        input.OnDropPickup += DropItem;
    }

    private void OnDisable()
    {
        if (input == null)
            return;

        input.OnInteract -= Interact;
        input.OnStorePickup -= StoreItem;
        input.OnConsumePickup -= ConsumeItem;
        input.OnDropPickup -= DropItem;
    }

    private void Interact()
    {
        if (heldItem != null)
        {
            currentInteractable?.Interact(gameObject);
            return;
        }
        if (currentInteractable is Item item)
        {
            PickUpItem(item);
            return;
        }
        currentInteractable?.Interact(gameObject);
    }

    private void PickUpItem(Item item)
    {
        heldItem = item;
        heldItemRenderer.sprite = item.InventoryItem.ItemImage;
        heldItemRenderer.enabled = true;
        item.HideWorldRepresentation();
        promptUI.Hide();
        UIActionBar.Instance.ShowActions(item.GetActions());
    }

    private void StoreItem()
    {
        if (heldItem == null)
            return;
        heldItem.Store(gameObject);
        ClearHeldItem();
    }

    private void ConsumeItem()
    {
        if (heldItem == null)
            return;

        heldItem.Consume(gameObject);
        ClearHeldItem();
    }

    private void DropItem()
    {
        if (heldItem == null)
            return;

        PlayerMovement movement = GetComponent<PlayerMovement>();
        Vector3 dropPosition = transform.position;
        if (movement != null)
            dropPosition += (Vector3)movement.LastDirection;

        heldItem.ShowWorldRepresentation(dropPosition);
        ClearHeldItem();
    }

    private void ClearHeldItem()
    {
        heldItem = null;
        heldItemRenderer.sprite = null;
        heldItemRenderer.enabled = false;
        UIActionBar.Instance.Hide();
    }

    public void RefreshHeldItemVisual()
    {
        if (heldItem == null)
        {
            heldItemRenderer.sprite = null;
            heldItemRenderer.enabled = false;
            return;
        }
        heldItemRenderer.sprite = heldItem.InventoryItem.ItemImage;
        heldItemRenderer.enabled = true;
    }
    public void HoldInventoryItem(ItemSO itemSO)
    {
        if (heldItem != null)
            DropItem();
        GameObject go = Instantiate(itemSO.WorldPrefab);
        heldItem = go.GetComponent<Item>();
        heldItem.HideWorldRepresentation();
        heldItemRenderer.sprite = itemSO.ItemImage;
        heldItemRenderer.enabled = true;
        UIActionBar.Instance.ShowActions(heldItem.GetActions());
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponentInParent<IInteractable>();
        if (interactable == null)
            interactable = other.GetComponentInChildren<IInteractable>();
        if (interactable == null)
            return;
        if (heldItem != null && interactable is Item)
            return;
        if (currentInteractable == interactable)
            return;
        currentInteractable = interactable;
        promptUI.Show(interactable.InteractionAnchor);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactable = other.GetComponentInParent<IInteractable>();
        if (interactable == null)
            interactable = other.GetComponentInChildren<IInteractable>();
        if (interactable == null)
            return;
        if (currentInteractable != interactable)
            return;
        currentInteractable = null;
        promptUI.Hide();
    }
}