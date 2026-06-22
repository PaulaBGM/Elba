using TMPro;
using UnityEngine;

public class UIActionSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text actionText;
    [SerializeField] private TMP_Text keyText;

    public void Initialize(
        string action,
        string key)
    {
        actionText.text = action;
        keyText.text = key;
    }
}