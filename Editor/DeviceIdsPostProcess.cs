using UnityEditor;
using UnityEditor.Callbacks;
#if UNITY_IOS
using UnityEditor.iOS.Xcode;
#endif
using System.IO;

namespace Company.DeviceId.Editor
{
    public static class DeviceIdsPostProcess
    {
        private const string TrackingDesc =
            "This identifier will be used to deliver personalized ads.";

        [PostProcessBuild(999)]
        public static void OnPostProcessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (target != BuildTarget.iOS)
                return;
#if UNITY_IOS
            InjectFrameworks(pathToBuiltProject);
            InjectPlist(pathToBuiltProject);
#endif
        }
#if UNITY_IOS
        // ===============================
        // 注入系统 Framework
        // ===============================
        private static void InjectFrameworks(string path)
        {
            string pbxPath = PBXProject.GetPBXProjectPath(path);
            var pbx = new PBXProject();
            pbx.ReadFromFile(pbxPath);

            string targetGuid = pbx.GetUnityMainTargetGuid();

            AddFramework(pbx, targetGuid, "AppTrackingTransparency.framework");
            AddFramework(pbx, targetGuid, "AdSupport.framework");

            pbx.WriteToFile(pbxPath);
        }

        private static void AddFramework(
            PBXProject pbx,
            string targetGuid,
            string framework)
        {
            if (!pbx.ContainsFramework(targetGuid, framework))
            {
                pbx.AddFrameworkToProject(targetGuid, framework, false);
            }
        }

        // ===============================
        // 注入 Info.plist
        // ===============================
        private static void InjectPlist(string path)
        {
            string plistPath = Path.Combine(path, "Info.plist");
            var plist = new PlistDocument();
            plist.ReadFromFile(plistPath);

            var root = plist.root;

            // NSUserTrackingUsageDescription
            if (!root.values.ContainsKey("NSUserTrackingUsageDescription"))
            {
                root.SetString(
                    "NSUserTrackingUsageDescription",
                    TrackingDesc
                );
            }

            File.WriteAllText(plistPath, plist.WriteToString());
        }
        #endif
    }
}

