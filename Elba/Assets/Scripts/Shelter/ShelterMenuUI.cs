using UnityEngine;

public class ShelterMenuUI : MonoBehaviour
{
    [SerializeField] private GameObject root;

    public void Show()
    {
        root.SetActive(true);
    }

    public void Hide()
    {
        root.SetActive(false);
    }

    public void ExitShelter()
    {
        ShelterSystem.Instance.ExitShelter();
        Hide();
    }
}