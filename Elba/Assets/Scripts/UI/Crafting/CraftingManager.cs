using Inventory;
using Inventory.Model;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    [SerializeField] private CraftingRecipeSO[] recipes;
    [SerializeField] private InventoryController inventory;

    public CraftingRecipeSO[] Recipes => recipes;

    public bool Craft(CraftingRecipeSO recipe)
    {
        if (!inventory.HasItems(recipe.ingredients))
            return false;

        inventory.ConsumeIngredients(recipe.ingredients);
        inventory.AddItem(recipe.result,recipe.amount);

        return true;
    }
    public bool CanCraft(CraftingRecipeSO recipe)
    {
        return inventory.HasItems(recipe.ingredients);
    }
    public int GetItemCount(ItemSO item)
    {
        return inventory.GetItemCount(item);
    }
}