using UnityEngine;
using UnityEngine.UI;
using DigitalRuby.RainMaker;

public class WeatherEffects : MonoBehaviour
{
    [SerializeField] private RainScript2D rain;

    [SerializeField] private Image coldOverlay;

    [SerializeField] private float overlaySpeed = 2f;

    private float targetAlpha;

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
        Color c = coldOverlay.color;

        c.a = Mathf.MoveTowards(
            c.a,
            targetAlpha,
            overlaySpeed * Time.deltaTime);

        coldOverlay.color = c;
    }

    void ChangeWeather(WeatherType weather)
    {
        switch (weather)
        {
            case WeatherType.Clear:

                rain.RainIntensity = 0;

                targetAlpha = 0;

                break;

            case WeatherType.Rain:

                rain.RainIntensity = 0.7f;

                targetAlpha = 0;

                break;

            case WeatherType.Cold:

                rain.RainIntensity = 0;

                targetAlpha = 0.6f;

                break;
        }
    }
}