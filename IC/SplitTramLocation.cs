using System;
using AccessRandomizer.Fsm;
using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.Locations;
using ItemChanger.Tags;
using Satchel;

namespace AccessRandomizer.IC
{
    public class SplitTramLocation : CoordinateLocation
    {
        public SplitTramLocation()
        {
            name = "Split_Tram_Pass";
            sceneName = SceneNames.Deepnest_26b;
            x = 21.7f;
            y = 4.4f;
            tags = [SplitTramLocationTag()];
        }
        
        private static Tag SplitTramLocationTag()
        {
            InteropTag tag = new();
            tag.Properties["ModSource"] = "AccessRandomizer";
            tag.Properties["PoolGroup"] = "Keys";
            tag.Properties["VanillaItem"] = "Upper_Tram_Pass";
            tag.Properties["MapLocations"] = new (string, float, float)[] {("Deepnest_26b", -0.85f, -0.2f)};
            tag.Message = "RandoSupplementalMetadata";
            return tag;
        }

        protected override void OnLoad()
        {
            Events.AddFsmEdit(SceneNames.Abyss_03, new("Tram Call Box", "Conversation Control"), LowerTramCallEdit);
            Events.AddFsmEdit(SceneNames.Abyss_03_b, new("Tram Call Box", "Conversation Control"), LowerTramCallEdit);
            Events.AddFsmEdit(SceneNames.Abyss_03_c, new("Tram Call Box", "Conversation Control"), LowerTramCallEdit);
            Events.AddFsmEdit(SceneNames.Abyss_03, new("Door Inspect", "Tram Door"), LowerTramEdit);
            Events.AddFsmEdit(SceneNames.Abyss_03_b, new("Door Inspect", "Tram Door"), LowerTramEdit);
            Events.AddFsmEdit(SceneNames.Abyss_03_c, new("Door Inspect", "Tram Door"), LowerTramEdit);
            Events.AddFsmEdit(SceneNames.Crossroads_46, new("Tram Call Box", "Conversation Control"), UpperTramCallEdit);
            Events.AddFsmEdit(SceneNames.Crossroads_46b, new("Tram Call Box", "Conversation Control"), UpperTramCallEdit);
            Events.AddFsmEdit(SceneNames.Crossroads_46, new("Door Inspect", "Tram Door"), UpperTramEdit);
            Events.AddFsmEdit(SceneNames.Crossroads_46b, new("Door Inspect", "Tram Door"), UpperTramEdit);
        }

        protected override void OnUnload()
        {
            Events.RemoveFsmEdit(SceneNames.Abyss_03, new("Tram Call Box", "Conversation Control"), LowerTramCallEdit);
            Events.RemoveFsmEdit(SceneNames.Abyss_03_b, new("Tram Call Box", "Conversation Control"), LowerTramCallEdit);
            Events.RemoveFsmEdit(SceneNames.Abyss_03_c, new("Tram Call Box", "Conversation Control"), LowerTramCallEdit);
            Events.RemoveFsmEdit(SceneNames.Abyss_03, new("Door Inspect", "Tram Door"), LowerTramEdit);
            Events.RemoveFsmEdit(SceneNames.Abyss_03_b, new("Door Inspect", "Tram Door"), LowerTramEdit);
            Events.RemoveFsmEdit(SceneNames.Abyss_03_c, new("Door Inspect", "Tram Door"), LowerTramEdit);
            Events.RemoveFsmEdit(SceneNames.Crossroads_46, new("Tram Call Box", "Conversation Control"), UpperTramCallEdit);
            Events.RemoveFsmEdit(SceneNames.Crossroads_46b, new("Tram Call Box", "Conversation Control"), UpperTramCallEdit);
            Events.RemoveFsmEdit(SceneNames.Crossroads_46, new("Door Inspect", "Tram Door"), UpperTramEdit);
            Events.RemoveFsmEdit(SceneNames.Crossroads_46b, new("Door Inspect", "Tram Door"), UpperTramEdit);
        }

        private void LowerTramCallEdit(PlayMakerFSM fsm)
        {
            fsm.RemoveAction("Got Pass?", 0);
            fsm.AddAction("Got Pass?", new AccessBooleanFsmCheck("LowerTram", "YES", "NO"));
        }

        private void LowerTramEdit(PlayMakerFSM fsm)
        {
            fsm.RemoveAction("Check Pass", 1);
            fsm.AddAction("Check Pass", new AccessBooleanFsmCheck("LowerTram", "PASS", "NO PASS"));
        }

        private void UpperTramCallEdit(PlayMakerFSM fsm)
        {
            fsm.RemoveAction("Got Pass?", 0);
            fsm.AddAction("Got Pass?", new AccessBooleanFsmCheck("UpperTram", "YES", "NO"));
        }

        private void UpperTramEdit(PlayMakerFSM fsm)
        {
            fsm.RemoveAction("Check Pass", 1);
            fsm.AddAction("Check Pass", new AccessBooleanFsmCheck("UpperTram", "PASS", "NO PASS"));
        }
    }
}