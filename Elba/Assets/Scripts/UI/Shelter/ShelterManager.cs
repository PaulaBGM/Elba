using UnityEngine;

public class ShelterManager : MonoBehaviour
{
    public static ShelterManager Instance { get; private set; }

    public bool IsInsideShelter { get; private set; }

    private Shelter currentShelter;
    private GameObject currentPlayer;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void EnterShelter(Shelter shelter, GameObject player)
    {
        if (IsInsideShelter)
            return;

        currentShelter = shelter;
        currentPlayer = player;

        IsInsideShelter = true;

        shelter.Enter(player);

        ShelterUI.Instance.Open(shelter);
    }

    public void ExitShelter()
    {
        if (!IsInsideShelter)
            return;

        IsInsideShelter = false;

        ShelterUI.Instance.Close();

        currentShelter = null;
        currentPlayer = null;
    }

    public Shelter CurrentShelter => currentShelter;

    public GameObject CurrentPlayer => currentPlayer;
}