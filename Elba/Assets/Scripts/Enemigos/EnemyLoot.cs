using Inventory.Model;
using UnityEngine;

public class EnemyLoot : MonoBehaviour
{
    [SerializeField] private ResourceReward[] rewards;

    public void DropLoot()
    {
        foreach (ResourceReward reward in rewards)
        {
            float roll = Random.Range(0f, 100f);

            if (roll > reward.dropChance)
                continue;

            int amount =
                Random.Range(
                    reward.minAmount,
                    reward.maxAmount + 1);

            for (int i = 0; i < amount; i++)
            {
                Instantiate(
                    reward.item.WorldPrefab,
                    transform.position +
                    (Vector3)Random.insideUnitCircle * 0.75f,
                    Quaternion.identity);
            }
        }
    }
}