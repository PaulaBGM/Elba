using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIStatBar : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private TMP_Text valueText;

    public void UpdateBar(float current, float max)
    {
        fillImage.fillAmount = current / max;

        if (valueText != null)
            valueText.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
    }
}