using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    Transform InteractionAnchor { get; }

    List<ActionData> GetActions();

    void Interact(GameObject interactor);
}