using System;
using UnityEngine;

public class PlayerMain : MonoBehaviour
{
    private PlayerHealthSystem playerHealthSystem;
    private PlayerThirstSystem playerThirstSystem;

    public event Action<float> OnHealed;

    public event Action<float> OnDehydrated;
    public event Action<float> OnHydrated;

    public event Action OnPlayerDeath; // Evento central de muerte

    private void Awake()
    {
        playerHealthSystem = GetComponentInChildren<PlayerHealthSystem>();
        playerThirstSystem = GetComponentInChildren<PlayerThirstSystem>();
    }

    private void OnEnable()
    {
        OnHealed += playerHealthSystem.Heal;
        OnDehydrated += playerHealthSystem.ApplyDamage;
        OnHydrated += playerThirstSystem.Drink;
    }

    private void OnDisable()
    {

        if (playerHealthSystem != null)
        {
            OnHealed -= playerHealthSystem.Heal;
            OnDehydrated -= playerHealthSystem.ApplyDamage;
        }

        if (playerThirstSystem != null)
        {
            OnHydrated -= playerThirstSystem.Drink;
        }
    }

    public void NotifiesDehydrated(float amount) => OnDehydrated?.Invoke(amount);
    public void NotifiesHydrated(float amount) => OnHydrated?.Invoke(amount);
    public void NotifyHealed(float amount) => OnHealed?.Invoke(amount);

    public void NotifyDeath()
    {
       // OnPlayerDeath?.Invoke();
        //UIManager.Instance.ShowMissionFailed();
    }
}