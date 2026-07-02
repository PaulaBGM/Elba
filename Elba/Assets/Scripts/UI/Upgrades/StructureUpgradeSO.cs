using System.Collections.Generic;
using UnityEngine;
using Inventory.Model;

[CreateAssetMenu(fileName = "StructureUpgradeSO", menuName = "Scriptable Objects/StructureUpgradeSO")]
public class StructureUpgradeSO : ScriptableObject
{
    [field: SerializeField]
    public string Title { get; private set; }

    [field: TextArea]
    [field: SerializeField]
    public string Description { get; private set; }

    [field: SerializeField]
    public List<RecipeIngredient> Requirements { get; private set; } = new();

    [field: SerializeField]
    public GameObject UpgradedPrefab { get; private set; }
}