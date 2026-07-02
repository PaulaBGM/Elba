using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class ShelterTrigger : MonoBehaviour
{
    [SerializeField] private Shelter shelter;

    private void Awake()
    {
        if (shelter == null)
            shelter = GetComponentInParent<Shelter>();

        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (ShelterManager.Instance.IsInsideShelter)
            return;

        ShelterManager.Instance.EnterShelter(
            shelter,
            other.gameObject);
    }
}