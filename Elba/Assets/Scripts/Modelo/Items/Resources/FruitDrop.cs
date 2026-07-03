using System;
using UnityEngine;
using Inventory.Model;

[Serializable]
public class FruitDrop
{
    public ItemSO fruit;

    [Range(0f, 100f)]
    public float dropChance = 35f;

    public int minAmount = 1;
    public int maxAmount = 2;
}