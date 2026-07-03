using UnityEngine;

public class PlayerTemperature : MonoBehaviour
{
    [SerializeField] private PlayerStatsSystem stats;

    [SerializeField] private float coldDamage = 3f;

    [SerializeField] private float coldRecovery = 5f;

    [SerializeField] private float tick = 1f;

    private float timer;

    private void Awake()
    {
        if (stats == null)
            stats = GetComponent<PlayerStatsSystem>();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer < tick)
            return;

        timer = 0;

        switch (WeatherManager.Instance.CurrentWeather)
        {
            case WeatherType.Clear:

                stats.ModifyStat(
                    StatType.Temperature,
                    coldRecovery);

                break;

            case WeatherType.Rain:

                stats.ModifyStat(
                    StatType.Temperature,
                    -coldDamage);

                break;

            case WeatherType.Cold:

                stats.ModifyStat(
                    StatType.Temperature,
                    -coldDamage * 2);

                break;
        }

        if (stats.IsLow(StatType.Temperature))
        {
            stats.ModifyStat(
                StatType.Health,
                -2f);
        }
    }
}