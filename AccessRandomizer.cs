using AccessRandomizer.Manager;
using AccessRandomizer.Settings;
using Modding;
using System;

namespace AccessRandomizer
{
    public class AccessRandomizer : Mod, ILocalSettings<LocalSettings>, IGlobalSettings<GlobalSettings> 
    {
        new public string GetName() => "AccessRandomizer";
        public override string GetVersion() => "1.0.0.1";

        private static AccessRandomizer _instance;
        public AccessRandomizer() : base()
        {
            _instance = this;
        }
        internal static AccessRandomizer Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException($"{nameof(AccessRandomizer)} was never initialized");
                }
                return _instance;
            }
        }
        public GlobalSettings GS { get; internal set; } = new();
        public LocalSettings LS { get; internal set; } = new();
        public override void Initialize()
        {
            // Ignore completely if Randomizer 4 is inactive
            if (ModHooks.GetMod("Randomizer 4") is Mod)
            {
                Instance.Log("Initializing...");
                AccessManager.Hook();
                if (ModHooks.GetMod("RandoSettingsManager") is Mod)
                    RSM_Interop.Hook();
                CondensedSpoilerLogger.AddCategory("Hollow Knight Chains", () => AccessManager.Settings.Enabled, ["Hollow_Knight_Chain"]);
                Instance.Log("Initialized.");
            }
        }
        public void OnLoadGlobal(GlobalSettings s) => GS = s;
        public GlobalSettings OnSaveGlobal() => GS;
        public void OnLoadLocal(LocalSettings s) => LS = s;
        public LocalSettings OnSaveLocal() => LS;
    }   
}