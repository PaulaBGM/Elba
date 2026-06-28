using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Inventory.Model;

namespace Inventory.UI
{
    public class UIInventoryDescription : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private Image itemImage;
        [SerializeField] private TMP_Text title;
        [SerializeField] private TMP_Text description;

        [Header("Stats")]
        [SerializeField] private Transform statsParent;
        [SerializeField] private UIStatRow statRowPrefab;

        private readonly List<UIStatRow> statRows = new();

        private void Awake()
        {
            ResetDescription();
        }

        public virtual void ResetDescription()
        {
            itemImage.gameObject.SetActive(false);

            title.text = string.Empty;
            description.text = string.Empty;

            ClearStats();
        }
        public void ShowItem(ItemSO item, Sprite damageIcon)
        {
            ResetDescription();
            SetDescription(item.ItemImage,item.Name,item.Description);
            ClearStats();
            if (item is EdibleItemSO edible)
            {
                foreach (var modifier in edible.ModifiersData)
                {
                    AddStat(modifier.statModifier.Icon,modifier.value);
                }
            }
            else if (item is ToolItemSO tool)
            {
                AddStat(damageIcon,tool.AnimalDamage);
            }
        }
        public void SetDescription(Sprite sprite,string itemName,string itemDescription)
        {
            itemImage.gameObject.SetActive(true);
            itemImage.sprite = sprite;
            title.text = itemName;
            description.text = itemDescription;
            ClearStats();
        }

        public void AddStat(Sprite icon, float value)
        {
            UIStatRow row = Instantiate(statRowPrefab, statsParent);
            row.SetData(icon, value);
            statRows.Add(row);
        }

        public void ClearStats()
        {
            foreach (UIStatRow row in statRows)
            {
                if (row != null)
                    Destroy(row.gameObject);
            }

            statRows.Clear();
        }
    }
}