using System.Collections;
using UnityEngine;

public class PlayerAttackSystem : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputReader input;
    [SerializeField] private PlayerMovement movement;
    [SerializeField] private Transform attackPoint;

    [Header("Attack")]
    [SerializeField] private float attackDistance = 1.5f;
    [SerializeField] private LayerMask attackLayer;
    [SerializeField] private float attackDuration = 0.3f;

    private bool isAttacking;

    public bool IsAttacking => isAttacking;

    private void Awake()
    {
        if (input == null)
            input = FindFirstObjectByType<InputReader>();

        if (movement == null)
            movement = GetComponent<PlayerMovement>();
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

        if (UIManager.Instance != null &&
            UIManager.Instance.IsInventoryOpen)
            return;

        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;

        PerformAttack();

        yield return new WaitForSeconds(attackDuration);

        isAttacking = false;
    }

    private void PerformAttack()
    {
        Vector2 direction = movement.LastDirection.normalized;

        Vector3 origin =
            attackPoint != null
            ? attackPoint.position
            : transform.position;

        RaycastHit2D hit =
            Physics2D.Raycast(
                origin,
                direction,
                attackDistance,
                attackLayer);

        Debug.DrawRay(
            origin,
            direction * attackDistance,
            Color.red,
            0.25f);

        if (!hit.collider)
            return;

        IAttackable attackable =
            hit.collider.GetComponentInParent<IAttackable>();

        if (attackable == null)
            attackable =
                hit.collider.GetComponentInChildren<IAttackable>();

        if (attackable == null)
            return;

        attackable.ReceiveHit(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        if (movement == null)
            movement = GetComponent<PlayerMovement>();

        if (movement == null)
            return;

        Vector3 origin =
            attackPoint != null
            ? attackPoint.position
            : transform.position;

        Gizmos.color = Color.red;

        Gizmos.DrawLine(origin,  origin + (Vector3)(movement.LastDirection.normalized * attackDistance));
    }
}