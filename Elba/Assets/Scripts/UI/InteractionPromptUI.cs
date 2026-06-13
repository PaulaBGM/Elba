using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Inventory.Model;
using Inventory;

public class InteractionPromptUI : MonoBehaviour
{
    public static InteractionPromptUI Instance { get; private set; }
    public bool IsOpen => root.activeSelf;
    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text itemName;

    [SerializeField] private Button saveButton;
    [SerializeField] private Button consumeButton;

    private Item currentItem;
    private GameObject currentPlayer;

    private void Awake()
    {
        Instance = this;

        root.SetActive(false);
    }

    public void Show(Item item, GameObject player)
    {
        currentItem = item;
        currentPlayer = player;

        itemName.text = item.InventoryItem.Name;

        saveButton.gameObject.SetActive(true);
        consumeButton.gameObject.SetActive(true);

        saveButton.onClick.RemoveAllListeners();
        consumeButton.onClick.RemoveAllListeners();

        saveButton.onClick.AddListener(SaveItem);
        consumeButton.onClick.AddListener(ConsumeItem);

        root.SetActive(true);
    }

    public void Hide()
    {
        root.SetActive(false);

        currentItem = null;
        currentPlayer = null;
    }

    private void SaveItem()
    {
        if (currentItem == null)
            return;

        InventoryController inventory =
            currentPlayer.GetComponent<InventoryController>();

        int remainder =
            inventory.InventoryData.AddItem(
                currentItem.InventoryItem,
                currentItem.Quantity);

        if (remainder == 0)
            currentItem.DestroyItem();

        Hide();
    }

    public void Show(string text)
    {
        root.SetActive(true);

        itemName.text = text;

        saveButton.gameObject.SetActive(false);
        consumeButton.gameObject.SetActive(false);
    }

    private void ConsumeItem()
    {
        if (currentItem == null)
            return;

        if (currentItem.InventoryItem is not EdibleItemSO edible)
            return;

        edible.PerformAction(currentPlayer);

        currentItem.DestroyItem();

        Hide();
    }
}