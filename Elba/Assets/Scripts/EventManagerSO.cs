using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EventManager", menuName = "Scriptable Objects/EventManagerSO")]
public class EventManagerSO : ScriptableObject
{
    public event Action<StatType, float, float> OnPlayerStatChanged;
    public event Action<GameObject, float, float> OnEnemyDamaged;
    public event Action OnPlayerDeath;

    public void NotifyPlayerStatChanged(
        StatType stat,
        float current,
        float max)
    {
        OnPlayerStatChanged?.Invoke(stat, current, max);
    }

    public void EnemyNotifiesDamaged(
        GameObject enemy,
        float currentHealth,
        float maxHealth)
    {
        OnEnemyDamaged?.Invoke(enemy, currentHealth, maxHealth);
    }

    public void NotifyPlayerDeath()
    {
        OnPlayerDeath?.Invoke();
    }
}