using AccessRandomizer.Settings;
using AccessRandomizer.Modules;
using ItemChanger;
using Newtonsoft.Json;
using RandomizerMod.Logging;
using RandomizerMod.RC;

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
            ItemChangerMod.Modules.GetOrAdd<AccessModule>();
        }

        private static void AddFileSettings(LogArguments args, System.IO.TextWriter tw)
        {
            // Log settings into the settings file
            tw.WriteLine("Access Randomizer Settings:");
            using JsonTextWriter jtw = new(tw) { CloseOutput = false };
            RandomizerMod.RandomizerData.JsonUtil._js.Serialize(jtw, Settings);
            tw.WriteLine();            
        }        
    }
}