using UnityEngine;

public class SurvivalSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerStatsSystem stats;

    [Header("Hunger")]
    [SerializeField] private float hungerDecayPerSecond = 0.15f;

    [Header("Thirst")]
    [SerializeField] private float thirstDecayPerSecond = 0.25f;

    [Header("Critical Damage")]
    [SerializeField] private float starvationDamagePerSecond = 2f;
    [SerializeField] private float dehydrationDamagePerSecond = 4f;

    private void Awake()
    {
        if (stats == null)
            stats = GetComponent<PlayerStatsSystem>();
    }

    private void Update()
    {
        UpdateHunger();
        UpdateThirst();
        ApplyCriticalEffects();
    }

    private void UpdateHunger()
    {
        stats.ModifyStat(
            StatType.Hunger,
            -hungerDecayPerSecond * Time.deltaTime);
    }

    private void UpdateThirst()
    {
        stats.ModifyStat(
            StatType.Thirst,
            -thirstDecayPerSecond * Time.deltaTime);
    }

    private void ApplyCriticalEffects()
    {
        if (stats.IsEmpty(StatType.Hunger))
        {
            stats.ModifyStat(
                StatType.Health,
                -starvationDamagePerSecond * Time.deltaTime);
        }

        if (stats.IsEmpty(StatType.Thirst))
        {
            stats.ModifyStat(
                StatType.Health,
                -dehydrationDamagePerSecond * Time.deltaTime);
        }
    }
}