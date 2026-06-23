using System.Collections.Generic;
using UnityEngine;

public class UIActionBar : MonoBehaviour
{
    public static UIActionBar Instance { get; private set; }

    [SerializeField] private UIActionSlot slotPrefab;
    [SerializeField] private Transform container;

    private readonly List<UIActionSlot> activeSlots = new();

    private void Awake()
    {
        Instance = this;
        Hide();
    }

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
            if (slot != null)
                Destroy(slot.gameObject);
        }

        activeSlots.Clear();
    }
}