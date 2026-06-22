using UnityEngine;

public class ResourceNode : MonoBehaviour, IAttackable
{
    [SerializeField] private int maxDurability = 3;

    [Header("Drops")]
    [SerializeField] private ResourceReward[] rewards;

    protected int currentDurability;

    [SerializeField] private Transform interactionAnchor;
    public Transform InteractionAnchor => interactionAnchor;

    private void OnEnable()
    {
        ResetNode();
    }

    public virtual void ResetNode()
    {
        currentDurability = maxDurability;
    }

    public virtual void ReceiveHit(GameObject attacker)
    {
        AgentWeapon weapon = attacker.GetComponent<AgentWeapon>();

        if (weapon != null)
        {
            weapon.DamageTool(1);
        }

        if (currentDurability > 0)
            return;

        SpawnRewards();

        gameObject.SetActive(false);
    }

    protected virtual void SpawnRewards()
    {
        foreach (ResourceReward reward in rewards)
        {
            float roll = Random.Range(0f, 100f);

            if (roll > reward.dropChance)
                continue;

            int amount = Random.Range(reward.minAmount, reward.maxAmount + 1);

            for (int i = 0; i < amount; i++)
            {
                Instantiate(
                    reward.item.WorldPrefab,
                    transform.position + (Vector3)Random.insideUnitCircle * 0.75f,
                    Quaternion.identity);
            }
        }
    }
}