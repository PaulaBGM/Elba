using UnityEngine;
using Inventory;
using Inventory.Model;

public class ResourceNode : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemSO itemReward;
    [SerializeField] private int amount = 1;
    [SerializeField] private int durability = 3;

    public void Interact(GameObject interactor)
    {
        durability--;

        if (durability > 0)
            return;

        InventoryController inventory =
            interactor.GetComponent<InventoryController>();

        if (inventory == null)
            return;

        inventory.InventoryData.AddItem(itemReward, amount);

        Destroy(gameObject);
    }
}