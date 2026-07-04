using System;
using System.Collections;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager Instance { get; private set; }

    public WeatherType CurrentWeather { get; private set; }

    public event Action<WeatherType> OnWeatherChanged;

    [Header("Weather Time")]
    [SerializeField] private float minTime = 120f;
    [SerializeField] private float maxTime = 240f;

    private Coroutine weatherRoutine;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        SetWeather(WeatherType.Clear);

        weatherRoutine = StartCoroutine(WeatherLoop());
    }

    private IEnumerator WeatherLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(minTime, maxTime));

            WeatherType next =
                CurrentWeather == WeatherType.Clear
                ? WeatherType.Rain
                : WeatherType.Clear;

            SetWeather(next);
        }
    }

    public void SetWeather(WeatherType weather)
    {
        if (CurrentWeather == weather)
            return;

        CurrentWeather = weather;

        OnWeatherChanged?.Invoke(CurrentWeather);
    }

    public bool IsRaining()
    {
        return CurrentWeather == WeatherType.Rain;
    }

    public bool IsClear()
    {
        return CurrentWeather == WeatherType.Clear;
    }
}