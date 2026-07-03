using UnityEngine;

public class ShelterUI : MonoBehaviour
{
    public static ShelterUI Instance { get; private set; }

    [SerializeField] private GameObject root;
    [SerializeField] private CraftingUI craftingUI;

    private Shelter currentShelter;

    public Shelter CurrentShelter => currentShelter;

    private void Awake()
    {
        Instance = this;

        if (root != null)
            root.SetActive(false);
    }

    public void Open(Shelter shelter)
    {
        currentShelter = shelter;

        if (root != null)
            root.SetActive(true);

        if (UIManager.Instance != null)
            UIManager.Instance.IsExternalMenuOpen = true;
    }

    public void OpenCrafting()
    {
        if (root != null)
            root.SetActive(false);

        if (craftingUI != null)
            craftingUI.Open(this);
    }

    public void Reopen()
    {
        if (root != null)
            root.SetActive(true);

        if (UIManager.Instance != null)
            UIManager.Instance.IsExternalMenuOpen = true;
    }

    public void Close()
    {
        currentShelter = null;

        if (UIManager.Instance != null)
            UIManager.Instance.IsExternalMenuOpen = false;

        if (root != null)
            root.SetActive(false);
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