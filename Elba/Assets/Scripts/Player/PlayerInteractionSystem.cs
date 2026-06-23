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