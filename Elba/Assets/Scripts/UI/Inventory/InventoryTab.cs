using UnityEngine;
using UnityEngine.UI;
using System;
using Inventory.Model;

namespace Inventory.UI
{
    public class InventoryTab : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private ItemCategory category;

        public event Action<ItemCategory> OnTabSelected;

        private void Awake()
        {
            button.onClick.AddListener(() =>
            {
                OnTabSelected?.Invoke(category);
            });
        }
    }
}