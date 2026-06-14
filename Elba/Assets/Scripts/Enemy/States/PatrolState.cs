using System.Collections.Generic;
using UnityEngine;

public class PatrolState : States<EnemyController>
{
    [SerializeField] private Transform patrolPath;
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float arrivalDistance = 0.2f;
    [SerializeField] private float waitTime = 1f;

    private readonly List<Vector2> patrolPoints = new();

    private int currentPoint;
    private float waitTimer;
    private float arrivalDistanceSqr;

    public override void InitController(EnemyController controller)
    {
        base.InitController(controller);

        patrolPoints.Clear();

        if (patrolPath != null)
        {
            foreach (Transform point in patrolPath)
            {
                patrolPoints.Add(point.position);
            }
        }

        arrivalDistanceSqr = arrivalDistance * arrivalDistance;

        controller.Sensor.OnPlayerDetected -= StartChase;
        controller.Sensor.OnPlayerDetected += StartChase;
    }

    private void OnDestroy()
    {
        if (_controller != null && _controller.Sensor != null)
            _controller.Sensor.OnPlayerDetected -= StartChase;
    }

    public override void OnEnter()
    {
        waitTimer = 0f;
        _controller.Rigidbody.linearVelocity = Vector2.zero;
    }

    public override void OnUpdate()
    {
        if (patrolPoints.Count == 0)
            return;

        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = patrolPoints[currentPoint];

        Vector2 offset = targetPosition - currentPosition;

        if (offset.sqrMagnitude <= arrivalDistanceSqr)
        {
            _controller.Rigidbody.linearVelocity = Vector2.zero;

            waitTimer += Time.deltaTime;

            if (waitTimer < waitTime)
                return;

            waitTimer = 0f;

            currentPoint++;

            if (currentPoint >= patrolPoints.Count)
                currentPoint = 0;

            return;
        }

        _controller.Rigidbody.linearVelocity =
            offset.normalized * patrolSpeed;
    }

    public override void OnExit()
    {
        _controller.Rigidbody.linearVelocity = Vector2.zero;
    }

    private void StartChase(Transform player)
    {
        if (_controller.Target == player)
            return;

        Debug.Log("PATROL -> CHASE");

        _controller.Target = player;
        _controller.SetState(_controller.ChaseState);
    }
}