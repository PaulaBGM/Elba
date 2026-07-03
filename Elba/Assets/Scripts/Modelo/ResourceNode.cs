using UnityEngine;

public class ResourceNode : MonoBehaviour, IAttackable
{
    [Header("Durability")]
    [SerializeField] private int maxDurability = 3;

    [Header("Drops")]
    [SerializeField] protected ResourceReward[] rewards;

    [Header("Interaction")]
    [SerializeField] private Transform interactionAnchor;

    protected int currentDurability;

    public Transform InteractionAnchor => interactionAnchor;

    public bool IsDestroyed => currentDurability <= 0;

    protected virtual void Awake()
    {
        // Solo referencias. No llamar aquí a métodos virtuales.
    }

    protected virtual void Start()
    {
        ResetNode();
    }

    public virtual void ResetNode()
    {
        currentDurability = maxDurability;
    }

    public virtual void ReceiveHit(GameObject attacker, float damage)
    {
        if (!CanReceiveDamage(attacker))
            return;

        currentDurability -= Mathf.RoundToInt(damage);

        OnHit(attacker);

        if (currentDurability > 0)
            return;

        DestroyNode();
    }

    protected virtual bool CanReceiveDamage(GameObject attacker)
    {
        return true;
    }

    protected virtual void OnHit(GameObject attacker)
    {
    }

    protected virtual void DestroyNode()
    {
        SpawnRewards(rewards);

        OnDestroyed();

        gameObject.SetActive(false);
    }

    protected virtual void OnDestroyed()
    {
    }

    protected void SpawnRewards(ResourceReward[] rewardList)
    {
        if (rewardList == null)
            return;

        foreach (ResourceReward reward in rewardList)
        {
            if (reward.item == null)
                continue;

            if (!reward.RollDrop())
                continue;

            int amount = reward.GetAmount();

            for (int i = 0; i < amount; i++)
            {
                Instantiate(
                    reward.item.WorldPrefab,
                    transform.position + (Vector3)Random.insideUnitCircle * 0.75f,
                    Quaternion.identity);
            }
        }
    }

    /*
    private void DamageTool(GameObject attacker)
    {
        AgentWeapon weapon = attacker.GetComponent<AgentWeapon>();

        if (weapon != null)
            weapon.DamageTool(1);
    }
    */
}