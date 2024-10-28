using BepInEx;
using BepInEx.Logging;

namespace StudioItemSkeleton
{
    [BepInPlugin(GUID, PluginName, Version)]
    public class StudioItemSkeleton : BaseUnityPlugin
    {
        public const string PluginName = "StudioItemSkeleton";
        public const string GUID = "org.njaecha.plugins.studioitemskeleton";
        public const string Version = "1.0.0";
        
        internal new static ManualLogSource Logger;
        
        void Awake()
        {
            Logger = base.Logger;
        }
    }
}

