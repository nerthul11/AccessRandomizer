using AccessRandomizer.Fsm;
using ItemChanger;
using ItemChanger.Locations;
using ItemChanger.Tags;
using Satchel;

namespace AccessRandomizer.IC
{
    public class SlyKeyLocation : CoordinateLocation
    {
        public SlyKeyLocation()
        {
            name = "Sly_Key";
            sceneName = SceneNames.Crossroads_04;
            x = 88.6f;
            y = 3.4f;
            tags = [LocationTag()];
        }
        
        private static Tag LocationTag()
        {
            InteropTag tag = new();
            tag.Properties["ModSource"] = "AccessRandomizer";
            tag.Properties["PoolGroup"] = "Keys";
            tag.Properties["PinSprite"] = new AccessSprite("SlyKey");
            tag.Properties["VanillaItem"] = "Sly_Key";
            tag.Properties["MapLocations"] = new (string, float, float)[] {("Crossroads_04", 0.15f, -0.2f)};
            tag.Message = "RandoSupplementalMetadata";
            return tag;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            Events.AddFsmEdit(SceneNames.Crossroads_04, new("door1", "Door Control"), ToggleTP);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            Events.RemoveFsmEdit(SceneNames.Crossroads_04, new("door1", "Door Control"), ToggleTP);
        }

        private void ToggleTP(PlayMakerFSM fsm)
        {
            fsm.AddState("Is Locked?");
            fsm.AddAction("Is Locked?", new AccessBooleanFsmCheck("UnlockedSly", "UNLOCKED", "LOCKED"));
            fsm.ChangeTransition("Idle", "IN RANGE", "Is Locked?");
            fsm.AddTransition("Is Locked?", "UNLOCKED", "In Range");
            fsm.AddTransition("Is Locked?", "LOCKED", "Idle");
        }
    }
}