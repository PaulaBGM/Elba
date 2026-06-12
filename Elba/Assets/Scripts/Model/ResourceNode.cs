using System.Collections;
using UnityEngine;
using Inventory.Model;

public class ResourceNode : MonoBehaviour, IAttackable
{
    [Header("Stats")]
    [SerializeField] private int durability = 3;

    [Header("Tools")]
    [SerializeField] private ToolType requiredTool = ToolType.None;

    [Header("Drops")]
    [SerializeField] private ResourceReward[] rewards;

    [Header("Feedback")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float flashDuration = 0.1f;

    private Color originalColor;

    private void Awake()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        originalColor = spriteRenderer.color;
    }

    public void ReceiveHit(GameObject attacker)
    {
        durability--;

        Debug.Log($"{name} Durabilidad restante: {durability}");

        if (durability > 0)
            return;

        SpawnRewards();

        Destroy(gameObject);
    }

    private IEnumerator HitFeedback()
    {
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.color = originalColor;
    }

    private void SpawnRewards()
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