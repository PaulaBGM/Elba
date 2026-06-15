using UnityEngine;

public class ShelterUI : MonoBehaviour
{
    public static ShelterUI Instance { get; private set; }

    [SerializeField] private GameObject root;

    private Shelter currentShelter;

    private void Awake()
    {
        Instance = this;

        if (root != null)
            root.SetActive(false);
    }

    public void Open(Shelter shelter)
    {
        currentShelter = shelter;

        root.SetActive(true);

        Time.timeScale = 0f;
    }

    public void Close()
    {
        currentShelter = null;

        root.SetActive(false);

        Time.timeScale = 1f;
    }

    public void Sleep()
    {
        currentShelter?.Sleep();
    }

    public void ExitShelter()
    {
        currentShelter?.ExitShelter();
    }
}