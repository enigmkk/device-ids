#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;

namespace Astra.Device
{
    public interface IDeviceIdsBridge
    {
        void OnGAIDReady(string gaid);
    }

    public class IdsBridge : AndroidJavaProxy
    {
        private readonly IDeviceIdsBridge m_callback;

        public IdsBridge(IDeviceIdsBridge callback) 
            : base("com.astra.ids.IDeviceIdsCallback") // 对应 Java 接口全名
        {
            m_callback = callback;
        }

        // Java 调用的方法
        public void onGAIDReady(string gaid)
        {
            m_callback?.OnGAIDReady(gaid);
        }
    }
}
#endif
