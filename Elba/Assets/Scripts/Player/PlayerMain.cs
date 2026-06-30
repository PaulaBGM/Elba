using System;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    [SerializeField] private PlayerStatsSystem playerStatsSystem;
    [SerializeField] private PlayerMovement playerMovement;

    public event Action OnPlayerDeath;

    private void Awake()
    {
        if (playerStatsSystem == null)
            playerStatsSystem = GetComponent<PlayerStatsSystem>();

        if (playerMovement == null)
            playerMovement = GetComponent<PlayerMovement>();
       
        playerStatsSystem.OnDeath += HandleDeath;
    }

    public void Damage(float amount)
    {
        if (ShelterManager.Instance != null && ShelterManager.Instance.IsInsideShelter)
        {
            return;
        }

        playerStatsSystem.ModifyStat(StatType.Health,-amount);

        if (playerStatsSystem.IsEmpty(StatType.Health))
            HandleDeath();
    }

    public void ApplyKnockback(Vector2 direction, float force)
    {
        if (playerMovement == null)
            return;

        playerMovement.ApplyKnockback(direction, force);
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

    private void HandleDeath()
    {
        OnPlayerDeath?.Invoke();
        GameOverController.Instance?.Show();
    }
}