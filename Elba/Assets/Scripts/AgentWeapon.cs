using Inventory.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentWeapon : MonoBehaviour
{
    [SerializeField] private EquippableItemSO weapon;

    [SerializeField]
    private InventorySO inventoryData;

    [SerializeField]
    private List<ItemParameter> parametersToModify, itemCurrentState;

    public void SetWeapon(EquippableItemSO weaponItemSO, List<ItemParameter> itemState)
    {
        if (weapon != null)
        {
            inventoryData.AddItem(weapon, 1, itemCurrentState);
        }

        this.weapon = weaponItemSO;
        this.itemCurrentState = new List<ItemParameter>(itemState);
        ModifyParameters();
    }

    private void ModifyParameters()
    {
        foreach (var modifier in parametersToModify)
        {
            for (int i = 0; i < itemCurrentState.Count; i++)
            {
                if (itemCurrentState[i].itemParameter == modifier.itemParameter)
                {
                    itemCurrentState[i] = new ItemParameter
                    {
                        itemParameter = modifier.itemParameter,
                        value = itemCurrentState[i].value + modifier.value
                    };
                }
            }
        }
    }
}