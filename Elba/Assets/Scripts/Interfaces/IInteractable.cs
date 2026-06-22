using UnityEngine;

public interface IInteractable
{
    Transform InteractionAnchor { get; }

    void Interact(GameObject interactor);
}