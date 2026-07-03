using System;
using System.Collections;
using UnityEngine;

public abstract class ChargeAnimalBehaviour : MonoBehaviour, IAnimalBehaviour
{
    [Header("Damage")]
    [SerializeField] protected float damage = 10f;

    [Header("Charge")]
    [SerializeField] protected float chargeTime = 0.25f;
    [SerializeField] protected float chargeSpeed = 8f;
    [SerializeField] protected float chargeDuration = 1.2f;

    [Header("Impact")]
    [SerializeField] protected float hitRange = 0.6f;
    [SerializeField] protected float knockbackForce = 8f;

    [Header("Recovery")]
    [SerializeField] protected float stunDuration = 1.5f;

    [Header("Animation")]
    [SerializeField] private float impactEventTimeout = 1f;

    [Header("Visual")]
    [SerializeField] protected SpriteRenderer spriteRenderer;

    protected bool busy;
    protected float hitRangeSqr;

    public bool IsBusy => busy;

    public event Action OnChargeStarted;
    public event Action<bool> OnStunnedStateChanged;

    private Transform currentTarget;
    private Vector2 chargeDirection;
    private bool impactResolved;

    protected virtual void Awake()
    {
        hitRangeSqr = hitRange * hitRange;

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public virtual void Attack(EnemyController enemy)
    {
        if (busy || enemy == null || enemy.Target == null)
            return;

        StartCoroutine(ChargeRoutine(enemy));
    }

    protected virtual IEnumerator ChargeRoutine(EnemyController enemy)
    {
        busy = true;
        enemy.IsBusy = true;

        currentTarget = enemy.Target;
        impactResolved = false;

        if (currentTarget == null)
        {
            EndRoutine(enemy);
            yield break;
        }

        chargeDirection =
            ((Vector2)currentTarget.position - (Vector2)enemy.transform.position).normalized;

        SetPreparingColor();
        enemy.Rigidbody.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(chargeTime);

        if (currentTarget == null)
        {
            EndRoutine(enemy);
            yield break;
        }

        SetNormalColor();

        float timer = chargeDuration;
        bool reachedAttackPoint = false;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;

            if (currentTarget == null)
                break;

            Vector2 offset =
                (Vector2)currentTarget.position - (Vector2)enemy.transform.position;

            if (offset.sqrMagnitude <= hitRangeSqr)
            {
                reachedAttackPoint = true;
                break;
            }

            enemy.Rigidbody.linearVelocity = chargeDirection * chargeSpeed;
            yield return null;
        }

        enemy.Rigidbody.linearVelocity = Vector2.zero;

        if (!reachedAttackPoint)
        {
            EndRoutine(enemy);
            yield break;
        }

        OnChargeStarted?.Invoke();

        float wait = 0f;
        while (!impactResolved && wait < impactEventTimeout)
        {
            wait += Time.deltaTime;
            yield return null;
        }

        if (!impactResolved)
            ResolveImpact();

        yield return StartCoroutine(StunRoutine(enemy));

        EndRoutine(enemy);
    }

    /// <summary>
    /// Llamar desde un Animation Event en el frame exacto del impacto.
    /// </summary>
    public void DealDamage()
    {
        ResolveImpact();
    }

    protected virtual void ResolveImpact()
    {
        if (impactResolved)
            return;

        impactResolved = true;

        if (currentTarget == null)
            return;

        PlayerMain player = currentTarget.GetComponent<PlayerMain>();
        if (player == null)
            return;

        player.Damage(damage);
        player.ApplyKnockback(chargeDirection, knockbackForce);
    }

    protected virtual IEnumerator StunRoutine(EnemyController enemy)
    {
        OnStunnedStateChanged?.Invoke(true);

        float stunTimer = stunDuration;
        while (stunTimer > 0f)
        {
            stunTimer -= Time.deltaTime;
            enemy.Rigidbody.linearVelocity = Vector2.zero;
            yield return null;
        }

        OnStunnedStateChanged?.Invoke(false);
    }

    protected virtual void EndRoutine(EnemyController enemy)
    {
        SetNormalColor();
        enemy.Rigidbody.linearVelocity = Vector2.zero;

        currentTarget = null;
        impactResolved = false;

        busy = false;
        enemy.IsBusy = false;
    }

    protected virtual void SetPreparingColor()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = Color.white;
    }

    protected virtual void SetNormalColor()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = Color.white;
    }
}