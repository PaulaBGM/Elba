using System;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    [SerializeField] private PlayerStatsSystem playerStatsSystem;

    public event Action OnPlayerDeath;

    private void Awake()
    {
        if (playerStatsSystem == null)
            playerStatsSystem = GetComponent<PlayerStatsSystem>();
    }

    public void Damage(float amount)
    {
        playerStatsSystem.ModifyStat(StatType.Health, -amount);

        if (playerStatsSystem.IsEmpty(StatType.Health))
            NotifyDeath();
    }

    public void Heal(float amount)
    {
        playerStatsSystem.ModifyStat(StatType.Health, amount);
    }

    public void Drink(float amount)
    {
        playerStatsSystem.ModifyStat(StatType.Thirst, amount);
    }

    public void Eat(float amount)
    {
        playerStatsSystem.ModifyStat(StatType.Hunger, amount);
    }

    public void RestoreStamina(float amount)
    {
        playerStatsSystem.ModifyStat(StatType.Stamina, amount);
    }

    public void NotifyDeath()
    {
        OnPlayerDeath?.Invoke();
    }
}