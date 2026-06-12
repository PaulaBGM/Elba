using UnityEngine;
using Inventory.Model;

[CreateAssetMenu(menuName = "Inventory/Cooking Recipe")]
public class CookingRecipeSO : ScriptableObject
{
    public ItemSO rawItem;
    public ItemSO cookedItem;
}