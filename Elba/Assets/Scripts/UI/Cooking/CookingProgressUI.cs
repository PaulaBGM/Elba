using UnityEngine;
using UnityEngine.UI;

public class CookingProgressUI : MonoBehaviour
{
    public static CookingProgressUI Instance { get; private set; }

    [SerializeField] private GameObject root;
    [SerializeField] private Image fillImage;

    private void Awake()
    {
        Instance = this;

        Hide();
    }

    public void Show()
    {
        root.SetActive(true);
        SetProgress(0f);
    }

    public void Hide()
    {
        root.SetActive(false);
    }

    public void SetProgress(float value)
    {
        fillImage.fillAmount = value;
    }
}