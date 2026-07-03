using System;
using UnityEngine;
using Inventory.Model;

[Serializable]
public class ResourceReward
{
    public ItemSO item;

    [Min(1)]
    public int minAmount = 1;

    [Min(1)]
    public int maxAmount = 1;

    [Range(0f, 100f)]
    public float dropChance = 100f;

    public bool RollDrop()
    {
        return UnityEngine.Random.Range(0f, 100f) <= dropChance;
    }

    public int GetAmount()
    {
        return UnityEngine.Random.Range(minAmount, maxAmount + 1);
    }
}