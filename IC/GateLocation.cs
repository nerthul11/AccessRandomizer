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
        public string objectName;
        public GateLocation(string gate, string scene, string objectName, float x, float y, string mapScene=null, float mapX=0.0f, float mapY=0.0f)
        {
            name = $"Shade_Gate-{gate}";
            this.gate = gate.Replace("_", "");
            this.objectName = objectName;
            sceneName = scene;
            flingType = FlingType.DirectDeposit;
            tags = [LocationTag(gate, scene, x, y, mapScene, mapX, mapY)];
        }
        
        private static Tag LocationTag(string gate, string scene, float x, float y, string mapScene, float mapX, float mapY)
        {
            InteropTag tag = new();
            tag.Properties["ModSource"] = "AccessRandomizer";
            tag.Properties["PoolGroup"] = "Key";
            tag.Properties["PinSprite"] = new AccessSprite("Gate");
            tag.Properties["VanillaItem"] = $"Shade_Gate-{gate}";
            if (mapScene != "Undefined")
            {
                tag.Properties["MapLocations"] = new (string, float, float)[] {(mapScene, mapX, mapY)};
            }
            else
            {
                tag.Properties["WorldMapLocations"] = new (string, float, float)[] {(scene, x, y)};
            }
            tag.Message = "RandoSupplementalMetadata";
            return tag;
        }

        protected override void OnUnload()
        {
            Events.RemoveFsmEdit(sceneName, new("Dash Effect", "Control"), Animation);
            Events.RemoveFsmEdit(sceneName, new("Push L", "Push"), Push);
            Events.RemoveFsmEdit(sceneName, new("Push R", "Push"), Push);
        }
        protected override void OnLoad()
        {
            Events.AddFsmEdit(sceneName, new("Dash Effect", "Control"), Animation);
            Events.AddFsmEdit(sceneName, new("Push L", "Push"), Push);
            Events.AddFsmEdit(sceneName, new("Push R", "Push"), Push);
            On.ShadowGateColliderControl.FixedUpdate += ToggleCollider;
        }

        private void ToggleCollider(On.ShadowGateColliderControl.orig_FixedUpdate orig, ShadowGateColliderControl self)
        {
            string gateName = self.gameObject.transform.parent.name;
            if (sceneName == GameManager.instance.sceneName && objectName == gateName)
            {
                bool hasItem = AccessModule.Instance.ShadeGates.GetVariable<bool>(gate);
                self.disableCollider.enabled = !hasItem;
            }
            else
            {
                orig(self);
            }
        }

        private void Push(PlayMakerFSM fsm)
        {
            string gateName = fsm.gameObject.transform.parent.name;
            if (objectName == gateName)
            {
                fsm.AddState("Gate Open?");
                fsm.AddCustomAction("Gate Open?", () =>
                {
                    AccessRandomizer.Instance.Log($"({fsm.gameObject.transform.position.x}, {fsm.gameObject.transform.position.y})");
                    bool giveItem = HeroController.instance.GetState("shadowDashing");
                    if (giveItem && !Placement.AllObtained())
                    {
                        ItemUtility.GiveSequentially(Placement.Items, Placement, new GiveInfo()
                        {
                            FlingType = FlingType.DirectDeposit,
                            MessageType = MessageType.Corner,
                        });
                    }
                    bool check = AccessModule.Instance.ShadeGates.GetVariable<bool>(gate) == true;
                    if (check)
                        fsm.SendEvent("PASS");
                    else
                        fsm.SendEvent("BOUNCE");
                });
                fsm.AddTransition("Gate Open?", "PASS", "Idle");
                fsm.AddTransition("Gate Open?", "BOUNCE", "Push");
                fsm.ChangeTransition("Idle", "COLLIDE", "Gate Open?");
                fsm.RemoveAction("Shadow Dashing?", 1);
            }
        }

        private void Animation(PlayMakerFSM fsm)
        {            
            string gateName = fsm.gameObject.transform.parent.name;
            if (objectName == gateName)
            {
                fsm.AddState("Gate Open?");
                fsm.AddCustomAction("Gate Open?", () =>
                {
                    fsm.SendEvent(AccessModule.Instance.ShadeGates.GetVariable<bool>(gate) == true ? "PASS" : "BOUNCE");
                });
                fsm.AddTransition("Gate Open?", "PASS", "Effect");
                fsm.AddTransition("Gate Open?", "BOUNCE", "Idle");
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
}