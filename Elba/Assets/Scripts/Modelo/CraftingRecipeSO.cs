using UnityEngine;
using System.Collections.Generic;
using Inventory.Model;

[CreateAssetMenu(fileName = "CraftingRecipeSO", menuName = "Scriptable Objects/CraftingRecipeSO")]
public class CraftingRecipeSO : ScriptableObject
{
    public List<RecipeIngredient> ingredients;
    public ItemSO result;
    public int amount;
}