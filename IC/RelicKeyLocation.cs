using ItemChanger;
using ItemChanger.Locations;
using ItemChanger.Tags;

namespace AccessRandomizer.IC
{
    public class RelicKeyLocation : CoordinateLocation
    {
        public RelicKeyLocation()
        {
            name = "Relic_Key";
            sceneName = SceneNames.Waterways_01;
            x = 78.3f;
            y = 31.4f;
            tags = [RelicLocationTag()];
        }
        
        private static Tag RelicLocationTag()
        {
            InteropTag tag = new();
            tag.Properties["ModSource"] = "AccessRandomizer";
            tag.Properties["PoolGroup"] = "Keys";
            tag.Properties["PinSprite"] = new AccessSprite("RelicKey");
            tag.Properties["VanillaItem"] = "Relic_Key";
            tag.Properties["MapLocations"] = new (string, float, float)[] {("Waterways_01", 0f, 0f)};
            tag.Message = "RandoSupplementalMetadata";
            return tag;
        }
    }
}