using Inventory.Model;
using System.Collections;
using UnityEngine;
using Inventory;
using System.Collections.Generic;

public class Item : MonoBehaviour, IInteractable
{
    [field: SerializeField]
    public ItemSO InventoryItem { get; private set; }

    [field: SerializeField]
    public int Quantity { get; set; } = 1;

    [Header("Interaction")]
    [SerializeField] private Transform interactionAnchor;

    [Header("Feedback")]
    [SerializeField] private float duration = 0.3f;

    public Transform InteractionAnchor => interactionAnchor;

    private void Start()
    {
        SpriteRenderer spriteRenderer =
            GetComponentInChildren<SpriteRenderer>();

        if (spriteRenderer != null)
            spriteRenderer.sprite = InventoryItem.ItemImage;
    }

    public void Interact(GameObject interactor)
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

    public void DestroyItem()
    {
        Collider2D col = GetComponent<Collider2D>();

        if (col != null)
            col.enabled = false;

        StartCoroutine(AnimateItemPickup());
    }
    public List<ActionData> GetActions()
    {
        return new List<ActionData>()
    {
        new("Recoger", "E")
    };
    }
    private IEnumerator AnimateItemPickup()
    {
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;

        float currentTime = 0f;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;

            transform.localScale =
                Vector3.Lerp(
                    startScale,
                    endScale,
                    currentTime / duration);

            yield return null;
        }

        Destroy(gameObject);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (interactionAnchor == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            interactionAnchor.position,
            0.1f);
    }
#endif
}