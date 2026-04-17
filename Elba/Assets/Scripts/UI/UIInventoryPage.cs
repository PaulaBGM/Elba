using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.UI
{
    public class UIInventoryPage : MonoBehaviour
    {
        [SerializeField] private UIInventoryItem itemPrefab;
        [SerializeField] private RectTransform contentPanel;
        [SerializeField] private UIInventoryDescription itemDescription;
        [SerializeField] private MouseFollower mouseFollower;

        [SerializeField] private ItemActionPanel actionPanel;

        private List<UIInventoryItem> listOfUIItems = new List<UIInventoryItem>();
        private int currentlyDraggedItemIndex = -1;

        public event Action<int> OnDescriptionRequested,
                                 OnItemActionRequested,
                                 OnStartDragging;

        public event Action<int, int> OnSwapItems;

        private void Awake()
        {
            Hide();
            mouseFollower.Toggle(false);
            itemDescription.ResetDescription();
        }

        public void InitializeInventoryUI(int inventorySize)
        {
            for (int i = 0; i < inventorySize; i++)
            {
                UIInventoryItem uiItem =
                    Instantiate(itemPrefab, Vector3.zero, Quaternion.identity);

                uiItem.transform.SetParent(contentPanel);
                uiItem.transform.localScale = Vector3.one;

                listOfUIItems.Add(uiItem);

                uiItem.OnItemClicked += HandleItemSelection;
                uiItem.OnItemBeginDrag += HandleBeginDrag;
                uiItem.OnItemDroppedOn += HandleSwap;
                uiItem.OnItemEndDrag += HandleEndDrag;
                uiItem.OnRightMouseBtnClick += HandleShowItemActions;
            }
        }

        public void ResetAllItems()
        {
            foreach (var item in listOfUIItems)
            {
                item.ResetData();
                item.Deselect();
            }
        }

        public void UpdateData(int itemIndex, Sprite sprite, int quantity)
        {
            if (itemIndex < listOfUIItems.Count)
                listOfUIItems[itemIndex].SetData(sprite, quantity);
        }

        public void UpdateDescription(int itemIndex, Sprite image, string name, string description)
        {
            itemDescription.SetDescription(image, name, description);
            DeselectAllItems();
            listOfUIItems[itemIndex].Select();
        }

        private void HandleItemSelection(UIInventoryItem item)
        {
            int index = listOfUIItems.IndexOf(item);
            if (index == -1) return;

            OnDescriptionRequested?.Invoke(index);
        }

        private void HandleShowItemActions(UIInventoryItem item)
        {
            int index = listOfUIItems.IndexOf(item);
            if (index == -1) return;

            OnItemActionRequested?.Invoke(index);
        }

        private void HandleBeginDrag(UIInventoryItem item)
        {
            int index = listOfUIItems.IndexOf(item);
            if (index == -1) return;

            currentlyDraggedItemIndex = index;
            OnStartDragging?.Invoke(index);
        }

        private void HandleEndDrag(UIInventoryItem item)
        {
            ResetDraggedItem();
        }

        private void HandleSwap(UIInventoryItem item)
        {
            int index = listOfUIItems.IndexOf(item);
            if (index == -1) return;

            OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
        }

        private void ResetDraggedItem()
        {
            mouseFollower.Toggle(false);
            currentlyDraggedItemIndex = -1;
        }

        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            mouseFollower.Toggle(true);
            mouseFollower.SetData(sprite, quantity);
        }

        public void AddAction(string name, Action action)
        {
            actionPanel.AddButon(name, action);
        }

        public void ShowItemAction(int index)
        {
            actionPanel.Toggle(true);
            actionPanel.transform.position = listOfUIItems[index].transform.position;
        }

        public void ResetSelection()
        {
            itemDescription.ResetDescription();
            DeselectAllItems();
        }

        private void DeselectAllItems()
        {
            foreach (var item in listOfUIItems)
                item.Deselect();

            actionPanel.Toggle(false);
        }

        public void Show()
        {
            gameObject.SetActive(true);
            ResetSelection();
        }

        public void Hide()
        {
            actionPanel.Toggle(false);
            gameObject.SetActive(false);
            ResetDraggedItem();
        }
    }
}