using UnityEngine;

public class UIStatsManager : MonoBehaviour
{
    [SerializeField] private EventManagerSO eventManager;

    [Header("Bars")]
    [SerializeField] private UIStatBar healthBar;
    [SerializeField] private UIStatBar hungerBar;
    [SerializeField] private UIStatBar thirstBar;
    [SerializeField] private UIStatBar staminaBar;
    //[SerializeField] private UIStatBar temperatureBar;

    private void OnEnable()
    {
        Debug.Log("UIStatsManager SUBSCRIBE");
        eventManager.OnPlayerStatChanged += UpdateStat;
    }

    private void OnDisable()
    {
        Debug.Log("UIStatsManager UNSUBSCRIBE");
        eventManager.OnPlayerStatChanged -= UpdateStat;
    }

    private void UpdateStat( StatType stat,float current,float max)
    {
        Debug.Log($"UI RECIBE {stat}: {current}/{max}");

        switch (stat)
        {
            case StatType.Health:

                Debug.Log("ENTRANDO EN HEALTH");

                healthBar.UpdateBar(current, max);

                Debug.Log("SALIENDO DE HEALTH");

                break;

            case StatType.Hunger:
                hungerBar.UpdateBar(current, max);
                break;

            case StatType.Thirst:
                thirstBar.UpdateBar(current, max);
                break;

            case StatType.Stamina:
                staminaBar.UpdateBar(current, max);
                break;

            case StatType.Temperature:
                //temperatureBar.UpdateBar(current, max);
                break;
        }
    }
}