using UnityEngine;

public class PlayerInteractionSystem : MonoBehaviour
{
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private IInteractable currentInteractable;

    private void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            currentInteractable?.Interact(gameObject);
        }
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
            currentInteractable = null;
    }
}
