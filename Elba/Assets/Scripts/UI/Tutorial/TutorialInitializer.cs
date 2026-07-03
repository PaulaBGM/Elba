using UnityEngine;

public class TutorialInitializer : MonoBehaviour
{
    private void Start()
    {
        TutorialPopup.Instance.Show(
            "StartTutorial",
            "MISION",
            "Sobrevive el mayor tiempo posible, reúne recursos para fabricar objetos y mejora tu refugio.\n\n" +
            "� Click izquierdo: Atacar y talar.\n" +
            "� E: Interactuar con objetos.\n" 
        );
    }
}