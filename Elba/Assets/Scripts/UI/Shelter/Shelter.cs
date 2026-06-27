using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shelter : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform shelterPoint;
    [SerializeField] private ShelterStorage storage;
    public ShelterStorage Storage => storage;

    private GameObject currentPlayer;
    private PlayerMovement currentMovement;
    [SerializeField] private Transform interactionAnchor;

    public Transform InteractionAnchor => interactionAnchor;

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
    public List<ActionData> GetActions()
    {
        return new List<ActionData>()
    {
        new("Entrar", "E")
    };
    }
    public void Sleep()
    {
        StartCoroutine(SleepRoutine());
    }

    private IEnumerator SleepRoutine()
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

    public void ExitShelter()
    {
        if (currentMovement != null)
            currentMovement.SetMovementLocked(false);

        ShelterManager.Instance.ExitShelter();

        ShelterUI.Instance.Close();
    }


}