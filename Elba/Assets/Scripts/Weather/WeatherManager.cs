using System;
using System.Collections;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager Instance;

    public WeatherType CurrentWeather { get; private set; }

    public event Action<WeatherType> OnWeatherChanged;

    [Header("Weather Time")]
    [SerializeField] private float minTime = 120f;
    [SerializeField] private float maxTime = 240f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetWeather(WeatherType.Clear);
        StartCoroutine(WeatherLoop());
    }

    IEnumerator WeatherLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(minTime, maxTime));

            WeatherType next =
                (WeatherType)UnityEngine.Random.Range(0, Enum.GetValues(typeof(WeatherType)).Length);

            SetWeather(next);
        }
    }

    public void SetWeather(WeatherType weather)
    {
        if (CurrentWeather == weather)
            return;

        CurrentWeather = weather;

        OnWeatherChanged?.Invoke(weather);
    }
}