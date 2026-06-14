using System;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    [SerializeField] private PlayerStatsSystem playerStatsSystem;
    [SerializeField] private Rigidbody2D rb;

    public event Action OnPlayerDeath;

    private void Awake()
    {
        if (playerStatsSystem == null)
            playerStatsSystem = GetComponent<PlayerStatsSystem>();

        if (rb == null)
            rb = GetComponent<Rigidbody2D>();
    }

    public void Damage(float amount)
    {
        playerStatsSystem.ModifyStat(StatType.Health, -amount);

        if (playerStatsSystem.IsEmpty(StatType.Health))
            NotifyDeath();
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        if (rb == null)
            return;

        rb.AddForce(direction.normalized * force, ForceMode2D.Impulse);
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