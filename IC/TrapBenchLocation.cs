using ItemChanger;
using ItemChanger.Locations;
using ItemChanger.Tags;

namespace AccessRandomizer.IC
{
    public class TrapBenchLocation : CoordinateLocation
    {
        public TrapBenchLocation()
        {
            name = "Trap_Bench";
            sceneName = SceneNames.Deepnest_Spider_Town;
            x = 45.0f;
            y = 58.4f;
            tags = [TrapBenchLocationTag()];
        }
        
        private static Tag TrapBenchLocationTag()
        {
            InteropTag tag = new();
            tag.Properties["ModSource"] = "AccessRandomizer";
            tag.Properties["PoolGroup"] = "Keys";
            tag.Properties["PinSprite"] = new ItemChangerSprite("ShopIcons.BenchPin");
            tag.Properties["VanillaItem"] = "Trap_Bench";
            tag.Properties["MapLocations"] = new (string, float, float)[] {("Deepnest_10", 0.35f, 0.25f)};
            tag.Message = "RandoSupplementalMetadata";
            return tag;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
        }
    }
}