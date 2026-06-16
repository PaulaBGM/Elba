using System.Collections;
using UnityEngine;

public abstract class ChargeAnimalBehaviour : MonoBehaviour, IAnimalBehaviour
{
    [Header("Damage")]
    [SerializeField] protected float damage;

    [Header("Charge")]
    [SerializeField] protected float chargeTime;
    [SerializeField] protected float chargeSpeed;
    [SerializeField] protected float chargeDuration;

    [Header("Impact")]
    [SerializeField] protected float hitRange;
    [SerializeField] protected float knockbackForce;

    [Header("Recovery")]
    [SerializeField] protected float stunDuration;

    [Header("Visual")]
    [SerializeField] protected SpriteRenderer spriteRenderer;

    protected bool busy;
    protected float hitRangeSqr;

    public bool IsBusy => busy;

    protected virtual void Awake()
    {
        hitRangeSqr = hitRange * hitRange;

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public virtual void Attack(EnemyController enemy)
    {
        if (busy || enemy.Target == null)
            return;

        StartCoroutine(ChargeRoutine(enemy));
    }

    protected virtual IEnumerator ChargeRoutine(EnemyController enemy)
    {
        busy = true;
        enemy.IsBusy = true;

        Vector2 chargeDirection =
            ((Vector2)enemy.Target.position -
            (Vector2)enemy.transform.position).normalized;

        SetPreparingColor();

        enemy.Rigidbody.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(chargeTime);

        if (enemy.Target == null)
        {
            SetNormalColor();

            busy = false;
            enemy.IsBusy = false;

            yield break;
        }

        SetNormalColor();

        float timer = chargeDuration;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;

            enemy.Rigidbody.linearVelocity = chargeDirection * chargeSpeed;

            if (enemy.Target != null)
            {
                Vector2 offset =  (Vector2)enemy.Target.position -   (Vector2)enemy.transform.position;

                if (offset.sqrMagnitude <= hitRangeSqr)
                {
                    PlayerMain player = enemy.Target.GetComponent<PlayerMain>();

                    if (player != null)
                    {
                        player.Damage(damage);

                        player.ApplyKnockback(chargeDirection,knockbackForce);

                        enemy.Rigidbody.linearVelocity = Vector2.zero;

                        break;
                    }
                }
            }

            yield return null;
        }

        enemy.Rigidbody.linearVelocity = Vector2.zero;
        SetStunnedColor();
        float stunTimer = stunDuration;

        while (stunTimer > 0f)
        {
            stunTimer -= Time.deltaTime;
            enemy.Rigidbody.linearVelocity = Vector2.zero;
            yield return null;
        }

        SetNormalColor();
        busy = false;
        enemy.IsBusy = false;
    }

    protected virtual void SetPreparingColor()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = Color.red;
    }

    protected virtual void SetStunnedColor()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = Color.yellow;
    }

    protected virtual void SetNormalColor()
    {
        if (spriteRenderer != null)
            spriteRenderer.color = Color.white;
    }
}