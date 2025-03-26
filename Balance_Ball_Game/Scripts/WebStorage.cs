using System;
using System.Runtime.InteropServices;
using UnityEngine;

public static class WebStorage
{
    [DllImport("__Internal")]
    private static extern int WebGL_SaveToStorage(string key, string data);

    [DllImport("__Internal")]
    private static extern IntPtr WebGL_LoadFromStorage(string key);

    [DllImport("__Internal")]
    private static extern void Free(IntPtr ptr);

    public static bool Save(string key, object data)
    {
        string json = JsonUtility.ToJson(data);
#if UNITY_WEBGL && !UNITY_EDITOR
        return WebGL_SaveToStorage(key, json) == 1;
#else
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
        return true;
#endif
    }

    public static T Load<T>(string key) where T : new()
    {
        string json;
#if UNITY_WEBGL && !UNITY_EDITOR
        IntPtr ptr = WebGL_LoadFromStorage(key);
        if (ptr != IntPtr.Zero)
        {
            json = Marshal.PtrToStringUTF8(ptr); // Исправление: PtrToStringUTF8
            Free(ptr);
        }
        else
        {
            json = null;
        }
#else
        json = PlayerPrefs.GetString(key);
#endif
        return !string.IsNullOrEmpty(json) ? JsonUtility.FromJson<T>(json) : new T();
    }
}