using UnityEngine;

public class Shelter : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform shelterPoint;

    private GameObject currentPlayer;
    private PlayerMovement currentMovement;

    public void Interact(GameObject interactor)
    {
        currentPlayer = interactor;

        currentMovement =
            interactor.GetComponent<PlayerMovement>();

        ShelterManager.Instance.EnterShelter();

        if (currentMovement != null)
            currentMovement.SetMovementLocked(true);

        if (shelterPoint != null)
            interactor.transform.position =
                shelterPoint.position;

        ShelterUI.Instance.Open(this);
    }

    public void Sleep()
    {
        if (currentPlayer == null)
            return;

        PlayerStatsSystem stats =
            currentPlayer.GetComponent<PlayerStatsSystem>();

        if (stats == null)
            return;

        stats.SetStat(
            StatType.Health,
            stats.GetMax(StatType.Health));

        stats.SetStat(
            StatType.Stamina,
            stats.GetMax(StatType.Stamina));
    }

    public void ExitShelter()
    {
        if (currentMovement != null)
            currentMovement.SetMovementLocked(false);

        ShelterManager.Instance.ExitShelter();

        ShelterUI.Instance.Close();
    }
}