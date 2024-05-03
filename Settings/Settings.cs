namespace AccessRandomizer.Settings
{
    public class GlobalSettings
    {
        public AccessSettings Settings { get; set; } = new();
    }

    public class LocalSettings
    {
        public int ChainsBroken { get; set; } = 0;
    }
}