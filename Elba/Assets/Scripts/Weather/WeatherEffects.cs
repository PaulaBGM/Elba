using UnityEngine;
using UnityEngine.UI;
using DigitalRuby.RainMaker;

public class WeatherEffects : MonoBehaviour
{
    [SerializeField] private RainScript2D rain;
    [SerializeField] private PlayerStatsSystem stats;

    [SerializeField] private Image coldOverlay;

    [SerializeField] private float overlaySpeed = 1.5f;

    [SerializeField] private float startOverlayPercent = 0.7f;
    [SerializeField] private float maxOverlayPercent = 0.2f;

    private void Start()
    {
        WeatherManager.Instance.OnWeatherChanged += ChangeWeather;
    }

    private void OnDestroy()
    {
        if (WeatherManager.Instance != null)
            WeatherManager.Instance.OnWeatherChanged -= ChangeWeather;
    }

    private void Update()
    {
        float percent = stats.GetPercent(StatType.Temperature);

        float alpha = 0f;

        if (percent <= startOverlayPercent)
        {
            alpha = Mathf.InverseLerp(
                startOverlayPercent,
                maxOverlayPercent,
                percent);
        }

        Color c = coldOverlay.color;

        c.a = Mathf.MoveTowards(
            c.a,
            alpha,
            overlaySpeed * Time.deltaTime);

        coldOverlay.color = c;
    }

    void ChangeWeather(WeatherType weather)
    {
        switch (weather)
        {
            case WeatherType.Clear:
                rain.RainIntensity = 0f;
                break;

            case WeatherType.Rain:
                rain.RainIntensity = 0.8f;
                break;
        }
    }
}