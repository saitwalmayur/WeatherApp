using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class WeatherDetail : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_Temprature;
    [SerializeField] private TextMeshProUGUI m_LongLat;
    [SerializeField] private TextMeshProUGUI m_AQI;
    [SerializeField] private TextMeshProUGUI m_Country;
    [SerializeField] private WeatherResponse lastDataLoaded = new WeatherResponse();
    [SerializeField] private AirQualityResponse airQualityResponse = new AirQualityResponse();
    [SerializeField] private DayForcast[] dayForcasts;
    [SerializeField] private Button m_ShowToast;
    [SerializeField] private Loading m_Loading;
    private void OnEnable()
    {
        m_Loading.gameObject.SetActive(true);
        if (LocationHandler.Instance.isLocationEnabled())
        {
            GetWeatherDetails();
        }
        LocationHandler.Instance.OnPermissionAccept += OnPermissionAccept;

        LocationHandler.Instance.GetCountryName();

        foreach (TimeZoneInfo tz in TimeZoneInfo.GetSystemTimeZones())
        {
            Debug.Log("ID: " + tz.Id + " | Name: " + tz.DisplayName);
        }
    }
    private void Start()
    {
        m_ShowToast.onClick.RemoveAllListeners();
        m_ShowToast.onClick.AddListener(()=>
        {

            NativeSdk.Instance.ShowShortToast("Todays Temperature:- "   +lastDataLoaded.daily.temperature_2m_max[0].ToString() + "°C");
        });
    }

    private void OnPermissionAccept()
    {
        GetWeatherDetails();
    }
    private void OnDisable()
    {
        LocationHandler.Instance.OnPermissionAccept -= OnPermissionAccept;
    }
    public void GetWeatherDetails()
    {
        StartCoroutine(WeathorAPI.GetWeatherData(LocationHandler.Instance.currLoc.lat, LocationHandler.Instance.currLoc.lon, "IST", "temperature_2m_max",(result)=>
        {
            lastDataLoaded = result;
            ShowDetails();
            m_Loading.gameObject.SetActive(false);
        }));

        StartCoroutine(WeathorAPI.FetchAirQuality(LocationHandler.Instance.currLoc.lat, LocationHandler.Instance.currLoc.lon, (result) =>
        {
            airQualityResponse = result;

            float pm25 = result.hourly.pm2_5[0];

            int aqi = ConvertPM25ToAQI(pm25);

            m_AQI.text = "AQI  " + aqi.ToString();
            m_Loading.gameObject.SetActive(false);
        }));
    }
   
    void ShowDetails()
    {
        m_Temprature.text = lastDataLoaded.daily.temperature_2m_max[0].ToString() + "°C";
        m_Country.text = LocationHandler.Instance.GetCountryName();
        for (int i = 0; i < lastDataLoaded.daily.time.Length; i++)
        {
            dayForcasts[i].SetDetail(lastDataLoaded.daily.time[i], lastDataLoaded.daily.temperature_2m_max[i]);
        }
        m_LongLat.text = LocationHandler.Instance.currLoc.lat.ToString("F2") + "°, " + LocationHandler.Instance.currLoc.lon.ToString("F2") + "°";
    }

    int ConvertPM25ToAQI(float pm)
    {
        if (pm <= 12.0f) return CalcAQI(pm, 0, 12, 0, 50);
        if (pm <= 35.4f) return CalcAQI(pm, 12.1f, 35.4f, 51, 100);
        if (pm <= 55.4f) return CalcAQI(pm, 35.5f, 55.4f, 101, 150);
        if (pm <= 150.4f) return CalcAQI(pm, 55.5f, 150.4f, 151, 200);
        if (pm <= 250.4f) return CalcAQI(pm, 150.5f, 250.4f, 201, 300);
        if (pm <= 350.4f) return CalcAQI(pm, 250.5f, 350.4f, 301, 400);
        if (pm <= 500.4f) return CalcAQI(pm, 350.5f, 500.4f, 401, 500);

        return 500; // MAX
    }

    int CalcAQI(float C, float Clow, float Chigh, int Ilow, int Ihigh)
    {
        return Mathf.RoundToInt(((Ihigh - Ilow) / (Chigh - Clow)) * (C - Clow) + Ilow);
    }
}
