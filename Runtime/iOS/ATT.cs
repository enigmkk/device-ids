#if UNITY_IOS && !UNITY_EDITOR

using System;
using UnityEngine;

public enum ATTStatus
{
    NotDetermined = 0,
    Authorized    = 1,
    Denied        = 2,
    Restricted    = 3
}

public static class ATT
{
    public static event Action<ATTStatus> OnStatusChanged;

#if UNITY_IOS && !UNITY_EDITOR
    private static ATTNative.ATTCallback _callback;
#endif

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void Init()
    {
        _callback = OnNativeStatus;
        ATTNative.ATT_SetCallback(_callback);
    }

    public static void Request()
    {
        ATTNative.ATT_Request();
    }

    public static bool IsAuthorized()
    {
        return ATTNative.ATT_IsAuthorized();
    }

    [AOT.MonoPInvokeCallback(typeof(ATTNative.ATTCallback))]
    private static void OnNativeStatus(int status)
    {
        OnStatusChanged?.Invoke((ATTStatus)status);
    }
}
#endif