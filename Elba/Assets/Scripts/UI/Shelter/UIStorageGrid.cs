using System;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.UI
{
    public class UIStorageGrid : MonoBehaviour
    {
        [Header("Grid")]
        [SerializeField] private UIInventoryItem itemPrefab;
        [SerializeField] private RectTransform contentPanel;

        private readonly List<UIInventoryItem> listOfUIItems = new();

        public event Action<int> OnItemClicked;

        public void InitializeInventoryUI(int size)
        {
            if (listOfUIItems.Count > 0)
                return;

            for (int i = 0; i < size; i++)
            {
                UIInventoryItem uiItem = Instantiate(itemPrefab, contentPanel);
                uiItem.transform.localScale = Vector3.one;

                listOfUIItems.Add(uiItem);

                uiItem.OnItemClicked += HandleItemSelection;
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
            if (index < 0 || index >= listOfUIItems.Count)
                return;

            listOfUIItems[index].SetData(sprite, quantity);
        }

        private void HandleItemSelection(UIInventoryItem item)
        {
            int index = listOfUIItems.IndexOf(item);

            if (index == -1)
                return;

            OnItemClicked?.Invoke(index);
        }

        public RectTransform GetItemTransform(int index)
        {
            if (index < 0 || index >= listOfUIItems.Count)
                return null;

            return listOfUIItems[index].transform as RectTransform;
        }
        public int VisualToRealIndex(int visualIndex, List<int> displayedSlots)
        {
            if (visualIndex < 0 || visualIndex >= displayedSlots.Count)
                return -1;

            return displayedSlots[visualIndex];
        }
    }
}