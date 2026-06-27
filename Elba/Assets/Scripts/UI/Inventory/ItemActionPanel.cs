using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class ItemActionPanel : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;

        [Header("Texts")]
        [SerializeField] private TMP_Text leftText;
        [SerializeField] private TMP_Text rightText;

        private void Awake()
        {
            Clear();
        }

        public void Show(string leftName, Action leftAction,string rightName, Action rightAction)
        {
            gameObject.SetActive(true);
            ConfigureButton(leftButton, leftText, leftName, leftAction);
            ConfigureButton(rightButton, rightText, rightName, rightAction);
        }

        public void Clear()
        {
            gameObject.SetActive(true);
            ConfigureButton(leftButton, leftText, "", null);
            ConfigureButton(rightButton, rightText, "", null);
        }

        private void ConfigureButton(Button button,  TMP_Text text, string label,Action action)
        {
            text.text = label;
            button.onClick.RemoveAllListeners();
            if (action == null)
            {
                button.interactable = false;
                return;
            }
            button.interactable = true;
            button.onClick.AddListener(() => action.Invoke());
        }
    }
}