using AccessRandomizer.Interop;
using AccessRandomizer.Manager;
using AccessRandomizer.Settings;
using Modding;
using System;

namespace AccessRandomizer
{
    public class AccessRandomizer : Mod, IGlobalSettings<AccessSettings> 
    {
        new public string GetName() => "AccessRandomizer";
        public override string GetVersion() => "1.2.4.1";

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
        public AccessSettings GS { get; internal set; } = new();
        public override void Initialize()
        {
            // Ignore completely if Randomizer 4 is inactive
            if (ModHooks.GetMod("Randomizer 4") is Mod)
            {
                Instance.Log("Initializing...");
                AccessManager.Hook();
                
                if (ModHooks.GetMod("RandoSettingsManager") is Mod)
                    RSM_Interop.Hook();
                
                if (ModHooks.GetMod("FStatsMod") is Mod)
                    FStats_Interop.Hook();
                
                CondensedSpoilerLogger.AddCategory("Miscellaneous Access", () => AccessManager.Settings.Enabled, 
                    [
                        "Mapper_Key", "Mantis_Respect", "Graveyard_Key", "Waterways_Key", "Pleasure_Key", 
                        "Coffin_Key", "Glade_Key", "Hollow_Knight_Chain", 
                        "Left_Elevator_Pass", "Right_Elevator_Pass",
                        "Upper_Tram_Pass", "Lower_Tram_Pass", "Trap_Bench"
                    ]
                );
                Instance.Log("Initialized.");
            }
        }
        public void OnLoadGlobal(AccessSettings s) => GS = s;
        public AccessSettings OnSaveGlobal() => GS;
    }   
}