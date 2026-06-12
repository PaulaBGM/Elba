using System.Collections.Generic;
using UnityEngine;
using Inventory.Model;

public class ResourceNode : MonoBehaviour, IInteractable
{
    [SerializeField] private List<ResourceReward> rewards = new();
    [SerializeField] private int durability = 3;

    public void Interact(GameObject interactor)
    {
        durability--;

        if (durability > 0)
            return;

        SpawnRewards();

        Destroy(gameObject);
    }

    private void SpawnRewards()
    {
        foreach (ResourceReward reward in rewards)
        {
            if (reward.item == null)
                continue;

            if (reward.item.WorldPrefab == null)
            {
                Debug.LogWarning( $"El item {reward.item.name} no tiene WorldPrefab asignado.");
                continue;
            }

            for (int i = 0; i < reward.amount; i++)
            {
                Vector2 offset = Random.insideUnitCircle * 0.5f;

                Instantiate(reward.item.WorldPrefab,transform.position + (Vector3)offset, Quaternion.identity);
            }
        }
    }
}