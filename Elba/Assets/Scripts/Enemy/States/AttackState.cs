using UnityEngine;

public class AttackState : States<EnemyController>
{
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float attackRange = 1.25f;

    private float attackRangeSqr;
    private float cooldownTimer;

    public override void OnEnter()
    {
        attackRangeSqr = attackRange * attackRange;
        cooldownTimer = 0f;

        _controller.Rigidbody.linearVelocity = Vector2.zero;
    }

    public override void OnUpdate()
    {
        if (_controller.Target == null)
        {
            _controller.SetState(_controller.PatrolState);
            return;
        }

        Vector2 offset =
            (Vector2)_controller.Target.position -
            (Vector2)transform.position;

        if (offset.sqrMagnitude > attackRangeSqr)
        {
            _controller.SetState(_controller.ChaseState);
            return;
        }

        cooldownTimer -= Time.deltaTime;

        if (cooldownTimer > 0f)
            return;

        cooldownTimer = attackCooldown;

        _controller.AnimalBehaviour.Attack(_controller);
    }

    public override void OnExit()
    {
    }
}