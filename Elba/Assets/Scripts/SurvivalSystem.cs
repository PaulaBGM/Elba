using UnityEngine;

public class SurvivalSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerStatsSystem stats;

    [Header("Tick")]
    [SerializeField] private float tickRate = 1f;

    [Header("Needs")]
    [SerializeField] private float hungerDecayPerTick = 0.5f;
    [SerializeField] private float thirstDecayPerTick = 1f;

    [Header("Critical Damage")]
    [SerializeField] private float starvationDamagePerTick = 2f;
    [SerializeField] private float dehydrationDamagePerTick = 4f;

    private float timer;

    private void Awake()
    {
        if (stats == null)
            stats = GetComponent<PlayerStatsSystem>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer < tickRate)
            return;

        timer = 0f;

        UpdateNeeds();
    }

    private void UpdateNeeds()
    {
        stats.ModifyStat(
            StatType.Hunger,
            -hungerDecayPerTick);

        stats.ModifyStat(
            StatType.Thirst,
            -thirstDecayPerTick);

        ApplyCriticalEffects();
    }

    private void ApplyCriticalEffects()
    {
        if (stats.IsEmpty(StatType.Hunger))
        {
            stats.ModifyStat(
                StatType.Health,
                -starvationDamagePerTick);
        }

        if (stats.IsEmpty(StatType.Thirst))
        {
            stats.ModifyStat(
                StatType.Health,
                -dehydrationDamagePerTick);
        }
    }
}