using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class WeatherDetail : MonoBehaviour
{
    // UI text to display current temperature
    [SerializeField] private TextMeshProUGUI m_Temprature;

    // UI text to display latitude & longitude
    [SerializeField] private TextMeshProUGUI m_LongLat;

    // UI text for AQI value
    [SerializeField] private TextMeshProUGUI m_AQI;

    // UI text showing the user’s country name
    [SerializeField] private TextMeshProUGUI m_Country;

    // Stores the last fetched weather data
    [SerializeField] private WeatherResponse lastDataLoaded = new WeatherResponse();

    // Stores the last fetched air quality data
    [SerializeField] private AirQualityResponse airQualityResponse = new AirQualityResponse();

    // UI prefab elements for daily weather forecast
    [SerializeField] private DayForcast[] dayForcasts;

    // Button to trigger toast message
    [SerializeField] private Button m_ShowToast;

    // Loading spinner reference
    [SerializeField] private Loading m_Loading;

    private void OnEnable()
    {
        // Show loading UI
        m_Loading.gameObject.SetActive(true);

        // If location permission already granted, fetch weather immediately
        if (LocationHandler.Instance.isLocationEnabled())
        {
            GetWeatherDetails();
        }

        // Register permission callback
        LocationHandler.Instance.OnPermissionAccept += OnPermissionAccept;

        // Fetch country name for UI display
        LocationHandler.Instance.GetCountryName();

        // Print all system time zones for debugging
        foreach (TimeZoneInfo tz in TimeZoneInfo.GetSystemTimeZones())
        {
            Debug.Log("ID: " + tz.Id + " | Name: " + tz.DisplayName);
        }
    }

    private void Start()
    {
        // Add onClick listener for showing toast
        m_ShowToast.onClick.RemoveAllListeners();
        m_ShowToast.onClick.AddListener(() =>
        {
            NativeSdk.Instance.ShowShortToast(
                "Today's Temperature: " + lastDataLoaded.daily.temperature_2m_max[0].ToString() + "°C"
            );
        });
    }

    private void OnPermissionAccept()
    {
        // Called when user accepts location permissions
        GetWeatherDetails();
    }

    private void OnDisable()
    {
        // Unregister listener to avoid memory leaks
        LocationHandler.Instance.OnPermissionAccept -= OnPermissionAccept;
    }

    /// <summary>
    /// Calls weather and air quality APIs using the current latitude and longitude.
    /// </summary>
    public void GetWeatherDetails()
    {
        // Fetch weather data
        StartCoroutine(WeathorAPI.GetWeatherData(
            LocationHandler.Instance.currLoc.lat,
            LocationHandler.Instance.currLoc.lon,
            "IST",
            "temperature_2m_max",
            (result) =>
            {
                lastDataLoaded = result;
                ShowDetails();
                m_Loading.gameObject.SetActive(false);
            }));

        // Fetch air quality data
        StartCoroutine(WeathorAPI.FetchAirQuality(
            LocationHandler.Instance.currLoc.lat,
            LocationHandler.Instance.currLoc.lon,
            (result) =>
            {
                airQualityResponse = result;

                float pm25 = result.hourly.pm2_5[0];
                int aqi = ConvertPM25ToAQI(pm25);

                m_AQI.text = "AQI  " + aqi.ToString();
                m_Loading.gameObject.SetActive(false);
            }));
    }

    /// <summary>
    /// Updates all UI elements with received weather data.
    /// </summary>
    void ShowDetails()
    {
        // Set temperature
        m_Temprature.text = lastDataLoaded.daily.temperature_2m_max[0].ToString() + "°C";

        // Show country name
        m_Country.text = LocationHandler.Instance.GetCountryName();

        // Show 7-day forecast
        for (int i = 0; i < lastDataLoaded.daily.time.Length; i++)
        {
            dayForcasts[i].SetDetail(
                lastDataLoaded.daily.time[i],
                lastDataLoaded.daily.temperature_2m_max[i]
            );
        }

        // Format latitude & longitude to 2 decimal places
        m_LongLat.text =
            LocationHandler.Instance.currLoc.lat.ToString("F2") + "°, " +
            LocationHandler.Instance.currLoc.lon.ToString("F2") + "°";
    }

    /// <summary>
    /// Converts PM2.5 concentration into US AQI value based on EPA standards.
    /// </summary>
    int ConvertPM25ToAQI(float pm)
    {
        if (pm <= 12.0f) return CalcAQI(pm, 0, 12, 0, 50);
        if (pm <= 35.4f) return CalcAQI(pm, 12.1f, 35.4f, 51, 100);
        if (pm <= 55.4f) return CalcAQI(pm, 35.5f, 55.4f, 101, 150);
        if (pm <= 150.4f) return CalcAQI(pm, 55.5f, 150.4f, 151, 200);
        if (pm <= 250.4f) return CalcAQI(pm, 150.5f, 250.4f, 201, 300);
        if (pm <= 350.4f) return CalcAQI(pm, 250.5f, 350.4f, 301, 400);
        if (pm <= 500.4f) return CalcAQI(pm, 350.5f, 500.4f, 401, 500);

        return 500; // Maximum possible AQI
    }

    /// <summary>
    /// AQI calculation formula:
    /// (Ihigh - Ilow) / (Chigh - Clow) * (C - Clow) + Ilow
    /// </summary>
    int CalcAQI(float C, float Clow, float Chigh, int Ilow, int Ihigh)
    {
        return Mathf.RoundToInt(((Ihigh - Ilow) / (Chigh - Clow)) * (C - Clow) + Ilow);
    }
}
