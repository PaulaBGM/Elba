using UnityEngine;

public class TutorialInitializer : MonoBehaviour
{
    private void Start()
    {
        TutorialPopup.Instance.Show(
            "StartTutorial",
            "MISION",
            "Sobrevive el mayor tiempo posible.\n\n" +
            "• Click izquierdo: Atacar y talar.\n" +
            "• E: Interactuar con objetos.\n" +
            "• Reúne recursos para fabricar herramientas.\n" +
            "• Construye y mejora tu refugio."
        );
    }
}