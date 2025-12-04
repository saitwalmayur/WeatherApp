using UnityEngine;
using TMPro;
using System;

public class DayForcast : MonoBehaviour
{
    public TextMeshProUGUI m_Date;
    public TextMeshProUGUI m_Day;
    public TextMeshProUGUI m_Temp;

    public void SetDetail(string date,float temp)
    {
        m_Date.text = date;
        m_Temp.text = temp.ToString()+ "°C";
        string dayName = DateTime.Parse(date).ToString("dddd");
        m_Day.text = dayName.Substring(0, 3);
    }
}
