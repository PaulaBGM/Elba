using UnityEngine;

public class ChaseState : States<EnemyController>
{
    [SerializeField] private float chaseSpeed = 4f;
    [SerializeField] private float attackRange = 1.25f;

    private float attackRangeSqr;

    public override void OnEnter()
    {
        Debug.Log("ENTER CHASE");
        attackRangeSqr = attackRange * attackRange;
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

        if (offset.sqrMagnitude <= attackRangeSqr)
        {
            _controller.SetState(_controller.AttackState);
            return;
        }

        _controller.Rigidbody.linearVelocity =
            offset.normalized * chaseSpeed;
    }

    public override void OnExit()
    {
        _controller.Rigidbody.linearVelocity = Vector2.zero;
    }
}