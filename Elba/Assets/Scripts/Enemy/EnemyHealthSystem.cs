using System;
using System.Collections;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour, IAttackable
{
    [SerializeField] private float maxHealth = 100f;

    [Header("Feedback")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float flashDuration = 0.1f;

    public event Action OnEnemyDied;

    private float currentHealth;
    private Color originalColor;
    private bool dead;

    private void Awake()
    {
        currentHealth = maxHealth;

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
            originalColor = spriteRenderer.color;
    }

    public void ReceiveHit(GameObject attacker)
    {
        TakeDamage(10f);
    }

    public void TakeDamage(float damage)
    {
        if (dead)
            return;

        currentHealth -= damage;

        Debug.Log($"{name} recibe {damage} de daño. Vida restante: {currentHealth}");

        if (spriteRenderer != null)
            StartCoroutine(HitFeedback());

        if (currentHealth > 0f)
            return;

        dead = true;
        OnEnemyDied?.Invoke();
    }

    private IEnumerator HitFeedback()
    {
        spriteRenderer.color = Color.red;

        yield return new WaitForSeconds(flashDuration);

        spriteRenderer.color = originalColor;
    }
}