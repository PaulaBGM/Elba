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
        eventManager.OnPlayerStatChanged += UpdateStat;
    }

    private void OnDisable()
    {
        eventManager.OnPlayerStatChanged -= UpdateStat;
    }

    private void UpdateStat( StatType stat,float current,float max)
    {

        switch (stat)
        {
            case StatType.Health:


                healthBar.UpdateBar(current, max);


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