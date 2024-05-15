using Newtonsoft.Json;
using RandomizerMod.Logging;
using AccessRandomizer.Settings;

namespace AccessRandomizer.Manager
{
    internal static class AccessManager
    {
        public static AccessSettings Settings => AccessRandomizer.Instance.GS.Settings;
        public static void Hook()
        {
            ItemHandler.Hook();
            ConnectionMenu.Hook();
            LogicHandler.Hook();
            SettingsLog.AfterLogSettings += AddFileSettings;
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