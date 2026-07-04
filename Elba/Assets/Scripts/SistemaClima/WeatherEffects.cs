using UnityEngine;
using UnityEngine.UI;
using DigitalRuby.RainMaker;

public class WeatherEffects : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RainScript2D rain;
    [SerializeField] private Image coldOverlay;

    [Header("Overlay")]
    [SerializeField] private float overlaySpeed = 2f;
    [SerializeField] private float secondsToMaxOverlay = 30f;

    private bool isRaining;
    private float rainExposure;

    private void Start()
    {
        WeatherManager.Instance.OnWeatherChanged += ChangeWeather;
        ChangeWeather(WeatherManager.Instance.CurrentWeather);
    }

    private void OnDestroy()
    {
        if (WeatherManager.Instance != null)
            WeatherManager.Instance.OnWeatherChanged -= ChangeWeather;
    }

    private void Update()
    {
        if (isRaining)
            rainExposure += Time.deltaTime;
        else
            rainExposure -= Time.deltaTime * 2f;
        rainExposure = Mathf.Clamp(rainExposure, 0f, secondsToMaxOverlay);
        float targetAlpha = rainExposure / secondsToMaxOverlay;

        if (UIManager.Instance.IsInventoryOpen ||UIManager.Instance.IsExternalMenuOpen)
        {
            targetAlpha = 0f;
        }
        Color color = coldOverlay.color;
        color.a = Mathf.MoveTowards(color.a,targetAlpha,overlaySpeed * Time.deltaTime);
        coldOverlay.color = color;
    }

    private void ChangeWeather(WeatherType weather)
    {
        isRaining = weather == WeatherType.Rain;
        rain.RainIntensity = isRaining ? 0.8f : 0f;
    }
}