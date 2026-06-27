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

        private List<UIInventoryItem> listOfUIItems = new();
        private int currentlyDraggedItemIndex = -1;

        public event Action<int> OnDescriptionRequested,
                                 OnItemActionRequested,
                                 OnStartDragging;

        public event Action<int, int> OnSwapItems;

        public event Action<ItemCategory> OnTabChanged;

        private void Awake()
        {
            mouseFollower.Toggle(false);
            itemDescription.ResetDescription();

            foreach (var tab in tabs)
                tab.OnTabSelected += HandleTabChanged;
        }

        private void HandleTabChanged(ItemCategory category)
        {
            OnTabChanged?.Invoke(category);
        }

        public void InitializeInventoryUI(int size)
        {
            for (int i = 0; i < size; i++)
            {
                UIInventoryItem uiItem = Instantiate(itemPrefab, contentPanel);
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

        public void UpdateData(int index, Sprite sprite, int quantity)
        {
            if (index < listOfUIItems.Count)
            {
                listOfUIItems[index].SetData(sprite, quantity);
            }
        }

        private void HandleItemSelection(UIInventoryItem item)
        {
            Debug.Log($"[UIInventoryPage] CLICK RECIBIDO EN: {item.name}");

            int index = listOfUIItems.IndexOf(item);

            Debug.Log($"[UIInventoryPage] INDEX EN LISTA = {index}");

            if (index == -1)
            {
                Debug.LogError("[UIInventoryPage] EL ITEM NO EXISTE EN listOfUIItems");
                return;
            }

            Debug.Log($"[UIInventoryPage] Lanzando OnDescriptionRequested({index})");

            OnDescriptionRequested?.Invoke(index);
        }

        private void HandleShowItemActions(UIInventoryItem item)
        {
            int index = listOfUIItems.IndexOf(item);

            if (index != -1)
                OnItemActionRequested?.Invoke(index);
        }
        
        public void HideItemActionPanel()
        {
            actionPanel.Toggle(false);
        }

        private void HandleBeginDrag(UIInventoryItem item)
        {
            Debug.Log($"[UIInventoryPage] BeginDrag {item.name}");

            int index = listOfUIItems.IndexOf(item);

            Debug.Log($"[UIInventoryPage] INDEX = {index}");

            if (index == -1)
            {
                Debug.LogError("[UIInventoryPage] Drag sobre item no registrado");
                return;
            }

            currentlyDraggedItemIndex = index;

            Debug.Log($"[UIInventoryPage] OnStartDragging({index})");

            OnStartDragging?.Invoke(index);
        }

        private void HandleEndDrag(UIInventoryItem item)
        {
            Debug.Log($"[UIInventoryPage] EndDrag {item.name}");

            mouseFollower.Toggle(false);
            currentlyDraggedItemIndex = -1;
        }

        private void HandleSwap(UIInventoryItem item)
        {
            Debug.Log($"[UIInventoryPage] Drop sobre {item.name}");

            int index = listOfUIItems.IndexOf(item);

            Debug.Log($"[UIInventoryPage] FROM = {currentlyDraggedItemIndex}");
            Debug.Log($"[UIInventoryPage] TO = {index}");

            if (index == -1)
            {
                Debug.LogError("[UIInventoryPage] Destino de swap no registrado");
                return;
            }

            OnSwapItems?.Invoke(currentlyDraggedItemIndex, index);
        }

        public void CreateDraggedItem(Sprite sprite, int quantity)
        {
            Debug.Log($"[UIInventoryPage] CreateDraggedItem Qty:{quantity}");

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
            actionPanel.transform.position =
                listOfUIItems[index].transform.position;
        }

        public void UpdateDescription(int index, Sprite img, string name, string desc)
        {
            itemDescription.SetDescription(img, name, desc);
        }

        public void ResetSelection()
        {
            itemDescription.ResetDescription();
        }
    }
}