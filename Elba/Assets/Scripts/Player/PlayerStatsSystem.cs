using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsSystem : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private float maxHealth = 100f;

    [Header("Hunger")]
    [SerializeField] private float maxHunger = 100f;

    [Header("Thirst")]
    [SerializeField] private float maxThirst = 100f;

    [Header("Stamina")]
    [SerializeField] private float maxStamina = 100f;

    [Header("Temperature")]
    [SerializeField] private float maxTemperature = 100f;

    [SerializeField] private EventManagerSO eventManager;
    private Dictionary<StatType, Stat> stats;
    public event Action OnDeath;
    private bool isDead;
    public event Action<bool> OnTiredStateChanged;

    [SerializeField]
    [Range(0f, 1f)]
    private float tiredThreshold = 0.2f;

    private bool isTired;
    public bool IsDead => isDead;
    public bool IsTired => isTired;

    private void Awake()
    {
        stats = new Dictionary<StatType, Stat>
        {
            { StatType.Health, new Stat(maxHealth) },
            { StatType.Hunger, new Stat(maxHunger) },
            { StatType.Thirst, new Stat(maxThirst) },
            { StatType.Stamina, new Stat(maxStamina) },
            { StatType.Temperature, new Stat(maxTemperature) }
        };
    }

    private void Start()
    {
        NotifyAllStats();
    }

    public float GetCurrent(StatType stat)
    {
        return stats[stat].Current;
    }

    public float GetMax(StatType stat)
    {
        return stats[stat].Max;
    }

    public float GetPercent(StatType stat)
    {
        return stats[stat].Percent;
    }

    public bool IsEmpty(StatType stat)
    {
        return stats[stat].Current <= 0f;
    }

    public void ModifyStat(StatType stat, float amount)
    {
        if (!stats.TryGetValue(stat, out Stat statData))
            return;

        float previousValue = statData.Current;
        statData.Modify(amount);

        if (Mathf.Approximately(previousValue, statData.Current))
            return;

        eventManager?.NotifyPlayerStatChanged(stat,statData.Current, statData.Max);
        UpdateTiredState();
        if (stat == StatType.Health && statData.Current <= 0f && !isDead)
        {
            isDead = true;
            OnDeath?.Invoke();
            eventManager?.NotifyPlayerDeath();
        }
    }

    private void NotifyAllStats()
    {
        foreach (var stat in stats)
        {
            eventManager?.NotifyPlayerStatChanged(stat.Key,stat.Value.Current,stat.Value.Max);
        }
    }

    public bool IsLow(StatType stat, float threshold = 0.2f)
    {
        return GetPercent(stat) <= threshold;
    }

    public bool IsFull(StatType stat)
    {
        return GetPercent(stat) >= 1f;
    }

    public void SetStat(StatType stat, float value)
    {
        if (!stats.TryGetValue(stat, out Stat statData))
            return;

        statData.SetCurrent(value);
        eventManager?.NotifyPlayerStatChanged(stat,statData.Current,statData.Max);
    }
    private void UpdateTiredState()
    {
        bool tired = IsEmpty(StatType.Stamina) ||IsLow(StatType.Stamina, tiredThreshold) || IsLow(StatType.Hunger, tiredThreshold) ||IsLow(StatType.Thirst, tiredThreshold);
        if (tired == isTired)
            return;
        isTired = tired;
        OnTiredStateChanged?.Invoke(isTired);
    }
}