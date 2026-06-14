using System;
using UnityEngine;

public class EnemySensor : MonoBehaviour
{
    public event Action<Transform> OnPlayerDetected;
    public event Action OnPlayerLost;

    public Transform CurrentTarget { get; private set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        CurrentTarget = other.transform;

        OnPlayerDetected?.Invoke(CurrentTarget);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        CurrentTarget = null;

        OnPlayerLost?.Invoke();
    }
}