using System;
using ItemChanger;
using ItemChanger.Locations;
using ItemChanger.Placements;
using ItemChanger.Tags;

namespace AccessRandomizer.IC
{
    public class SplitElevatorLocation : CoordinateLocation
    {
        public SplitElevatorLocation()
        {
            name = "Split_Elevator_Pass";
            sceneName = SceneNames.Ruins2_10b;
            x = 22.2f;
            y = 10.4f;
            tags = [SplitElevatorLocationTag()];
        }

        private static Tag SplitElevatorLocationTag()
        {
            InteropTag tag = new();
            tag.Properties["ModSource"] = "AccessRandomizer";
            tag.Properties["PoolGroup"] = "Keys";
            tag.Properties["VanillaItem"] = "Right_Elevator_Pass";
            tag.Properties["MapLocations"] = new (string, float, float)[] {("Ruins2_10b", 0.1f, -2.2f)};
            tag.Message = "RandoSupplementalMetadata";
            return tag;
        }

        public override AbstractPlacement Wrap()
        {
            return new MutablePlacement(name)
            {
                Location = this,
                Cost = new GeoCost(150)
            };
        }
    }
}