using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class UIStatRow : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text value;

        public void SetData(Sprite statIcon, float statValue)
        {
            icon.sprite = statIcon;
            value.text = statValue.ToString();
        }
    }
}