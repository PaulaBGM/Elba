using UnityEngine;

public class ShelterUI : MonoBehaviour
{
    public static ShelterUI Instance { get; private set; }

    [SerializeField] private GameObject root;
    [SerializeField] private GameObject shelterButtonsPanel;
    [SerializeField] private CraftingUI craftingUI;

    private Shelter currentShelter;

    public Shelter CurrentShelter => currentShelter;

    private void Awake()
    {
        Instance = this;

        if (root != null)
            root.SetActive(false);
    }

    public void SetShelterButtonsVisible(bool visible)
    {
        if (shelterButtonsPanel != null)
            shelterButtonsPanel.SetActive(visible);
    }

    public void Open(Shelter shelter)
    {
        TutorialPopup.Instance.Show("ShelterTutorial","REFUGIO",
        "Aquí podrás:\n\n" + "• Guardar objetos en el almacén.\n" + "• Fabricar herramientas.\n" + "• Dormir para pasar la noche.\n" + "• Salir del refugio cuando quieras.");
        currentShelter = shelter;

        if (root != null)
            root.SetActive(true);

        SetShelterButtonsVisible(true);

        if (UIManager.Instance != null)
            UIManager.Instance.IsExternalMenuOpen = true;
    }

    public void OpenCrafting()
    {
        SetShelterButtonsVisible(false);

        if (craftingUI != null)
            craftingUI.Open(this);
    }

    public void Reopen()
    {
        if (root != null)
            root.SetActive(true);

        SetShelterButtonsVisible(true);

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