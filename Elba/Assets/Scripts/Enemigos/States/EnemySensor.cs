using System;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class EnemySensor : MonoBehaviour
{
    [SerializeField] private float sensorDistance = 9f;
    [SerializeField] private float sensorAngle = 60f;
    [SerializeField] private LayerMask whatIsObstacle;

    public event Action<Transform> OnPlayerDetected;
    public event Action<Transform> OnPlayerLost;

    private CircleCollider2D sensorTrigger;
    private Transform currentTarget;

    private void Awake()
    {
        sensorTrigger = GetComponent<CircleCollider2D>();
        sensorTrigger.isTrigger = true;
        sensorTrigger.radius = sensorDistance;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        CheckDetection(other);
    }

    private void CheckDetection(Collider2D other)
    {
        Vector2 directionToTarget = (other.transform.position - transform.position);
        float distanceToTarget = directionToTarget.magnitude;

        directionToTarget.Normalize();

        RaycastHit2D hit = Physics2D.Raycast(
            transform.position,
            directionToTarget,
            distanceToTarget,
            whatIsObstacle);

        if (hit.collider != null)
        {
            LoseTarget(other.transform);
            return;
        }

        float angleToTarget = Vector2.Angle(transform.right, directionToTarget);

        if (angleToTarget <= sensorAngle * 0.5f)
        {
            if (currentTarget != other.transform)
            {
                currentTarget = other.transform;
                Debug.Log("Player visto!!");
                OnPlayerDetected?.Invoke(currentTarget);
            }
        }
        else
        {
            LoseTarget(other.transform);
        }
    }

    private void LoseTarget(Transform target)
    {
        if (currentTarget != target) return;

        currentTarget = null;
        OnPlayerLost?.Invoke(target);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        LoseTarget(other.transform);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sensorDistance);

        Vector3 left =Quaternion.Euler(0, 0, sensorAngle * 0.5f) * transform.right;

        Vector3 right = Quaternion.Euler(0, 0, -sensorAngle * 0.5f) * transform.right;

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + left * sensorDistance);
        Gizmos.DrawLine(transform.position, transform.position + right * sensorDistance);
    }
#endif
}