namespace AccessRandomizer.Settings
{
    public class AccessSettings
    {
        public bool Enabled { get; set; }
        public bool MantisRespect { get; set;}
        public bool HollowKnightChains { get; set;}
        public bool UniqueKeys { get; set; }
        public bool SplitTram { get; set; }
        public bool SplitElevator { get; set; }
        public bool TrapBench { get; set; }
        public bool ShadeGates { get; set; }
        public CustomKeySettings CustomKeys { get; set; } = new();
    }

    public class CustomKeySettings
    {
        public bool MapperKey;
        public bool SlyKey;
        public bool BrettaKey;
        public bool ZoteKey;
        public bool RelicKey;
        public bool GladeKey;
        public bool Any() => MapperKey || SlyKey || BrettaKey || ZoteKey || RelicKey || GladeKey;
    }
}