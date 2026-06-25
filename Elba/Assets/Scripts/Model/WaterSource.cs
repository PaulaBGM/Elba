using UnityEngine;
using System.Collections.Generic;
using Inventory.Model;
using Inventory;

public class WaterSource : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform interactionAnchor;
    [SerializeField] private WaterCupItemSO filledCup;

    public Transform InteractionAnchor => interactionAnchor;

    public List<ActionData> GetActions() => new();

    public void Interact(GameObject interactor)
    {
        AgentWeapon equipment =
            interactor.GetComponent<AgentWeapon>();

        if (equipment == null)
            return;

        if (equipment.CurrentWeapon is not CupItemSO)
            return;

        equipment.UnequipWeapon();

        InventoryController inventory =
            interactor.GetComponent<InventoryController>();

        inventory.InventoryData.AddItem(
            filledCup,
            1);

        Debug.Log("Vaso rellenado.");
    }
}