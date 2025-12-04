using System;
using System.Collections;
using UnityEngine;

public class NativeSdk : MonoBehaviour
{

    // This method runs BEFORE the first scene loads
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Init()
    {
        // Create GameObject only if not already present
        if (Instance == null)
        {
            GameObject obj = new GameObject("NativeSdk");
            Instance = obj.AddComponent<NativeSdk>();
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

        ItializePlugin("com.mayur.unitytestplugin.SdkInstance");
    }

    private AndroidJavaClass unityclass;
    private AndroidJavaObject unityActivity;
    private AndroidJavaObject _PluginInstance;
    public static NativeSdk Instance;

    private void ItializePlugin(string pluginName)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        unityclass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        unityActivity = unityclass.GetStatic<AndroidJavaObject>("currentActivity");
        _PluginInstance = new AndroidJavaObject(pluginName);
        if (_PluginInstance != null)
        {
            Debug.Log("Done");
        }
        _PluginInstance.CallStatic("receiveUnityActivity", unityActivity);
#endif
    }

    public void ShowShortToast(string message)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (_PluginInstance != null)
        {
            _PluginInstance.Call("ToastShort", message);
        }
#endif
    }

    public void ShowLongToast(string message)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        if (_PluginInstance != null)
        {
            _PluginInstance.Call("ToastLong", message);
        }
#endif
    }
}
