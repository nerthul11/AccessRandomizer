using System;
using AccessRandomizer.Modules;
using ItemChanger;
using ItemChanger.Locations;
using ItemChanger.Tags;
using ItemChanger.Util;
using Satchel;
using UnityEngine;

namespace AccessRandomizer.IC
{
    public class GateLocation : AutoLocation
    {
        public string gate;
        public string objectName;
        public GateLocation(string gate, string scene, string objectName, float x, float y)
        {
            name = $"Shade_Gate-{gate}";
            this.gate = gate.Replace("_", "");
            this.objectName = objectName;
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
                AccessRandomizer.Instance.Log("Editing Push FSM");
                fsm.AddState("Gate Open?");
                fsm.AddCustomAction("Gate Open?", () =>
                {
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
            AccessRandomizer.Instance.Log(gateName);
            AccessRandomizer.Instance.Log(objectName);
            AccessRandomizer.Instance.Log(objectName == gateName);
            if (objectName == gateName)
            {
                AccessRandomizer.Instance.Log("Editing Animation FSM");
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