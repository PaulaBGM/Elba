using System.Collections.Generic;
using UnityEngine;
using Inventory.Model;

public class AgentWeapon : MonoBehaviour
{
    [Header("Current Equipment")]
    [SerializeField] private EquippableItemSO equippedItem;

    [Header("Inventory")]
    [SerializeField] private InventorySO inventoryData;

    [Header("Parameters")]
    [SerializeField] private List<ItemParameter> parametersToModify = new();
    [SerializeField] private List<ItemParameter> itemCurrentState = new();

    [SerializeField] private ItemParameterSO durabilityParameter;

    [Header("Visual")]
    [SerializeField] private SpriteRenderer armPointRenderer;

    public EquippableItemSO CurrentWeapon => equippedItem;
    public EquippableItemSO EquippedItem => equippedItem;
    public bool HasEquippedItem => equippedItem != null;

    public ToolType CurrentToolType
    {
        get
        {
            if (equippedItem is ToolItemSO tool)
                return tool.ToolType;

            return ToolType.None;
        }
    }

    private void Awake()
    {
        UpdateVisual();
    }

    public bool HasTool(ToolType toolType)
    {
        return CurrentToolType == toolType;
    }

    public T GetEquipped<T>() where T : EquippableItemSO
    {
        return equippedItem as T;
    }

    public void SetWeapon(
        EquippableItemSO item,
        List<ItemParameter> itemState)
    {
        if (equippedItem != null)
        {
            inventoryData.AddItem(
                equippedItem,
                1,
                itemCurrentState);
        }

        equippedItem = item;

        itemCurrentState =
            itemState != null
            ? new List<ItemParameter>(itemState)
            : new List<ItemParameter>();

        ModifyParameters();

        UpdateVisual();

        Debug.Log($"[Equipment] Equipado: {equippedItem.Name}");
    }

    public void UnequipWeapon()
    {
        if (equippedItem == null)
            return;

        inventoryData.AddItem(
            equippedItem,
            1,
            itemCurrentState);

        Debug.Log($"[Equipment] Desequipado: {equippedItem.Name}");

        equippedItem = null;
        itemCurrentState.Clear();

        UpdateVisual();
    }
    public void RemoveEquippedItem()
    {
        if (equippedItem == null)
            return;

        Debug.Log($"[Equipment] Eliminando {equippedItem.Name}");

        equippedItem = null;
        itemCurrentState.Clear();

        UpdateVisual();
    }

    public void RefreshVisual()
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (armPointRenderer == null)
            return;

        if (equippedItem == null)
        {
            armPointRenderer.sprite = null;
            armPointRenderer.enabled = false;
            return;
        }

        if (equippedItem is CupItemSO cup)
        {
            armPointRenderer.sprite = cup.CurrentSprite;
        }
        else
        {
            armPointRenderer.sprite = equippedItem.ItemImage;
        }

        armPointRenderer.enabled = true;
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

    public float GetDurability()
    {
        foreach (ItemParameter parameter in itemCurrentState)
        {
            if (parameter.itemParameter == durabilityParameter)
                return parameter.value;
        }

        return 0;
    }

    public bool IsBroken()
    {
        return GetDurability() <= 0;
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
        if (equippedItem is ToolItemSO tool)
            return tool.ResourceDamage;

        return 1;
    }

    public float GetAnimalDamage()
    {
        if (equippedItem is ToolItemSO tool)
            return tool.AnimalDamage;

        return 5f;
    }

    public int GetDurabilityCost()
    {
        if (equippedItem is ToolItemSO tool)
            return tool.DurabilityCost;

        return 1;
    }
}