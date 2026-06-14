using UnityEngine;

public class PatrolState : States<EnemyController>
{
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float waitTime = 1f;

    private int currentPoint;
    private float waitTimer;

    private const float ArrivalDistanceSqr = 0.05f;

    public override void InitController(EnemyController controller)
    {
        base.InitController(controller);

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
    }

    public override void OnUpdate()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return;

        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = patrolPoints[currentPoint].position;

        Vector2 offset = targetPosition - currentPosition;

        if (offset.sqrMagnitude <= ArrivalDistanceSqr)
        {
            _controller.Rigidbody.linearVelocity = Vector2.zero;

            waitTimer += Time.deltaTime;

            if (waitTimer < waitTime)
                return;

            waitTimer = 0f;

            currentPoint++;

            if (currentPoint >= patrolPoints.Length)
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
        Debug.Log("PATROL -> CHASE");

        _controller.Target = player;

        _controller.SetState(_controller.ChaseState);
    }
}