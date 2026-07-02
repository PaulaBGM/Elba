using UnityEngine;
using System;
using System.Collections;

public enum AttackType
{
    Animal,
    Tree
}

public class PlayerAttackSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader input;
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private AgentWeapon weapon;
    [SerializeField] private Transform attackPoint;

    [Header("Attack")]
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private LayerMask attackLayer;
    [SerializeField] private float attackDuration = 0.3f;

    public event Action<AttackType> OnAttackStarted;

    private bool isAttacking;

    public bool IsAttacking => isAttacking;

    private void Awake()
    {
        if (input == null)
            input = FindFirstObjectByType<InputReader>();

        if (movement == null)
            movement = GetComponent<PlayerMovement>();

        if (weapon == null)
            weapon = GetComponent<AgentWeapon>();
    }

    private void OnEnable()
    {
        if (input != null)
            input.OnAttack += Attack;
    }

    private void OnDisable()
    {
        if (input != null)
            input.OnAttack -= Attack;
    }

    private void Attack()
    {
        if (isAttacking)
            return;

        if (UIManager.Instance != null && UIManager.Instance.IsInventoryOpen)
            return;

        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;

        AttackType attackType = PerformAttack();

        OnAttackStarted?.Invoke(attackType);

        yield return new WaitForSeconds(attackDuration);

        isAttacking = false;
    }

    private AttackType PerformAttack()
    {
        Vector2 direction = movement.LastDirection.normalized;

        Vector3 origin = attackPoint != null
            ? attackPoint.position
            : transform.position;

        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            direction,
            attackDistance,
            attackLayer);

        Debug.DrawRay(origin, direction * attackDistance, Color.red, 0.25f);

        if (!hit.collider)
            return AttackType.Animal;

        // ========= RECURSOS =========

        ResourceNode resource = hit.collider.GetComponentInParent<ResourceNode>();

        if (resource == null)
            resource = hit.collider.GetComponentInChildren<ResourceNode>();

        if (resource != null)
        {
            resource.ReceiveHit(gameObject, weapon.GetResourceDamage());

            weapon.ConsumeDurability();

            return AttackType.Tree;
        }

        // ========= ENEMIGOS =========

        IAttackable attackable = hit.collider.GetComponentInParent<IAttackable>();

        if (attackable == null)
            attackable = hit.collider.GetComponentInChildren<IAttackable>();

        if (attackable != null)
        {
            attackable.ReceiveHit(gameObject, weapon.GetAnimalDamage());

            weapon.ConsumeDurability();

            return AttackType.Animal;
        }

        return AttackType.Animal;
    }

    private void OnDrawGizmosSelected()
    {
        if (movement == null)
            movement = GetComponent<PlayerMovement>();

        if (movement == null)
            return;

        Vector3 origin = attackPoint != null
            ? attackPoint.position
            : transform.position;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(origin,
            origin + (Vector3)(movement.LastDirection.normalized * attackDistance));
    }
}