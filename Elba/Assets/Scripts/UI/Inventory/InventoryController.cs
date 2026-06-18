using Inventory.Model;
using Inventory.UI;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Inventory
{
    public class InventoryController : MonoBehaviour
    {
        [SerializeField] private UIInventoryPage inventoryUI;
        [SerializeField] private InventorySO inventoryData;
        [SerializeField] private InputReader input;
        [SerializeField] private AudioClip dropClip;
        [SerializeField] private AudioSource audioSource;
        private List<int> displayedSlots = new();
        private ItemCategory currentTab = ItemCategory.Food;

        public List<InventoryItem> initialItems = new();

        public InventorySO InventoryData => inventoryData;

        public event Action<InventoryItem> OnItemUsed;
        public event Action<InventoryItem> OnItemDropped;
        public event Action OnInventoryOpened;
        public event Action OnInventoryClosed;

        private void OnEnable()
        {
            if (input != null)
                input.OnInventory += ToggleInventory;
        }

        private void OnDisable()
        {
            if (input != null)
                input.OnInventory -= ToggleInventory;
        }
       
        private void Start()
        {
            if (input == null)
                input = FindFirstObjectByType<InputReader>();
            
            currentTab = ItemCategory.Food;
            UpdateInventoryUIFiltered();
            PrepareUI();
            PrepareInventoryData();
        }

        private void PrepareInventoryData()
        {
            inventoryData.Initialize();
            inventoryData.OnInventoryUpdated += UpdateInventoryUI;

            foreach (InventoryItem item in initialItems)
            {
                if (!item.IsEmpty)
                    inventoryData.AddItem(item);
            }
        }

        private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
        {
            UpdateInventoryUIFiltered();
        }

        private void PrepareUI()
        {
            inventoryUI.InitializeInventoryUI(inventoryData.Size);

            inventoryUI.OnDescriptionRequested += HandleDescriptionRequest;
            inventoryUI.OnSwapItems += HandleSwapItems;
            inventoryUI.OnStartDragging += HandleDragging;
            inventoryUI.OnItemActionRequested += HandleItemActionRequest;
            inventoryUI.OnTabChanged += HandleTabChanged;
        }

        private void HandleTabChanged(ItemCategory category)
        {
            currentTab = category;
            UpdateInventoryUIFiltered();
        }

        private void UpdateInventoryUIFiltered()
        {
            inventoryUI.ResetAllItems();

            displayedSlots.Clear();

            Dictionary<int, InventoryItem> inventory =
                inventoryData.GetCurrentInventoryState();

            int visualIndex = 0;

            foreach (var pair in inventory)
            {
                if (pair.Value.IsEmpty)
                    continue;

                if ((pair.Value.item.Categories & currentTab) == 0)
                    continue;

                displayedSlots.Add(pair.Key);

                inventoryUI.UpdateData(visualIndex,pair.Value.item.ItemImage, pair.Value.quantity);

                visualIndex++;
            }
        }

        private void HandleItemActionRequest(int visualIndex)
        {
            int realSlot = GetRealSlot(visualIndex);

            if (realSlot < 0)
                return;

            InventoryItem inventoryItem =
                inventoryData.GetItemAt(realSlot);

            if (inventoryItem.IsEmpty)
                return;

            inventoryUI.ShowItemAction(visualIndex);

            if (inventoryItem.item is IItemAction action)
            {
                inventoryUI.AddAction(
                    action.ActionName,
                    () => PerformAction(realSlot));
            }

            if (inventoryItem.item is IDestroyableItem)
            {
                inventoryUI.AddAction(
                    "Drop",
                    () => DropItem(realSlot, inventoryItem.quantity));
            }
        }

        private void DropItem(int itemIndex, int quantity)
        {
            InventoryItem inventoryItem =
                inventoryData.GetItemAt(itemIndex);

            SpawnDroppedItem(inventoryItem);

            OnItemDropped?.Invoke(inventoryItem);

            inventoryData.RemoveItem(itemIndex, quantity);

            inventoryUI.HideItemActionPanel();
            inventoryUI.ResetSelection();

            if (audioSource != null && dropClip != null)
                audioSource.PlayOneShot(dropClip);
        }

        public void PerformAction(int itemIndex)
        {
            InventoryItem item = inventoryData.GetItemAt(itemIndex);

            if (item.IsEmpty)
                return;

            if (item.item is IItemAction action)
            {
                bool success = action.PerformAction(
                    gameObject,
                    item.itemState);

                if (success)
                {
                    OnItemUsed?.Invoke(item);

                    if (item.item is IDestroyableItem)
                    {
                        inventoryData.RemoveItem(itemIndex, 1);
                    }

                    inventoryUI.HideItemActionPanel();

                    if (audioSource != null && action.actionSFX != null)
                        audioSource.PlayOneShot(action.actionSFX);

                    if (inventoryData.GetItemAt(itemIndex).IsEmpty)
                        inventoryUI.ResetSelection();
                }
            }
        }

        private void HandleDragging(int visualIndex)
        {
            int realSlot = GetRealSlot(visualIndex);

            if (realSlot < 0)
                return;

            InventoryItem item =
                inventoryData.GetItemAt(realSlot);

            if (item.IsEmpty)
                return;

            inventoryUI.CreateDraggedItem(
                item.item.ItemImage,
                item.quantity);
        }

        private void HandleSwapItems(int visualA, int visualB)
        {
            int realA = GetRealSlot(visualA);
            int realB = GetRealSlot(visualB);

            if (realA < 0 || realB < 0)
                return;

            inventoryData.SwapItems(realA, realB);
        }

        private int GetRealSlot(int visualIndex)
        {
            if (visualIndex < 0 || visualIndex >= displayedSlots.Count)
                return -1;

            return displayedSlots[visualIndex];
        }

        private void HandleDescriptionRequest(int visualIndex)
        {
            int realSlot = GetRealSlot(visualIndex);

            if (realSlot < 0)
                return;

            InventoryItem item = inventoryData.GetItemAt(realSlot);

            if (item.IsEmpty)
            {
                inventoryUI.ResetSelection();
                return;
            }

            inventoryUI.UpdateDescription(visualIndex, item.item.ItemImage,item.item.Name,PrepareDescription(item));
        }

        private string PrepareDescription(InventoryItem item)
        {
            StringBuilder sb = new();

            sb.AppendLine(item.item.Description);

            for (int i = 0; i < item.itemState.Count; i++)
            {
                sb.AppendLine(
                    $"{item.itemState[i].itemParameter.ParameterName}: " +
                    $"{item.itemState[i].value} / {item.item.DefaultParametersList[i].value}");
            }

            return sb.ToString();
        }

        private void ToggleInventory()
        {
            if (!inventoryUI.gameObject.activeSelf)
            {
                inventoryUI.Show();
                UpdateInventoryUIFiltered();

                OnInventoryOpened?.Invoke();
            }
            else
            {
                inventoryUI.Hide();

                OnInventoryClosed?.Invoke();
            }
        }

        public int AddItem(ItemSO item, int quantity)
        {
            return inventoryData.AddItem(item, quantity);
        }

        public bool ContainsItem(ItemSO item)
        {
            foreach (InventoryItem inventoryItem in inventoryData.GetCurrentInventoryState().Values)
            {
                if (inventoryItem.item == item)
                    return true;
            }

            return false;
        }

        public int GetItemCount(ItemSO item)
        {
            int count = 0;

            foreach (InventoryItem inventoryItem in inventoryData.GetCurrentInventoryState().Values)
            {
                if (inventoryItem.item == item)
                    count += inventoryItem.quantity;
            }

            return count;
        }

        public bool HasItems(List<RecipeIngredient> ingredients)
        {
            foreach (RecipeIngredient ingredient in ingredients)
            {
                if (GetItemCount(ingredient.item) < ingredient.amount)
                    return false;
            }

            return true;
        }
       
        private void SpawnDroppedItem(InventoryItem inventoryItem)
        {
            if (inventoryItem.item == null)
                return;

            if (inventoryItem.item.WorldPrefab == null)
            {
                Debug.LogWarning(
                    $"El item {inventoryItem.item.Name} no tiene WorldPrefab asignado");
                return;
            }

            Vector3 spawnPosition =
                transform.position +
                transform.right * 1.5f;

            GameObject droppedObject =
                Instantiate(
                    inventoryItem.item.WorldPrefab,
                    spawnPosition,
                    Quaternion.identity);

            Item worldItem =
                droppedObject.GetComponent<Item>();

            if (worldItem != null)
            {
                worldItem.Quantity = 1;
            }
        }
        
        public void ConsumeIngredients(List<RecipeIngredient> ingredients)
        {
            foreach (RecipeIngredient ingredient in ingredients)
            {
                int remaining = ingredient.amount;

                Dictionary<int, InventoryItem> inventoryState =
                    inventoryData.GetCurrentInventoryState();

                foreach (var slot in inventoryState)
                {
                    if (remaining <= 0)
                        break;

                    if (slot.Value.item != ingredient.item)
                        continue;

                    int amountToRemove =
                        Mathf.Min(
                            remaining,
                            slot.Value.quantity);

                    inventoryData.RemoveItem(
                        slot.Key,
                        amountToRemove);

                    remaining -= amountToRemove;
                }
            }
        }
        
        public Dictionary<int, InventoryItem> GetInventory()
        {
            return inventoryData.GetCurrentInventoryState();
        }
        
       
    }
   
    }