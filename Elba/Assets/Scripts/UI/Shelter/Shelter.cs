using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory;
using Inventory.Model;
public class Shelter : MonoBehaviour, IInteractable, IUpgradeableStructure
{
    [Header("Shelter")]
    [SerializeField] private Transform shelterPoint;
    [SerializeField] private ShelterStorage storage;

    [Header("Interaction")]
    [SerializeField] private Transform interactionAnchor;

    [Header("Upgrade")]
    [SerializeField] private StructureUpgradeSO nextUpgrade;

    private GameObject currentPlayer;
    private PlayerMovement currentMovement;
    private InventoryController currentInventory;

    public ShelterStorage Storage => storage;
    public Transform InteractionAnchor => interactionAnchor;

    public StructureUpgradeSO NextUpgrade => nextUpgrade;

    public void Interact(GameObject interactor)
    {
        currentInventory = interactor.GetComponent<InventoryController>();

        if (currentInventory == null)
            return;

        StructureUpgradeUI.Instance.Open(this, currentInventory);
    }

    public List<ActionData> GetActions()
    {
        return new()
        {
            new("Mejorar", "E")
        };
    }

    public void Enter(GameObject player)
    {
        currentPlayer = player;
        currentMovement = player.GetComponent<PlayerMovement>();

        if (currentMovement != null)
            currentMovement.SetMovementLocked(true);

        if (shelterPoint != null)
            player.transform.position = shelterPoint.position;

        ShelterUI.Instance.Open(this);
    }

    public IEnumerator SleepRoutine()
    {
        yield return UIFader.Instance.FadeOut();

        PlayerStatsSystem stats =
            currentPlayer.GetComponent<PlayerStatsSystem>();

        if (stats != null)
        {
            stats.SetStat(
                StatType.Health,
                stats.GetMax(StatType.Health));

            stats.SetStat(
                StatType.Stamina,
                stats.GetMax(StatType.Stamina));
        }

        yield return new WaitForSecondsRealtime(0.5f);

        yield return UIFader.Instance.FadeIn();
    }

    public void Sleep()
    {
        StartCoroutine(SleepRoutine());
    }

    public void ExitShelter()
    {
        if (currentMovement != null)
            currentMovement.SetMovementLocked(false);

        ShelterManager.Instance.ExitShelter();

        ShelterUI.Instance.Close();
    }

    #region Upgrade

    public bool CanUpgrade()
    {
        if (currentInventory == null || nextUpgrade == null)
            return false;

        foreach (RecipeIngredient ingredient in nextUpgrade.Requirements)
        {
            if (currentInventory.GetItemCount(ingredient.item) < ingredient.amount)
                return false;
        }

        return true;
    }

    public bool Upgrade()
    {
        if (!CanUpgrade())
            return false;

        currentInventory.ConsumeIngredients(nextUpgrade.Requirements);

        Instantiate(
            nextUpgrade.UpgradedPrefab,
            transform.position,
            transform.rotation,
            transform.parent);

        Destroy(gameObject);

        return true;
    }

    #endregion
}