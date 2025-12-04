# Unity Weather & Air Quality App

Test Apk: - https://drive.google.com/file/d/11k36lyS-t3h-Pk-6Yk_ZGdSYcK4qJzcX/view?usp=sharing

A **Unity-based Weather and Air Quality App** that fetches real-time weather and AQI (Air Quality Index) data using the [Open-Meteo API](https://open-meteo.com/en/docs). The app automatically detects the user’s location, displays temperature, AQI, and daily forecasts, and supports native toast notifications on **Android** and **iOS**.

---

## 📦 Features

- Fetch current weather by latitude and longitude.
- Fetch air quality (PM2.5 & PM10) and calculate AQI.
- Display daily temperature forecasts.
- Show country, latitude, and longitude.
- Native toast notifications with today’s temperature.
- Automatic location handling (GPS & permission management).
- Auto-initialization — no need to create prefabs manually.

---

## 📝 Usage

### Location Permission
On enabling the `WeatherDetail` script, the app checks for location permission. If granted, it fetches weather and air quality data automatically.

### Display Weather & AQI
Weather and air quality data is displayed on the UI using **TextMeshProUGUI** elements.

### Show Toast Notification
Clicking the assigned button triggers a native toast message showing today’s maximum temperature:

```csharp
NativeSdk.Instance.ShowShortToast("Today's Temperature: " + lastDataLoaded.daily.temperature_2m_max[0] + "°C");