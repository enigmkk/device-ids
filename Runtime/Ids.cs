namespace Astra.Device
{
    public class Ids
    {
        public static Ids s_instance;
        public static Ids Instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = new Ids();
                }
                return s_instance;
            }
        }
        public Ids()
        {
        }
        IdsPlatform m_idsPlatform = null;
        public void Init()
        {   
#if UNITY_ANDROID && !UNITY_EDITOR
            m_idsPlatform = new IdsAndroid();
#elif UNITY_IOS && !UNITY_EDITOR
            m_idsPlatform = new IdsiOS();
#else
            m_idsPlatform = new IdsPlatform();
#endif
            m_idsPlatform.Init();
        }
        /// <summary>
        /// 设备唯一 ID（稳定）
        /// </summary>
        public string DeviceId
        {
            get
            {
                return m_idsPlatform.UUID;
            }
        }
        public string IDFA
        {
            get
            {
                return m_idsPlatform.IDFA;
            }
        }
        public string IDFV
        {
            get
            {          
                return m_idsPlatform.IDFV;
            }
        }
        public string GAID
        {
            get
            {
                return m_idsPlatform.GAID;
            }
        }
    }
}
