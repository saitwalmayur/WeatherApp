using System;
using System.Collections;
using UnityEngine;
using System.Globalization;

[System.Serializable]
public class LocationDetail
{
    public float lon;
    public float lat;

    public LocationDetail()
    {
        lon = 0;
        lat = 0;
    }
    public LocationDetail(float lon, float lat)
    {
        this.lon = lon;
        this.lat = lat;
    }
}


public class LocationHandler : MonoBehaviour
{

    // This method runs BEFORE the first scene loads
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        // Create GameObject only if not already present
        if (Instance == null)
        {
            GameObject obj = new GameObject("NativeSdk");
            Instance = obj.AddComponent<LocationHandler>();
            DontDestroyOnLoad(obj);
        }
    }

    private void Awake()
    {
        // Prevent duplicates if scene already has one
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static LocationHandler Instance;
    public bool gps_ok;
    public LocationDetail startLoc = new LocationDetail();
    public LocationDetail currLoc = new LocationDetail();

    public Action OnPermissionAccept;

    public string GetCountryName()
    {
        RegionInfo region = RegionInfo.CurrentRegion;
        return region.DisplayName;
    }

    public bool isLocationEnabled()
    {
        return Input.location.isEnabledByUser;
    }

    IEnumerator Start()
    {
        // Check if the user has location service enabled.
        if (!Input.location.isEnabledByUser)
        {
            Debug.Log("Location not enabled on device or app does not have permission to access location");
        }
        // Starts the location service.
        Input.location.Start();

        // Waits until the location service initializes
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // If the service didn't initialize in 20 seconds this cancels location service use.
        if (maxWait < 1)
        {
            Debug.Log("Timed out");
            yield break;
        }

        // If the connection failed this cancels location service use.
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogError("Unable to determine device location");

            yield break;
        }
        else
        {
            // If the connection succeeded, this retrieves the device's current location and displays it in the Console window.
            Debug.Log("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
            gps_ok = true;
            OnPermissionAccept?.Invoke();
        }
    }
    public void StopGPS()
    {
        Input.location.Stop();
    }
    public void StoreCurrentGPS()
    {
        startLoc = new LocationDetail(currLoc.lon, currLoc.lat);
    }
    void Update()
    {
        if (gps_ok)
        {
            currLoc.lat = Input.location.lastData.latitude;
            currLoc.lon = Input.location.lastData.longitude;
        }
    }
}
