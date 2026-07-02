using System.Collections.Generic;
using Inventory.Model;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject root;
    [SerializeField] private InputReader input;
    [SerializeField] private UICraftingRecipe recipePrefab;
    [SerializeField] private Transform content;

    [Header("Description")]
    [SerializeField] private CraftDescriptionUI descriptionUI;
    [SerializeField] private Button craftButton;

    [Header("Icons")]
    [SerializeField] private Sprite damageIcon;

    [Header("References")]
    [SerializeField] private CraftingManager craftingManager;

    private readonly List<UICraftingRecipe> recipesUI = new();
    private CraftingRecipeSO selectedRecipe;
    private bool recipesBuilt;

    private void Awake()
    {
        if (root != null)
            root.SetActive(false);

        if (input == null)
            input = FindFirstObjectByType<InputReader>();

        if (craftButton != null)
            craftButton.onClick.AddListener(CraftSelectedRecipe);
    }

    private void OnEnable()
    {
        if (input != null)
            input.OnInventory += HandleTabPressed;
    }

    private void OnDisable()
    {
        if (input != null)
            input.OnInventory -= HandleTabPressed;
    }

    private void HandleTabPressed()
    {
        if (!root.activeSelf)
            return;

        Close();
    }

    public void Open()
    {
        if (root != null)
            root.SetActive(true);

        BuildRecipesIfNeeded();

        if (selectedRecipe != null)
            RefreshSelectedRecipe();
    }

    public void Close()
    {
        if (root != null)
            root.SetActive(false);
    }

    private void BuildRecipesIfNeeded()
    {
        if (recipesBuilt)
            return;

        recipesBuilt = true;
        BuildRecipes();
    }

    private void BuildRecipes()
    {
        foreach (CraftingRecipeSO recipe in craftingManager.Recipes)
        {
            UICraftingRecipe uiRecipe = Instantiate(recipePrefab, content);
            uiRecipe.Setup(recipe);
            uiRecipe.OnRecipeClicked += HandleRecipeClicked;
            recipesUI.Add(uiRecipe);
        }
    }

    private void HandleRecipeClicked(UICraftingRecipe recipeUI)
    {
        foreach (UICraftingRecipe recipe in recipesUI)
            recipe.Deselect();

        recipeUI.Select();
        selectedRecipe = recipeUI.Recipe;
        RefreshSelectedRecipe();
    }

    private void RefreshSelectedRecipe()
    {
        if (selectedRecipe == null)
            return;

        descriptionUI.ResetDescription();
        descriptionUI.SetDescription(
            selectedRecipe.result.ItemImage,
            selectedRecipe.result.Name,
            selectedRecipe.result.Description);

        descriptionUI.ClearStats();

        if (selectedRecipe.result is EdibleItemSO edible)
        {
            foreach (var modifier in edible.ModifiersData)
                descriptionUI.AddStat(modifier.statModifier.Icon, modifier.value);
        }
        else if (selectedRecipe.result is ToolItemSO tool)
        {
            descriptionUI.AddStat(damageIcon, tool.AnimalDamage);
        }

        descriptionUI.ClearIngredients();

        foreach (RecipeIngredient ingredient in selectedRecipe.ingredients)
        {
            int owned = craftingManager.GetItemCount(ingredient.item);
            descriptionUI.AddIngredient(
                ingredient.item.ItemImage,
                ingredient.item.Name,
                owned,
                ingredient.amount);
        }

        craftButton.interactable = craftingManager.CanCraft(selectedRecipe);
    }

    private void CraftSelectedRecipe()
    {
        if (selectedRecipe == null)
            return;

        if (!craftingManager.Craft(selectedRecipe))
            return;

        RefreshSelectedRecipe();
    }
}