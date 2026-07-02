using TMPro;
using UnityEngine;

public class StructureDescriptionUI : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private TMP_Text title;
    [SerializeField] private TMP_Text description;

    [Header("Materials")]
    [SerializeField] private Transform materialsParent;
    [SerializeField] private UIIngredientRow ingredientPrefab;

    private void Awake()
    {
        ResetDescription();
    }

    public void ResetDescription()
    {
        title.text = string.Empty;
        description.text = string.Empty;

        ClearIngredients();
    }

    public void SetDescription(string structureTitle, string structureDescription)
    {
        title.text = structureTitle;
        description.text = structureDescription;
    }

    public void AddIngredient(Sprite icon, string name, int owned, int required)
    {
        UIIngredientRow row = Instantiate(ingredientPrefab, materialsParent);
        row.Setup(icon, name, owned, required);
    }

    public void ClearIngredients()
    {
        foreach (Transform child in materialsParent)
            Destroy(child.gameObject);
    }
}