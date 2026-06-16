using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UICraftingRecipe :
    MonoBehaviour,
    IPointerClickHandler
{
    [SerializeField] private Image recipeImage;
    [SerializeField] private Image borderImage;

    private CraftingRecipeSO recipe;

    public event Action<UICraftingRecipe> OnRecipeClicked;

    private void Awake()
    {
        Deselect();
    }

    public void Setup(CraftingRecipeSO recipe)
    {
        this.recipe = recipe;

        recipeImage.sprite = recipe.result.ItemImage;
    }

    public CraftingRecipeSO Recipe => recipe;

    public void Select()
    {
        borderImage.enabled = true;
    }

    public void Deselect()
    {
        borderImage.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left)
            return;

        OnRecipeClicked?.Invoke(this);
    }
}