using System;
using UnityEngine;
using UnityEngine.UI;

namespace Inventory.UI
{
    public class ItemActionPanel : MonoBehaviour
    {
        [SerializeField] private GameObject buttonPrefab;
        [SerializeField] private InputReader inputReader;

        private void OnEnable()
        {
            if (inputReader != null)
                inputReader.OnUISelect += HandleSelect;
        }

        private void OnDisable()
        {
            if (inputReader != null)
                inputReader.OnUISelect -= HandleSelect;
        }

        private void HandleSelect()
        {
            if (!gameObject.activeSelf)
                return;

            bool insidePanel =
                RectTransformUtility.RectangleContainsScreenPoint(
                    (RectTransform)transform,
                    inputReader.MousePosition);

            if (!insidePanel)
                Toggle(false);
        }

        public void AddButon(string name, Action onClickAction)
        {
            GameObject button = Instantiate(buttonPrefab, transform);

            Button btn = button.GetComponent<Button>();

            btn.onClick.RemoveAllListeners();

            btn.onClick.AddListener(() =>
            {
                onClickAction?.Invoke();
                Toggle(false);
            });

            button.GetComponentInChildren<TMPro.TMP_Text>().text = name;
        }

        public void Toggle(bool val)
        {
            if (val)
                RemoveOldButtons();

            gameObject.SetActive(val);
        }

        private void RemoveOldButtons()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
    }
}