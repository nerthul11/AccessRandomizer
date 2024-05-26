using ItemChanger;
using ItemChanger.Locations;
using ItemChanger.Tags;

namespace AccessRandomizer.IC
{
    public class MapperKeyLocation : CoordinateLocation
    {
        public MapperKeyLocation()
        {
            name = "Mapper_Key";
            sceneName = SceneNames.Crossroads_33;
            x = 14.6f;
            y = 6.4f;
            tags = [MapperTag()];
        }
        
        private static Tag MapperTag()
        {
            InteropTag tag = new();
            tag.Properties["ModSource"] = "AccessRandomizer";
            tag.Properties["PoolGroup"] = "Keys";
            tag.Properties["PinSprite"] = new AccessSprite("MapperKey");
            tag.Properties["VanillaItem"] = "Mapper_Key";
            tag.Properties["MapLocations"] = new (string, float, float)[] {("Crossroads_33", -0.2f, -0.25f)};
            tag.Message = "RandoSupplementalMetadata";
            return tag;
        }
    }
}