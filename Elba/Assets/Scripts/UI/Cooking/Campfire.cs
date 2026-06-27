using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform interactionAnchor;
    [SerializeField] private float cookTime = 2f;

    private Coroutine cookingRoutine;

    private static readonly List<ActionData> emptyActions =
        new();

    public Transform InteractionAnchor =>
        interactionAnchor;

    public List<ActionData> GetActions()
    {
        return emptyActions;
    }

    public void Interact(GameObject interactor)
    {
        PlayerInteractionSystem player =
            interactor.GetComponent<PlayerInteractionSystem>();

        if (player == null)
            return;

        if (!player.HasHeldItem)
            return;

        Item heldItem = player.HeldItem;

        if (!heldItem.CanCook())
            return;

        if (cookingRoutine != null)
            return;

        cookingRoutine =
            StartCoroutine(
                CookRoutine(player));
    }

    private IEnumerator CookRoutine(
        PlayerInteractionSystem player)
    {
        CookingProgressUI.Instance.Show();

        float timer = 0f;

        while (timer < cookTime)
        {
            timer += Time.deltaTime;

            CookingProgressUI.Instance.SetProgress(
                timer / cookTime);

            yield return null;
        }

        if (player.HeldItem != null)
        {
            player.HeldItem.Cook();

            player.RefreshHeldItemVisual();
        }

        CookingProgressUI.Instance.Hide();

        cookingRoutine = null;
    }
}