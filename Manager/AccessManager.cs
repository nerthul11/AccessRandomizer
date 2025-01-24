using AccessRandomizer.Settings;
using AccessRandomizer.Modules;
using ItemChanger;
using Newtonsoft.Json;
using RandomizerMod.Logging;
using RandomizerMod.RC;
using ItemChanger.Modules;

namespace AccessRandomizer.Manager
{
    internal static class AccessManager
    {
        public static AccessSettings Settings => AccessRandomizer.Instance.GS;
        public static void Hook()
        {
            ItemHandler.Hook();
            ConnectionMenu.Hook();
            LogicHandler.Hook();
            SettingsLog.AfterLogSettings += AddFileSettings;
            RandoController.OnExportCompleted += InitiateModule;
        }

        private static void InitiateModule(RandoController controller)
        {
            if (!Settings.Enabled)
                return;
    
            ItemChangerMod.Modules.GetOrAdd<AccessModule>();

            if (Settings.NPCKeys)
                ItemChangerMod.Modules.Remove(ItemChangerMod.Modules.GetOrAdd<AutoUnlockIselda>());
            if (Settings.SplitElevator)
                ItemChangerMod.Modules.Remove(ItemChangerMod.Modules.GetOrAdd<ElevatorPass>());
            if (Settings.TrapBench)
                ItemChangerMod.Modules.Remove(ItemChangerMod.Modules.GetOrAdd<ReusableBeastsDenEntrance>());
        }

        private static void AddFileSettings(LogArguments args, System.IO.TextWriter tw)
        {
            if (!Settings.Enabled)
                return;

            // Log settings into the settings file
            tw.WriteLine("Access Randomizer Settings:");
            using JsonTextWriter jtw = new(tw) { CloseOutput = false };
            RandomizerMod.RandomizerData.JsonUtil._js.Serialize(jtw, Settings);
            tw.WriteLine();            
        }        
    }
}