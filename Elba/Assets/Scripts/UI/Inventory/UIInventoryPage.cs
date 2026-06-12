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

        [SerializeField] private List<InventoryTab> tabs;

        private List<UIInventoryItem> listOfUIItems = new();
        private int currentlyDraggedItemIndex = -1;

        public event Action<int> OnDescriptionRequested,
                                 OnItemActionRequested,
                                 OnStartDragging;

        public event Action<int, int> OnSwapItems;

        public event Action<Inventory.Model.ItemType> OnTabChanged;

        private void Awake()
        {
            Debug.Log("[UIInventoryPage] Awake");

            Hide();
            mouseFollower.Toggle(false);
            itemDescription.ResetDescription();

            foreach (var tab in tabs)
            {
                tab.OnTabSelected += HandleTabChanged;
            }
        }

        private void HandleTabChanged(Inventory.Model.ItemType type)
        {
            Debug.Log($"[UIInventoryPage] Cambio de pestaña: {type}");

            OnTabChanged?.Invoke(type);
        }

        public void InitializeInventoryUI(int size)
        {
            Debug.Log($"[UIInventoryPage] Inicializando {size} slots");

            for (int i = 0; i < size; i++)
            {
                UIInventoryItem uiItem = Instantiate(itemPrefab, contentPanel);
                uiItem.transform.localScale = Vector3.one;

                Debug.Log($"[UIInventoryPage] Slot creado -> {uiItem.name} ({i})");

                listOfUIItems.Add(uiItem);

                uiItem.OnItemClicked += HandleItemSelection;
                uiItem.OnItemBeginDrag += HandleBeginDrag;
                uiItem.OnItemDroppedOn += HandleSwap;
                uiItem.OnItemEndDrag += HandleEndDrag;
                uiItem.OnRightMouseBtnClick += HandleShowItemActions;

                Debug.Log($"[UIInventoryPage] Eventos conectados al slot {i}");
            }

            Debug.Log($"[UIInventoryPage] Total slots registrados: {listOfUIItems.Count}");
        }

        public void ResetAllItems()
        {
            Debug.Log("[UIInventoryPage] ResetAllItems");

            foreach (var item in listOfUIItems)
            {
                item.ResetData();
                item.Deselect();
            }
        }

        public void UpdateData(int index, Sprite sprite, int quantity)
        {
            Debug.Log($"[UIInventoryPage] UpdateData -> Index:{index} Qty:{quantity}");

            if (index < listOfUIItems.Count)
            {
                listOfUIItems[index].SetData(sprite, quantity);
            }
            else
            {
                Debug.LogWarning($"[UIInventoryPage] Index fuera de rango: {index}");
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
            Debug.Log($"[UIInventoryPage] Click derecho en {item.name}");

            int index = listOfUIItems.IndexOf(item);

            Debug.Log($"[UIInventoryPage] INDEX = {index}");

            if (index != -1)
                OnItemActionRequested?.Invoke(index);
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
            Debug.Log($"[UIInventoryPage] Añadiendo acción: {name}");

            actionPanel.AddButon(name, action);
        }

        public void ShowItemAction(int index)
        {
            Debug.Log($"[UIInventoryPage] Mostrar acciones para index {index}");

            actionPanel.Toggle(true);
            actionPanel.transform.position =
                listOfUIItems[index].transform.position;
        }

        public void UpdateDescription(int index, Sprite img, string name, string desc)
        {
            Debug.Log($"[UIInventoryPage] UpdateDescription -> {name}");

            itemDescription.SetDescription(img, name, desc);
        }

        public void Show()
        {
            Debug.Log("[UIInventoryPage] SHOW");

            gameObject.SetActive(true);
            ResetSelection();

            Debug.Log($"[UIInventoryPage] Slots registrados: {listOfUIItems.Count}");
        }

        public void Hide()
        {
            Debug.Log("[UIInventoryPage] HIDE");

            actionPanel.Toggle(false);
            gameObject.SetActive(false);
        }

        public void ResetSelection()
        {
            Debug.Log("[UIInventoryPage] ResetSelection");

            itemDescription.ResetDescription();
        }
    }
}