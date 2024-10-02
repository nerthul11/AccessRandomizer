using System;
using ItemChanger;
using ItemChanger.Locations;
using ItemChanger.Tags;
using ItemChanger.Util;
using Satchel;
using UnityEngine;

namespace AccessRandomizer.IC
{
    public class TrapBenchLocation : AutoLocation
    {
        public TrapBenchLocation()
        {
            name = "Trap_Bench";
            sceneName = SceneNames.Deepnest_Spider_Town;
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
            Events.AddFsmEdit(sceneName, new("RestBench Spider", "FSM"), SpawnShiny);
            Events.AddFsmEdit(sceneName, new("RestBench Spider", "Bench Control Spider"), GiveItem);
        }

        protected override void OnUnload()
        {
            Events.RemoveFsmEdit(sceneName, new("RestBench Spider", "FSM"), SpawnShiny);
            Events.RemoveFsmEdit(sceneName, new("RestBench Spider", "Bench Control Spider"), GiveItem);
        }

        private void SpawnShiny(PlayMakerFSM fsm)
        {
            fsm.AddState("Spawn Shiny");
            fsm.AddCustomAction("Spawn Shiny", () => 
                {
                    if (!Placement.AllObtained())
                    {
                        Container c = Container.GetContainer(Container.Shiny);
                        GameObject shiny = c.GetNewContainer(new(c.Name, Placement, flingType));
                        shiny.transform.position = new(47.0f, 58.4f);
                        shiny.SetActive(true);
                    }
                });
            fsm.ChangeTransition("Check", "DESTROY", "Spawn Shiny");
            fsm.AddTransition("Spawn Shiny", "FINISHED", "Destroy");
        }
        private void GiveItem(PlayMakerFSM fsm)
        {
            AccessRandomizer.Instance.Log("This exists");
            fsm.AddState("GiveItem");
            fsm.AddCustomAction("GiveItem", () => {
                ItemUtility.GiveSequentially(Placement.Items, Placement, new GiveInfo()
                {
                    FlingType = FlingType.DirectDeposit,
                    MessageType = MessageType.Corner,
                });
            });

            fsm.ChangeTransition("Sit Start", "FINISHED", "GiveItem");
            fsm.AddTransition("GiveItem", "FINISHED", "Neutral");
        }
    }
}