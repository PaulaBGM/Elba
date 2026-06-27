using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject root;

    [SerializeField] private UICraftingRecipe recipePrefab;
    [SerializeField] private Transform content;

    [Header("Selected Recipe")]
    [SerializeField] private TMP_Text recipeName;
    [SerializeField] private TMP_Text materialsText;
    [SerializeField] private Button craftButton;

    [Header("References")]
    [SerializeField] private CraftingManager craftingManager;

    private readonly List<UICraftingRecipe> recipesUI = new();

    private CraftingRecipeSO selectedRecipe;

    private void Awake()
    {
        if (root != null)
            root.SetActive(false);

        craftButton.onClick.AddListener(CraftSelectedRecipe);
    }

    private void Start()
    {
        BuildRecipes();
    }

    public void Open()
    {
        root.SetActive(true);
    }

    public void Close()
    {
        root.SetActive(false);
    }

    private void BuildRecipes()
    {
        foreach (CraftingRecipeSO recipe in craftingManager.Recipes)
        {
            UICraftingRecipe uiRecipe =
                Instantiate(recipePrefab, content);

            uiRecipe.Setup(recipe);

            uiRecipe.OnRecipeClicked += HandleRecipeClicked;

            recipesUI.Add(uiRecipe);
        }
    }

    private void HandleRecipeClicked(
        UICraftingRecipe recipeUI)
    {
        foreach (UICraftingRecipe recipe in recipesUI)
        {
            recipe.Deselect();
        }

        recipeUI.Select();

        selectedRecipe = recipeUI.Recipe;

        RefreshSelectedRecipe();
    }

    private void RefreshSelectedRecipe()
    {
        if (selectedRecipe == null)
            return;

        recipeName.text =
            selectedRecipe.result.Name;

        materialsText.text =
            BuildIngredientsText(selectedRecipe);
    }

    private string BuildIngredientsText(
        CraftingRecipeSO recipe)
    {
        System.Text.StringBuilder sb = new();

        foreach (var ingredient in recipe.ingredients)
        {
            int amount =
                craftingManager.GetItemCount(
                    ingredient.item);

            sb.AppendLine(
                $"{ingredient.item.Name} {amount}/{ingredient.amount}");
        }

        return sb.ToString();
    }

    private void CraftSelectedRecipe()
    {
        if (selectedRecipe == null)
            return;

        bool crafted =
            craftingManager.Craft(selectedRecipe);

        if (!crafted)
        {
            Debug.Log("Faltan materiales");
            return;
        }

        RefreshSelectedRecipe();
    }
}