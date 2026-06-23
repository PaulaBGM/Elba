using UnityEngine;

public class PlayerInteractionSystem : MonoBehaviour
{
    [SerializeField] private InputReader input;
    [SerializeField] private InteractionPromptUI promptUI;

    private IInteractable currentInteractable;

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
        currentInteractable?.Interact(gameObject);
    }
    private void StoreItem()
    {
        if (currentInteractable is not Item item)
            return;

        item.Store(gameObject);

        ClearInteraction();
    }

    private void ConsumeItem()
    {
        if (currentInteractable is not Item item)
            return;

        item.Consume(gameObject);

        ClearInteraction();
    }
    private void ClearInteraction()
    {
        currentInteractable = null;

        promptUI.Hide();

        UIActionBar.Instance.Hide();
    }
    private void DropItem()
    {
        if (currentInteractable is not Item item)
            return;

        item.Drop(gameObject);

        ClearInteraction();
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        IInteractable interactable =
            other.GetComponentInParent<IInteractable>();

        if (interactable == null)
            interactable =
                other.GetComponentInChildren<IInteractable>();

        if (interactable == null)
            return;

        if (currentInteractable == interactable)
            return;

        currentInteractable = interactable;

        promptUI.Show(
            interactable.InteractionAnchor);

        UIActionBar.Instance.ShowActions(interactable.GetActions());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactable =
            other.GetComponentInParent<IInteractable>();

        if (interactable == null)
            interactable =
                other.GetComponentInChildren<IInteractable>();

        if (interactable == null)
            return;

        if (currentInteractable != interactable)
            return;

        currentInteractable = null;

        promptUI.Hide();

        UIActionBar.Instance.Hide();
    }
}