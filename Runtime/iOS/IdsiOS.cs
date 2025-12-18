#if UNITY_IOS && !UNITY_EDITOR
using System;
using System.Runtime.InteropServices;

namespace Astra.Device
{
    public class IdsiOS : IdsPlatform
    {
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern IntPtr DeviceId();
        [System.Runtime.InteropServices.DllImport("__Internal")]
        internal static extern IntPtr DeviceIDFA();
        [System.Runtime.InteropServices.DllImport("__Internal")]
        internal static extern IntPtr DeviceIDFV();
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void Device_Free(IntPtr ptr);



        string m_idfa = "";
        public override string IDFA
        {
            get { return m_idfa; }
        }
        string m_idfv = "";
        public override string IDFV
        {
            get { return m_idfv; }
        }
        string m_deviceId = "";
        public override string UUID
        {
            get
            {
                if (string.IsNullOrEmpty(m_deviceId))
                {
                    m_deviceId = GetUUID();
                }
                return m_deviceId;
            }
        }
        public override void Init()
        {
            ATT.Init();
            ATT.OnStatusChanged += status =>
            {
                UnityEngine.Debug.Log("ATT status = " + status);

                if (status == ATTStatus.Authorized)
                {
                    // 可以安全获取 IDFA
                    m_idfa = GetIDFA();
                }
                else
                {
                    m_idfv = GetIDFV();
                }
            };
            // 在合适时机触发
            ATT.Request();
        }
        ///////////////////////////////////////////////////
        private string GetUUID()
        {
            IntPtr p = DeviceId();
            if (p == IntPtr.Zero)
                return null;

            string s = Marshal.PtrToStringAnsi(p);
            Device_Free(p);
            return s;
        }
        private string GetIDFA()
        {
            IntPtr p = DeviceIDFA();
            if (p == IntPtr.Zero) return null;

            string v = Marshal.PtrToStringAnsi(p);
            Device_Free(p);
            return v;
        }

        private string GetIDFV()
        {
            IntPtr p = DeviceIDFV();
            if (p == IntPtr.Zero) return null;

            string v = Marshal.PtrToStringAnsi(p);
            Device_Free(p);
            return v;
        }
    }
}
#endif