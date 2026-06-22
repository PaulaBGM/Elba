using Inventory.Model;
using System.Collections;
using UnityEngine;
using Inventory;

public class Item : MonoBehaviour, IInteractable
{
    [field: SerializeField]
    public ItemSO InventoryItem { get; private set; }

    [field: SerializeField]
    public int Quantity { get; set; } = 1;
    
    //public bool IsEdible =>InventoryItem is Inventory.Model.EdibleItemSO;
    
    [SerializeField] private float duration = 0.3f;
    [SerializeField] private Transform interactionAnchor;

    public Transform InteractionAnchor => interactionAnchor;

    private void Start()
    {
        GetComponentInChildren<SpriteRenderer>().sprite =
            InventoryItem.ItemImage;
    }

    public void Interact(GameObject interactor)
    {
        InventoryController inventory =  interactor.GetComponent<InventoryController>();

        if (inventory == null)
            return;

        int remainder = inventory.InventoryData.AddItem(InventoryItem, Quantity);

        if (remainder == 0)
            DestroyItem();
        else
            Quantity = remainder;
    }

    /*public void InteractOrConsume(GameObject interactor)
    {
        if (InventoryItem is EdibleItemSO)
        {
            InteractionPromptUI.Instance.Show(this, interactor);
            return;
        }

        Interact(interactor);
    }*/

    public void DestroyItem()
    {
        GetComponent<Collider2D>().enabled = false;
        StartCoroutine(AnimateItemPickup());
    }

    private IEnumerator AnimateItemPickup()
    {

        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;

        float currentTime = 0f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            transform.localScale = Vector3.Lerp(startScale,endScale, currentTime / duration);

            yield return null;
        }

        Destroy(gameObject);
    }
    /*public void Store(GameObject interactor)
    {
        InventoryController inventory =
            interactor.GetComponent<InventoryController>();

        if (inventory == null)
            return;

        int remainder =
            inventory.InventoryData.AddItem(
                InventoryItem,
                Quantity);

        if (remainder == 0)
        {
            DestroyItem();
        }
        else
        {
            Quantity = remainder;
        }
    }

    public void Consume(GameObject interactor)
    {
        if (InventoryItem is not EdibleItemSO edible)
            return;

        edible.PerformAction(interactor);

        DestroyItem();
    }*/
}