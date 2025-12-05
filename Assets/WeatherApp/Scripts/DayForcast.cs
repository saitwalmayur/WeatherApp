using UnityEngine;
using TMPro;
using System;

public class DayForcast : MonoBehaviour
{
    // UI element for showing the date (e.g., "2025-12-05")
    public TextMeshProUGUI m_Date;

    // UI element for showing short day name (e.g., "Mon", "Tue")
    public TextMeshProUGUI m_Day;

    // UI element for showing the temperature (e.g., "28°C")
    public TextMeshProUGUI m_Temp;

    /// <summary>
    /// Sets all UI details for the forecast item.
    /// </summary>
    /// <param name="date">Date string in a valid DateTime format (e.g., "2025-12-05")</param>
    /// <param name="temp">Temperature value in Celsius</param>
    public void SetDetail(string date, float temp)
    {
        // Assign raw date text
        m_Date.text = date;

        // Convert temperature to string and append degree symbol
        m_Temp.text = temp.ToString() + "°C";

        // Convert date string to DateTime and extract full weekday name (e.g., "Monday")
        string dayName = DateTime.Parse(date).ToString("dddd");

        // Use only the first 3 letters (e.g., "Mon")
        m_Day.text = dayName.Substring(0, 3);
    }
}
