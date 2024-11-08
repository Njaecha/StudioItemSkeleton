using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using Screencap;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StudioItemSkeleton
{
    [BepInPlugin(GUID, PluginName, Version)]
    [BepInProcess(KKAPI.KoikatuAPI.StudioProcessName)]
    [BepInDependency(KKAPI.KoikatuAPI.GUID, KKAPI.KoikatuAPI.VersionConst)]
    [BepInDependency(ScreenshotManager.GUID, ScreenshotManager.Version)]
    public class StudioItemSkeleton : BaseUnityPlugin
    {
        public const string PluginName = "StudioItemSkeleton";
        public const string GUID = "org.njaecha.plugins.studioitemskeleton";
        public const string Version = "1.0.1";
        
        internal new static ManualLogSource Logger;
        internal static StudioItemSkeletonGizmos Gizmos;

        internal static ConfigEntry<Color> GizmoColor;
        
        void Awake()
        {
            Logger = base.Logger;
            GizmoColor = Config.Bind("General", "Line Color", Color.white, "Color of the item skeleton");
        }

        void Start()
        {
            ScreenshotManager.OnPreCapture += ScreenshotManager_OnPreCapture;
            ScreenshotManager.OnPostCapture += ScreenshotManager_OnPostCapture;
            SceneManager.sceneLoaded += (s, lsm) => CreateStudioButton(s.name);
        }

        private static void CreateStudioButton(string arg0Name)
        {
            Gizmos = Camera.main.GetOrAddComponent<StudioItemSkeletonGizmos>();
        }

        private static void ScreenshotManager_OnPostCapture()
        {
            Gizmos.enabled = true;
        }

        private static void ScreenshotManager_OnPreCapture()
        {
            Gizmos.enabled = false;
        }
    }
}

