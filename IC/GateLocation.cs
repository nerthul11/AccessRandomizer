using AccessRandomizer.Modules;
using ItemChanger;
using ItemChanger.Locations;
using ItemChanger.Tags;
using ItemChanger.Util;
using Satchel;

namespace AccessRandomizer.IC
{
    public class GateLocation : AutoLocation
    {
        public string gate;
        public GateLocation(string gate, string scene, float x, float y)
        {
            name = $"Shade_Gate-{gate}";
            gate = gate.Replace("_", "");
            sceneName = scene;
            flingType = FlingType.DirectDeposit;
            tags = [LocationTag(gate, scene, x, y)];
        }
        
        private static Tag LocationTag(string gate, string scene, float x, float y)
        {
            InteropTag tag = new();
            tag.Properties["ModSource"] = "AccessRandomizer";
            tag.Properties["PoolGroup"] = "Key";
            tag.Properties["PinSprite"] = new AccessSprite("Gate");
            tag.Properties["VanillaItem"] = $"Shade_Gate-{gate}";
            tag.Properties["WorldMapLocations"] = new (string, float, float)[] {(scene, x, y)};
            tag.Message = "RandoSupplementalMetadata";
            return tag;
        }

        protected override void OnUnload()
        {
            Events.RemoveFsmEdit(sceneName, new("Dash Effect", "Control"), GateChange);
        }
        protected override void OnLoad()
        {
            Events.AddFsmEdit(sceneName, new("Dash Effect", "Control"), GateChange);
        }

        private void GateChange(PlayMakerFSM fsm)
        {
            fsm.AddState("Gate Open?");
            fsm.AddCustomAction("Gate Open?", () =>
            {
                fsm.SendEvent(AccessModule.Instance.ShadeGates.GetVariable<bool>(gate) == true ? "FINISHED" : "CANCEL");
            });
            fsm.AddTransition("Gate Open?", "FINISHED", "Effect");
            fsm.AddTransition("Gate Open?", "CANCEL", "Idle");

            fsm.AddState("GiveItem");
            fsm.AddCustomAction("GiveItem", () => {
                ItemUtility.GiveSequentially(Placement.Items, Placement, new GiveInfo()
                {
                    FlingType = FlingType.DirectDeposit,
                    MessageType = MessageType.Corner,
                });
            });
            fsm.AddTransition("GiveItem", "FINISHED", "Gate Open?");
            fsm.ChangeTransition("Shadow Dashing?", "FINISHED", "GiveItem");
            fsm.ChangeTransition("Shadow Dashing?", "CANCEL", "Gate Open?");
        }
    }
}