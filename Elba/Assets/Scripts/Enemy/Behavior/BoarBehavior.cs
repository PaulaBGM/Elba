using System.Collections;
using UnityEngine;

public class BoarBehaviour : MonoBehaviour, IAnimalBehaviour
{
    [SerializeField] private float damage = 25f;

    [Header("Charge")]
    [SerializeField] private float chargeTime = 0.5f;
    [SerializeField] private float chargeSpeed = 12f;
    [SerializeField] private float chargeDuration = 0.4f;

    [Header("Impact")]
    [SerializeField] private float hitRange = 1.2f;
    [SerializeField] private float knockbackForce = 8f;

    [Header("Recovery")]
    [SerializeField] private float stunDuration = 2f;

    [SerializeField] private SpriteRenderer spriteRenderer;

    private bool busy;
    private float hitRangeSqr;

    public bool IsBusy => busy;

    private void Awake()
    {
        hitRangeSqr = hitRange * hitRange;

        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void Attack(EnemyController enemy)
    {
        if (busy || enemy.Target == null)
            return;

        StartCoroutine(ChargeRoutine(enemy));
    }

    private IEnumerator ChargeRoutine(EnemyController enemy)
    {
        busy = true;

        Vector2 chargeDirection =
            ((Vector2)enemy.Target.position -
             (Vector2)enemy.transform.position).normalized;

        if (spriteRenderer != null)
            spriteRenderer.color = Color.red;

        enemy.Rigidbody.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(chargeTime);

        if (spriteRenderer != null)
            spriteRenderer.color = Color.white;

        float timer = chargeDuration;
        bool hitPlayer = false;

        while (timer > 0f)
        {
            timer -= Time.deltaTime;

            enemy.Rigidbody.linearVelocity =
                chargeDirection * chargeSpeed;

            if (!hitPlayer && enemy.Target != null)
            {
                Vector2 offset =
                    (Vector2)enemy.Target.position -
                    (Vector2)enemy.transform.position;

                if (offset.sqrMagnitude <= hitRangeSqr)
                {
                    PlayerMain player =
                        enemy.Target.GetComponent<PlayerMain>();

                    if (player != null)
                    {
                        player.Damage(damage);
                        player.ApplyKnockback(chargeDirection, knockbackForce);

                        hitPlayer = true;
                    }
                }
            }

            yield return null;
        }

        enemy.Rigidbody.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(stunDuration);

        busy = false;
    }
}