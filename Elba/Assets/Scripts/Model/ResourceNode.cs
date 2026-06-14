using System.Collections;
using UnityEngine;
using Inventory.Model;

public class ResourceNode : MonoBehaviour, IAttackable
{
    [SerializeField] private int maxDurability = 3;
    [Header("Drops")]
    [SerializeField] private ResourceReward[] rewards;
    private int currentDurability;

    private void OnEnable()
    {
        ResetNode();
    }

    public void ResetNode()
    {
        currentDurability = maxDurability;
    }

    public void ReceiveHit(GameObject attacker)
    {
        currentDurability--;

        if (currentDurability > 0)
            return;

        SpawnRewards();

        gameObject.SetActive(false);
    }

    private void SpawnRewards()
    {
        foreach (ResourceReward reward in rewards)
        {
            float roll = Random.Range(0f, 100f);

            if (roll > reward.dropChance)
                continue;

            int amount = Random.Range(reward.minAmount,reward.maxAmount + 1);

            for (int i = 0; i < amount; i++)
            {
                Instantiate(reward.item.WorldPrefab,transform.position +(Vector3)Random.insideUnitCircle * 0.75f, Quaternion.identity);
            }
        }
    }
}