using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EventManager", menuName = "Scriptable Objects/EventManagerSO")]
public class EventManagerSO : ScriptableObject
{
    public event Action<float, float> OnPlayerDamaged;
    public event Action<float, float> OnPlayerHealed;
    public event Action<float, float> OnPlayerThirstChanged;
    public event Action<GameObject, float, float> OnEnemyDamaged;
    public void PlayerNotifiesDamaged(float currentHealth, float maxHealth) => OnPlayerDamaged?.Invoke(currentHealth, maxHealth);

    public void PlayerNotifiesThirstChanged(float current, float max) => OnPlayerThirstChanged?.Invoke(current, max);

    public void InteractableNotifiesHealling(float currentHealth, float maxHealth) => OnPlayerHealed?.Invoke(currentHealth, maxHealth);

    public void EnemyNotifiesDamaged(GameObject enemy, float currentHealth, float maxHealth) => OnEnemyDamaged?.Invoke(enemy, currentHealth, maxHealth);
}

