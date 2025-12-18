#if UNITY_IOS && !UNITY_EDITOR
using System.Runtime.InteropServices;

internal static class ATTNative
{

    internal delegate void ATTCallback(int status);

    [DllImport("__Internal")]
    internal static extern void ATT_SetCallback(ATTCallback cb);

    [DllImport("__Internal")]
    internal static extern void ATT_Request();

    [DllImport("__Internal")]
    internal static extern bool ATT_IsAuthorized();
}
#endif
