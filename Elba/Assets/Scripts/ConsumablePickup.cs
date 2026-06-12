using Inventory.Model;
using UnityEngine;

public class ConsumablePickup : MonoBehaviour, IConsumablePickup
{
    [SerializeField]
    private EdibleItemSO edibleItem;

    public void Consume(GameObject consumer)
    {
        edibleItem.PerformAction(consumer);

        Destroy(gameObject);
    }
}