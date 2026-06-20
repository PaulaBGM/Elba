using System.Collections.Generic;
using UnityEngine;
using Inventory.Model;

public class AgentWeapon : MonoBehaviour
{
    [Header("Current Weapon")]
    [SerializeField] private EquippableItemSO weapon;

    [Header("Inventory")]
    [SerializeField] private InventorySO inventoryData;

    [Header("Parameters")]
    [SerializeField] private List<ItemParameter> parametersToModify = new();
    [SerializeField] private List<ItemParameter> itemCurrentState = new();
    [SerializeField]
    private ItemParameterSO durabilityParameter;

    [Header("Visual")]
    [SerializeField] private SpriteRenderer armPointRenderer;

    public EquippableItemSO CurrentWeapon => weapon;

    public ToolType CurrentToolType
    {
        get
        {
            if (weapon is ToolItemSO tool)
                return tool.ToolType;

            return ToolType.None;
        }
    }

    private void Awake()
    {
        UpdateWeaponVisual();
    }

    public bool HasTool(ToolType toolType)
    {
        return CurrentToolType == toolType;
    }

    public void SetWeapon( EquippableItemSO weaponItemSO,List<ItemParameter> itemState)
    {
        if (weapon != null)
        {
            inventoryData.AddItem(
                weapon,
                1,
                itemCurrentState);
        }

        weapon = weaponItemSO;

        itemCurrentState =
            itemState != null
            ? new List<ItemParameter>(itemState)
            : new List<ItemParameter>();

        ModifyParameters();

        UpdateWeaponVisual();

        Debug.Log($"Equipada herramienta: {weapon.Name}");
    }
    public bool IsBroken()
    {
        return GetDurability() <= 0;
    }
    public void UnequipWeapon()
    {
        if (weapon == null)
            return;

        inventoryData.AddItem(
            weapon,
            1,
            itemCurrentState);

        Debug.Log($"Desequipada herramienta: {weapon.Name}");

        weapon = null;
        itemCurrentState.Clear();

        UpdateWeaponVisual();
    }

    private void ModifyParameters()
    {
        foreach (ItemParameter modifier in parametersToModify)
        {
            for (int i = 0; i < itemCurrentState.Count; i++)
            {
                if (itemCurrentState[i].itemParameter != modifier.itemParameter)
                    continue;

                itemCurrentState[i] = new ItemParameter
                {
                    itemParameter = modifier.itemParameter,
                    value = itemCurrentState[i].value + modifier.value
                };
            }
        }
    }

    private void UpdateWeaponVisual()
    {
        if (armPointRenderer == null)
            return;

        if (weapon == null)
        {
            armPointRenderer.sprite = null;
            armPointRenderer.enabled = false;
            return;
        }

        armPointRenderer.sprite = weapon.ItemImage;
        armPointRenderer.enabled = true;
    }
    public float GetDurability()
    {
        foreach (ItemParameter parameter in itemCurrentState)
        {
            if (parameter.itemParameter == durabilityParameter)
                return parameter.value;
        }

        return 0;
    }

    public void DamageTool(float amount)
    {
        for (int i = 0; i < itemCurrentState.Count; i++)
        {
            if (itemCurrentState[i].itemParameter != durabilityParameter)
                continue;

            itemCurrentState[i] = new ItemParameter
            {
                itemParameter = durabilityParameter,
                value = Mathf.Max(
                    0,
                    itemCurrentState[i].value - amount)
            };

            Debug.Log(
                $"Durabilidad restante: {itemCurrentState[i].value}");

            break;
        }
    }
    public int GetResourceDamage()
    {
        if (weapon is ToolItemSO tool)
            return tool.ResourceDamage;

        return 1;
    }

    public float GetAnimalDamage()
    {
        if (weapon is ToolItemSO tool)
            return tool.AnimalDamage;

        return 5f;
    }

    public int GetDurabilityCost()
    {
        if (weapon is ToolItemSO tool)
            return tool.DurabilityCost;

        return 1;
    }
   
}