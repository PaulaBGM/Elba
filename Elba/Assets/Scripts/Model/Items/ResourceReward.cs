using System;
using UnityEngine;
using Inventory.Model;

[Serializable]
public class ResourceReward
{
    public ItemSO item;

    public int minAmount = 1;
    public int maxAmount = 1;

    [Range(0f, 100f)]
    public float dropChance = 100f;
}