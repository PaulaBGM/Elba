using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIFloatingStorageMenu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private Button primaryButton;
    [SerializeField] private Button cancelButton;

    [Header("Texts")]
    [SerializeField] private TMP_Text primaryText;
    [SerializeField] private TMP_Text cancelText;

    private void Awake()
    {
        Hide();
    }

    public void Show(Vector3 position, string actionName, Action action)
    {
        transform.position = position;

        gameObject.SetActive(true);

        primaryText.text = actionName;
        cancelText.text = "Cancelar";

        primaryButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();

        primaryButton.onClick.AddListener(() =>
        {
            action?.Invoke();
            Hide();
        });

        cancelButton.onClick.AddListener(Hide);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}