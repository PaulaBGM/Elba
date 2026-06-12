using UnityEngine;

public class PlayerInteractionSystem : MonoBehaviour
{
    [SerializeField] private InputReader input;
    [SerializeField] private InteractionPromptUI promptUI;

    private IInteractable currentInteractable;

    private void OnEnable()
    {
        if (input != null)
            input.OnInteract += Interact;
    }

    private void OnDisable()
    {
        if (input != null)
            input.OnInteract -= Interact;
    }

    private void Interact()
    {
        currentInteractable?.Interact(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        IInteractable interactable =
            other.GetComponentInParent<IInteractable>();

        if (interactable == null)
            interactable = other.GetComponentInChildren<IInteractable>();

        if (interactable == null)
            return;

        currentInteractable = interactable;

        Item item = other.GetComponentInParent<Item>();

        if (item != null)
        {
            if (item.IsEdible)
            {
                promptUI.Show("[E] Guardar\n[F] Consumir");
            }
            else
            {
                promptUI.Show("[E] Recoger");
            }

            return;
        }

        ResourceNode resourceNode =
            other.GetComponentInParent<ResourceNode>();

        if (resourceNode != null)
        {
            promptUI.Show("[Click Izq] Recolectar");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        IInteractable interactable =
            other.GetComponentInParent<IInteractable>();

        if (interactable == null)
            interactable = other.GetComponentInChildren<IInteractable>();

        if (interactable == null)
            return;

        if (currentInteractable == interactable)
        {
            currentInteractable = null;
            promptUI.Hide();
        }
    }
}