using AccessRandomizer.Fsm;
using ItemChanger;
using ItemChanger.Locations;
using ItemChanger.Tags;
using Satchel;

namespace AccessRandomizer.IC
{
    public class ZoteKeyLocation : CoordinateLocation
    {
        public ZoteKeyLocation()
        {
            name = "Zote_Key";
            sceneName = SceneNames.Deepnest_33;
            x = 56.2f;
            y = 12.4f;
            tags = [LocationTag()];
        }
        
        private static Tag LocationTag()
        {
            InteropTag tag = new();
            tag.Properties["ModSource"] = "AccessRandomizer";
            tag.Properties["PoolGroup"] = "Keys";
            tag.Properties["PinSprite"] = new AccessSprite("ZoteKey");
            tag.Properties["VanillaItem"] = "Zote_Key";
            tag.Properties["MapLocations"] = new (string, float, float)[] {("Deepnest_33", -0.12f, -0.15f)};
            tag.Message = "RandoSupplementalMetadata";
            return tag;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            Events.AddFsmEdit(SceneNames.Room_Bretta, new("Basement Open", "FSM"), BasementToggle);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            Events.RemoveFsmEdit(SceneNames.Room_Bretta, new("Basement Open", "FSM"), BasementToggle);
        }

        private void BasementToggle(PlayMakerFSM fsm)
        {
            AccessBooleanFsmCheck check = new("UnlockedZote", "", "DEACTIVATE");
            fsm.RemoveAction("Check", 0);
            fsm.AddAction("Check", check);
        }
    }
}