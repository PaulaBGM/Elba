using System;
using System.Collections.Generic;
using UnityEngine;
using Inventory.Model;

namespace Inventory.UI
{
    public class UIInventoryPage : MonoBehaviour
    {
        [SerializeField] private UIInventoryItem itemPrefab;
        [SerializeField] private RectTransform contentPanel;
        [SerializeField] private UIInventoryDescription itemDescription;
        [SerializeField] private MouseFollower mouseFollower;
        [SerializeField] private ItemActionPanel actionPanel;
        [SerializeField] private List<InventoryTab> tabs;

        private readonly List<UIInventoryItem> listOfUIItems = new();

        private int currentlyDraggedItemIndex = -1;

        public event Action<int> OnDescriptionRequested;
        public event Action<int> OnStartDragging;
        public event Action<int, int> OnSwapItems;
        public event Action<ItemCategory> OnTabChanged;

        private void Awake()
        {
            if (mouseFollower != null)
                mouseFollower.Toggle(false);

            itemDescription?.ResetDescription();

            foreach (var tab in tabs)
            {
                if (tab != null)
                    tab.OnTabSelected += HandleTabChanged;
            }
        }

        private void HandleTabChanged(ItemCategory category)
        {
            OnTabChanged?.Invoke(category);
        }

        public void InitializeInventoryUI(int size)
        {
            // Evita inicializar la UI dos veces
            if (listOfUIItems.Count > 0)
                return;

            for (int i = 0; i < size; i++)
            {
                UIInventoryItem uiItem = Instantiate(itemPrefab, contentPanel);
                uiItem.transform.localScale = Vector3.one;

                listOfUIItems.Add(uiItem);

                uiItem.OnItemClicked += HandleItemSelection;
                uiItem.OnItemBeginDrag += HandleBeginDrag;
                uiItem.OnItemDroppedOn += HandleSwap;
                uiItem.OnItemEndDrag += HandleEndDrag;
                //uiItem.OnRightMouseBtnClick += HandleShowItemActions;
            }
        }

        public void ClearActions()
        {
            actionPanel?.Clear();
        }

        public void ResetAllItems()
        {
            for (int i = listOfUIItems.Count - 1; i >= 0; i--)
            {
                if (listOfUIItems[i] == null)
                {
                    listOfUIItems.RemoveAt(i);
                    continue;
                }

                listOfUIItems[i].ResetData();
                listOfUIItems[i].Deselect();
            }
        }

        public void UpdateData(int index, Sprite sprite, int quantity)
        {
            if (index < 0 || index >= listOfUIItems.Count)
                return;

            if (listOfUIItems[index] == null)
                return;

            listOfUIItems[index].SetData(sprite, quantity);
        }

        private void HandleItemSelection(UIInventoryItem item)
        {
            if (item == null)
                return;

            int index = listOfUIItems.IndexOf(item);

            if (index == -1)
                return;

            OnDescriptionRequested?.Invoke(index);
        }

        public void HideItemActionPanel()
        {
            actionPanel?.Clear();
        }

        private void HandleBeginDrag(UIInventoryItem item)
        {
            if (item == null)
                return;

            int index = listOfUIItems.IndexOf(item);

            if (index == -1)
                return;

            currentlyDraggedItemIndex = index;

            OnStartDragging?.Invoke(index);
        }

        private void HandleEndDrag(UIInventoryItem item)
        {
            if (mouseFollower != null)
                mouseFollower.Toggle(false);

            currentlyDraggedItemIndex = -1;
        }

        private void HandleSwap(UIInventoryItem item)
        {
            if (item == null)
                return;

            int index = listOfUIItems.IndexOf(item);

            if (index == -1)
                return;

            OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
        }

        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            if (mouseFollower == null)
                return;

            mouseFollower.Toggle(true);
            mouseFollower.SetData(sprite, quantity);
        }

        public void UpdateDescription(int index, Sprite img, string name, string desc)
        {
            itemDescription?.SetDescription(img, name, desc);
        }

        public void AddStat(Sprite icon, float value)
        {
            itemDescription?.AddStat(icon, value);
        }

        public void ShowActions(
            string leftName,
            Action leftAction,
            string rightName,
            Action rightAction)
        {
            actionPanel?.Show(leftName, leftAction, rightName, rightAction);
        }

        public void ClearStats()
        {
            itemDescription?.ClearStats();
        }

        public void ResetUI()
        {
            actionPanel?.Clear();

            if (mouseFollower != null)
                mouseFollower.Toggle(false);

            ResetSelection();
        }

        public void ResetSelection()
        {
            itemDescription?.ResetDescription();
        }
    }
}