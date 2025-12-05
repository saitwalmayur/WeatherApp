using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WeathorAPI 
{

    public static IEnumerator GetWeatherData(float latitude, float longitude , string timezone,string dailyParam,Action<WeatherResponse> onComplete)
    {
        // Build API URL
        string url =
                   $"https://api.open-meteo.com/v1/forecast" +
                   $"?latitude={latitude}" +
                   $"&longitude={longitude}" +
                   $"&timezone={timezone}" +
                   $"&daily={dailyParam}";

        Debug.Log("Calling URL: " + url);

        UnityWebRequest request = UnityWebRequest.Get(url);

        // Wait for API
        yield return request.SendWebRequest();

        // Error Handling
        if (request.result == UnityWebRequest.Result.ConnectionError ||
            request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.LogError("API Error: " + request.error);
            yield break;
        }

        // Get Response
        string jsonResult = request.downloadHandler.text;
        Debug.Log("Weather API Response:\n" + jsonResult);

        // Optional: Parse JSON
        WeatherResponse data = JsonUtility.FromJson<WeatherResponse>(jsonResult);
        onComplete?.Invoke(data);

        Debug.Log("Max Temperature Today: " + data.daily.temperature_2m_max[0]);
    }


    public static IEnumerator FetchAirQuality(float latitude, float longitude,Action<AirQualityResponse> result)
    {
        string baseUrl = "https://air-quality-api.open-meteo.com/v1/air-quality";
        // Build URL
        string url = $"{baseUrl}?latitude={latitude}&longitude={longitude}&hourly=pm10,pm2_5";

        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            // Send request and wait
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError ||
                request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error fetching air quality: {request.error}");
            }
            else
            {
                string json = request.downloadHandler.text;
                try
                {
                    AirQualityResponse resp = JsonUtility.FromJson<AirQualityResponse>(json);
                    result?.Invoke(resp);
                    Debug.Log($"Latitude: {resp.latitude}, Longitude: {resp.longitude}, Elevation: {resp.elevation}");
                    Debug.Log($"First timestamp: {resp.hourly.time[0]}, PM10: {resp.hourly.pm10[0]} {resp.hourly_units.pm10}, PM2.5: {resp.hourly.pm2_5[0]} {resp.hourly_units.pm2_5}");
                    // Process more data as needed...
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to parse JSON: {ex}");
                }
            }
        }
    }
}
[System.Serializable]
public class DailyData
{
    public string[] time;
    public float[] temperature_2m_max;
}

[System.Serializable]
public class WeatherResponse
{
    public float latitude;
    public float longitude;
    public DailyData daily;
}

[System.Serializable]

public class AirQualityResponse
{
    public double latitude;
    public double longitude;
    public double elevation;
    public double generationtime_ms;
    public int utc_offset_seconds;
    public string timezone;
    public string timezone_abbreviation;
    public Hourly hourly;
    public HourlyUnits hourly_units;
}

[Serializable]
public class Hourly
{
    public List<string> time;
    public List<float> pm10;
    public List<float> pm2_5;
}

[Serializable]
public class HourlyUnits
{
    public string pm10;
    public string pm2_5;
}