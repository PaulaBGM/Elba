using TMPro;
using UnityEngine;

public class InteractionPromptUI : MonoBehaviour
{
    [SerializeField] private GameObject root;
    [SerializeField] private TMP_Text promptText;

    private void Start()
    {
        Hide();
    }

    public void Show(string text)
    {
        root.SetActive(true);
        promptText.text = $"[E] {text}";
    }

    public void Hide()
    {
        root.SetActive(false);
    }
}