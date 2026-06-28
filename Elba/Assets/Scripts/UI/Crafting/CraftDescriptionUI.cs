using Inventory.UI;
using UnityEngine;

public class CraftDescriptionUI : UIInventoryDescription
{
    [Header("Craft")]
    [SerializeField] private Transform materialsParent;
    [SerializeField] private UIIngredientRow ingredientPrefab;

    public void ClearIngredients()
    {
        foreach (Transform child in materialsParent)
            Destroy(child.gameObject);
    }

    public void AddIngredient(Sprite icon,string name,int owned, int required)
    {
        UIIngredientRow row =  Instantiate(ingredientPrefab, materialsParent);
        row.Setup(icon, name, owned, required);
    }

    public override void ResetDescription()
    {
        base.ResetDescription();
        ClearIngredients();
    }
}