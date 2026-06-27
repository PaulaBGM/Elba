using UnityEngine;

public class ShelterManager : MonoBehaviour
{
    public static ShelterManager Instance { get; private set; }

    public bool IsInsideShelter { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void EnterShelter()
    {
        IsInsideShelter = true;
    }

    public void ExitShelter()
    {
        IsInsideShelter = false;
    }
}