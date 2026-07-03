using UnityEngine;

public class AttackState : States<EnemyController>
{
    [SerializeField] private float attackRange = 1.25f;

    private float attackRangeSqr;

    public override void OnEnter()
    {
        attackRangeSqr = attackRange * attackRange;

        _controller.Rigidbody.linearVelocity = Vector2.zero;

        _controller.Sensor.OnPlayerLost += LoseTarget;
    }

    public override void OnUpdate()
    {
        if (_controller.IsBusy)
        {
            _controller.Rigidbody.linearVelocity = Vector2.zero;
            return;
        }

        if (_controller.Target == null)
        {
            _controller.SetState(_controller.PatrolState);
            return;
        }

        Vector2 offset = (Vector2)_controller.Target.position - (Vector2)transform.position;

        if (offset.sqrMagnitude > attackRangeSqr)
        {
            _controller.SetState(_controller.ChaseState);
            return;
        }

        _controller.Rigidbody.linearVelocity = Vector2.zero;

        if (_controller.AnimalBehaviour.IsBusy)
            return;

        _controller.AnimalBehaviour.Attack(_controller);
    }

    private void LoseTarget(Transform target)
    {
        if (_controller.Target != target)
            return;

        _controller.Target = null;
        _controller.SetState(_controller.PatrolState);
    }

    public override void OnExit()
    {
        _controller.Sensor.OnPlayerLost -= LoseTarget;
    }

    private void OnDisable()
    {
        if (_controller?.Sensor == null)
            return;

        _controller.Sensor.OnPlayerLost -= LoseTarget;
    }
}