using System.Collections.Generic;
using UnityEngine;

public class UIActionBar : MonoBehaviour
{
    [SerializeField] private UIActionSlot slotPrefab;
    [SerializeField] private Transform container;

    private readonly List<UIActionSlot> activeSlots = new();

    public void ShowActions(List<ActionData> actions)
    {
        Clear();

        foreach (ActionData action in actions)
        {
            UIActionSlot slot =
                Instantiate(slotPrefab, container);

            slot.Initialize(
                action.actionName,
                action.keyText);

            activeSlots.Add(slot);
        }

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        Clear();
        gameObject.SetActive(false);
    }

    private void Clear()
    {
        foreach (UIActionSlot slot in activeSlots)
        {
            Destroy(slot.gameObject);
        }

        activeSlots.Clear();
    }
}