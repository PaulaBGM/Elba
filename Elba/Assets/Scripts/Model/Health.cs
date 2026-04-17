using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        Debug.Log("Vida inicial: " + currentHealth);
    }

    void Update()
    {
        // Solo para pruebas rápidas
        if (Input.GetKeyDown(KeyCode.H))
        {
            AddHealth(10);
        }

        if (Input.GetKeyDown(KeyCode.J))
        {
            TakeDamage(10);
        }
    }

    public void AddHealth(int amount)
    {
        currentHealth += amount;

        // Evitar pasar el máximo
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        Debug.Log("Curado +" + amount + " | Vida actual: " + currentHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth < 0)
        {
            currentHealth = 0;
        }

        Debug.Log("Daño -" + amount + " | Vida actual: " + currentHealth);

        if (currentHealth == 0)
        {
            Debug.Log("El objeto ha muerto");
        }
    }
}