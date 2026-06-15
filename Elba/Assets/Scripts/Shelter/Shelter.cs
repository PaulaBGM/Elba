using UnityEngine;

public class Shelter : MonoBehaviour, IInteractable
{
    [SerializeField] private ShelterMenuUI menu;

    public void Interact(GameObject interactor)
    {
        ShelterSystem.Instance.EnterShelter();

        menu.Show();
    }
}