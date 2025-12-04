# Unity Native Toast Plugin (Android)

A lightweight Unity package that enables you to display native Android Toast messages directly from Unity using an easy C# API.

Delivered as a `.unitypackage` for plug-and-play integration in any Unity Android project.

---

## 📦 Package Contents

This package includes the following folder structure:

```
Assets/
└── NativeToast/
    ├── Example/
    │   └── Toast/
    │       ├── DetectClick.cs
    │       └── ToastExample.prefab
    ├── Plugins/
    │   ├── Android/
    │   │   └── unityandroidplugin.aar
    │   └── Scripts/
    │       └── NativeSdk.cs
```

---

## 🚀 Getting Started

### 1. Import the Unity Package

In Unity:

1. Go to **Assets → Import Package → Custom Package…**
2. Select the provided `.unitypackage`
3. Import all files

---

## ▶️ Example Scene

After importing, navigate to:

```
Assets/NativeToast/Example/Toast/
```

The example scene contains four interactive objects:

- **Cube**
- **Sphere**
- **Cylinder**
- **Capsule**

Each object has the script `DetectClick.cs` attached.

### ✔️ What Happens?

When you tap any of these objects on your Android device, Unity executes:

```csharp
NativeSdk.Instance.ShowLongToast(gameObject.name);
```

This displays a toast message showing the tapped object's name.

---

## 🧠 Usage in Your Own Game

You can show native toast messages from any script:

```csharp
// Short toast (approximately 2 seconds)
NativeSdk.Instance.ShowShortToast("Hello from Unity!");

// Long toast (approximately 3.5 seconds)
NativeSdk.Instance.ShowLongToast("This is a long toast!");
```

---

## 🚀 No Prefabs Needed — Auto Initialization

You do **not** need to:

- ❌ Create any prefab
- ❌ Add anything to your scene
- ❌ Manually initialize the plugin

The SDK script automatically initializes before the first scene loads using:

```csharp
[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
```

This ensures the plugin is ready to use instantly when the game starts.


---

## 📄 License

This package is provided as-is for use in your Unity projects.

---

## 🤝 Support

For issues, questions, or feature requests, please contact the package developer.

---

**Happy Toasting! 🍞**
