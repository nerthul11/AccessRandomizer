using RandoSettingsManager;
using RandoSettingsManager.SettingsManagement;
using RandoSettingsManager.SettingsManagement.Versioning;
using AccessRandomizer.Manager;
using AccessRandomizer.Settings;

namespace AccessRandomizer.Interop
{
    internal static class RSM_Interop
    {
        public static void Hook()
        {
            RandoSettingsManagerMod.Instance.RegisterConnection(new AccessSettingsProxy());
        }
    }

    internal class AccessSettingsProxy : RandoSettingsProxy<AccessSettings, string>
    {
        public override string ModKey => AccessRandomizer.Instance.GetName();

        public override VersioningPolicy<string> VersioningPolicy { get; }
            = new EqualityVersioningPolicy<string>(AccessRandomizer.Instance.GetVersion());

        public override void ReceiveSettings(AccessSettings settings)
        {
            if (settings != null)
            {
                ConnectionMenu.Instance!.Apply(settings);
            }
            else
            {
                ConnectionMenu.Instance!.Disable();
            }
        }

        public override bool TryProvideSettings(out AccessSettings settings)
        {
            settings = AccessManager.Settings;
            return settings.Enabled;
        }
    }
}