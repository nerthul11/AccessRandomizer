using AccessRandomizer.Interop;
using AccessRandomizer.Manager;
using AccessRandomizer.Settings;
using Modding;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AccessRandomizer
{
    public class AccessRandomizer : Mod, IGlobalSettings<AccessSettings> 
    {
        new public string GetName() => "AccessRandomizer";
        public static GameObject slyDoor;
        public override string GetVersion() => "1.3.0.0";

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
        public override List<(string, string)> GetPreloadNames() {
            return [("Crossroads_04", "_Transition Gates/Mender Door")];
        }
        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloads)
        {
            slyDoor = preloads["Crossroads_04"]["_Transition Gates/Mender Door"];
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
                        "Mantis_Respect", "Hollow_Knight_Chain",
                        "Graveyard_Key", "Waterways_Key", "Pleasure_Key", "Coffin_Key", 
                        "Mapper_Key", "Sly_Key", "Bretta_Key", "Zote_Key", "Relic_Key",
                        "Left_Elevator_Pass", "Right_Elevator_Pass",
                        "Upper_Tram_Pass", "Lower_Tram_Pass", 
                        "Glade_Key", "Trap_Bench"
                    ]
                );
                Instance.Log("Initialized.");
            }
        }
        public void OnLoadGlobal(AccessSettings s) => GS = s;
        public AccessSettings OnSaveGlobal() => GS;
    }   
}