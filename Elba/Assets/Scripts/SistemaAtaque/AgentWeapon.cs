using Inventory.Model;
using System.Collections.Generic;
using UnityEngine;

public class AgentWeapon : MonoBehaviour
{
    [Header("Current Weapon")]
    [SerializeField] private EquippableItemSO weapon;

    [Header("Inventory")]
    [SerializeField] private InventorySO inventoryData;

    [Header("Parameters")]
    [SerializeField] private List<ItemParameter> parametersToModify = new();
    [SerializeField] private List<ItemParameter> itemCurrentState = new();

    [Header("Visual")]
    [SerializeField] private Transform armPoint;
    [SerializeField] private SpriteRenderer weaponRenderer;

    public EquippableItemSO CurrentWeapon => weapon;
    public ToolItemSO CurrentTool => weapon as ToolItemSO;

    private void Awake()
    {
        if (armPoint == null)
            armPoint = transform.Find("ArmPoint");

        if (weaponRenderer == null && armPoint != null)
            weaponRenderer = armPoint.GetComponent<SpriteRenderer>();

        RefreshVisual();
    }

    public void SetWeapon(EquippableItemSO weaponItemSO, List<ItemParameter> itemState)
    {
        if (weapon != null)
        {
            inventoryData.AddItem(
                weapon,
                1,
                itemCurrentState);
        }

        weapon = weaponItemSO;
        itemCurrentState = new List<ItemParameter>(itemState);

        ModifyParameters();
        RefreshVisual();
    }

    private void ModifyParameters()
    {
        foreach (var modifier in parametersToModify)
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

    public void RemoveEquippedItem()
    {
        weapon = null;
        itemCurrentState.Clear();

        RefreshVisual();
    }

    private void RefreshVisual()
    {
        if (weaponRenderer == null)
            return;

        if (weapon == null)
        {
            weaponRenderer.sprite = null;
            weaponRenderer.enabled = false;
            return;
        }
      
        weaponRenderer.sprite = weapon.ItemImage;
        weaponRenderer.enabled = true;
    }

    public float GetAnimalDamage()
    {
        return CurrentTool != null
            ? CurrentTool.AnimalDamage
            : 1f;
    }

    public int GetResourceDamage()
    {
        return CurrentTool != null
            ? CurrentTool.ResourceDamage
            : 1;
    }

    public int GetDurabilityCost()
    {
        return CurrentTool != null
            ? CurrentTool.DurabilityCost
            : 0;
    }

    public void ConsumeDurability()
    {
        if (CurrentTool == null)
            return;

    }
    public bool HasTool(ToolType toolType)
    {
        return CurrentTool != null &&
               CurrentTool.ToolType == toolType;
    }

    public bool CanChopTrees()
    {
        return HasTool(ToolType.Axe);
    }

    public bool CanAttackAnimals()
    {
        return CurrentTool != null;
    }
}