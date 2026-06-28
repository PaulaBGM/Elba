using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIIngredientRow : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text itemName;
    [SerializeField] private TMP_Text amount;

    [Header("Colors")]
    [SerializeField] private Color enoughColor = Color.white;
    [SerializeField] private Color missingColor = Color.red;

    public void Setup(Sprite sprite, string name, int current, int required)
    {
        icon.sprite = sprite;
        itemName.text = name;

        amount.text = $"{current}/{required}";
        amount.color = current >= required? enoughColor: missingColor;
    }
}