using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

namespace Inventory.UI
{
    public class UIInventoryItem : MonoBehaviour,IPointerClickHandler, IBeginDragHandler, IEndDragHandler,IDropHandler,IDragHandler
    {
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text quantityTxt;
        [SerializeField] private Image borderImage;

        public event Action<UIInventoryItem> OnItemClicked;
        public event Action<UIInventoryItem> OnItemDroppedOn;
        public event Action<UIInventoryItem> OnRightMouseBtnClick;
        public event Action<UIInventoryItem> OnItemBeginDrag;
        public event Action<UIInventoryItem> OnItemEndDrag;

        public bool empty = true;

        private CanvasGroup cg;

        private void Awake()
        {
            cg = GetComponent<CanvasGroup>();

            ResetData();
            Deselect();
        }
     
        public void ResetData()
        {
            itemImage.gameObject.SetActive(false);
            quantityTxt.text = string.Empty;
            empty = true;
        }

        public void Deselect()
        {
            borderImage.enabled = false;
        }

        public void Select()
        {
            borderImage.enabled = true;
        }

        public void SetData(Sprite sprite, int quantity)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = sprite;
            quantityTxt.text = quantity.ToString();

            empty = false;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (empty)
                return;
            if (eventData.button == PointerEventData.InputButton.Right)
            {
                OnRightMouseBtnClick?.Invoke(this);
            }
            else if (eventData.button == PointerEventData.InputButton.Left)
            {
                OnItemClicked?.Invoke(this);
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (empty)
                return;

            OnItemBeginDrag?.Invoke(this);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            OnItemEndDrag?.Invoke(this);
        }

        public void OnDrop(PointerEventData eventData)
        {
            OnItemDroppedOn?.Invoke(this);
        }

        public void OnDrag(PointerEventData eventData)
        {
        }
    }
}