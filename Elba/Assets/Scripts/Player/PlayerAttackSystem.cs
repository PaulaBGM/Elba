using UnityEngine;

public class PlayerAttackSystem : MonoBehaviour
{
    [SerializeField] private InputReader input;

    [SerializeField] private float attackRange = 1.5f;

    [SerializeField] private LayerMask attackLayer;

    private void OnEnable()
    {
        input.OnAttack += Attack;
    }

    private void OnDisable()
    {
        input.OnAttack -= Attack;
    }

    private void Attack()
    {
        Collider2D[] hits =
            Physics2D.OverlapCircleAll(
                transform.position,
                attackRange,
                attackLayer);

        foreach (Collider2D hit in hits)
        {
            IAttackable attackable =
                hit.GetComponentInParent<IAttackable>();

            if (attackable == null)
                continue;

            attackable.ReceiveHit(gameObject);
        }
    }
}