using System.Collections.Generic;
using UnityEngine;
using Inventory;
using Inventory.Model;

public class WaterSource : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform interactionAnchor;
    [SerializeField] private WaterCupItemSO filledCup;

    public Transform InteractionAnchor => interactionAnchor;

    public List<ActionData> GetActions()
    {
        return new();
    }

    public void Interact(GameObject interactor)
    {
        AgentWeapon equipment = interactor.GetComponent<AgentWeapon>();
        InventoryController inventory = interactor.GetComponent<InventoryController>();

        if (equipment == null || inventory == null)
            return;

        if (equipment.CurrentWeapon is not CupItemSO)
            return;

        equipment.RemoveEquippedItem();

        inventory.InventoryData.AddItem(filledCup, 1);

        Debug.Log("[Water] Vaso rellenado.");
    }
}