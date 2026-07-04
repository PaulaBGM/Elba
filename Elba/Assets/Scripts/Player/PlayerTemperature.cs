using UnityEngine;

public class PlayerTemperature : MonoBehaviour
{
    [SerializeField] private PlayerStatsSystem stats;
    [SerializeField] private float coldDamage = 2f;
    [SerializeField] private float tick = 1f;
    private float timer;

    private void Awake()
    {
        if (stats == null)
            stats = GetComponent<PlayerStatsSystem>();
    }

    private void Update()
    {
        if (!WeatherManager.Instance.IsRaining())
            return;

        timer += Time.deltaTime;
        if (timer < tick)
            return;

        timer = 0f;
        if (Campfire.PlayerNearLitCampfire)
            return;
        stats.ModifyStat(StatType.Health,-coldDamage);
    }
}