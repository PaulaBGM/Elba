using UnityEngine;

public class ShelterSystem : MonoBehaviour
{
    public static ShelterSystem Instance { get; private set; }

    public bool IsInsideShelter { get; private set; }

    private void Awake()
    {
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