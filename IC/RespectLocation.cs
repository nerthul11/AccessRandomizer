using ItemChanger;
using ItemChanger.Locations;
using ItemChanger.Util;
using ItemChanger.Tags;
using KorzUtils.Helper;
using Satchel;
using System.Linq;
using AccessRandomizer.Fsm;

namespace AccessRandomizer.IC
{
    public class RespectLocation : AutoLocation
    {
        public RespectLocation()
        {
            name = "Mantis_Respect";
            sceneName = SceneNames.Fungus2_15_boss;
            flingType = FlingType.DirectDeposit;
            tags = [RespectTag()];
        }
        
        private static Tag RespectTag()
        {
            InteropTag tag = new();
            tag.Properties["ModSource"] = "AccessRandomizer";
            tag.Properties["PoolGroup"] = "Keys";
            tag.Properties["PinSprite"] = new AccessSprite("Respect");
            tag.Properties["VanillaItem"] = "Mantis_Respect";
            tag.Properties["MapLocations"] = new (string, float, float)[] {("Fungus2_15", 0.0f, -0.5f)};
            tag.Message = "RandoSupplementalMetadata";
            return tag;
        }

        protected override void OnUnload()
        {
            On.SceneAdditiveLoadConditional.OnEnable -= WeRespect;
            Events.RemoveFsmEdit(sceneName, new("Mantis Battle", "Battle Control"), DisrespectfulLords);
            Events.RemoveFsmEdit(sceneName, new("Challenge Prompt", "Challenge Start"), ToggleChallenge);
            Events.RemoveFsmEdit(sceneName, new("Bow Range", "Bow Range"), BowToGorb);
        }
        protected override void OnLoad()
        {
            On.SceneAdditiveLoadConditional.OnEnable += WeRespect;
            Events.AddFsmEdit(sceneName, new("Mantis Battle", "Battle Control"), DisrespectfulLords);
            Events.AddFsmEdit(sceneName, new("Challenge Prompt", "Challenge Start"), ToggleChallenge);
            Events.AddFsmEdit(sceneName, new("Bow Range", "Bow Range"), BowToGorb);
        }

        private void BowToGorb(PlayMakerFSM fsm)
        {
            if (!Placement.AllObtained())
                fsm.DestroyAll();
        }

        private void WeRespect(On.SceneAdditiveLoadConditional.orig_OnEnable orig, SceneAdditiveLoadConditional self)
        {
            if (self.sceneNameToLoad == sceneName && !Placement.AllObtained())
            {
                self.altSceneNameToLoad = self.sceneNameToLoad;
            }
            orig(self);
        }

        private void ToggleChallenge(PlayMakerFSM fsm)
        {
            // Disable if location is cleared
            if (Placement.AllObtained() || Placement.Items.All(x => x.WasEverObtained()))
            {
                fsm.ChangeTransition("Can Talk Bool?", "TRUE", "Idle 2");
                // Keep the location disabled but spawn shiny item
                if (Placement.Items.All(x => x.WasEverObtained()))
                    ItemHelper.SpawnShiny(new(30.2f, 7.4f), Placement);
            }
            else
                // Force enable if it isn't
                On.DeactivateIfPlayerdataTrue.OnEnable += ButStillWeFight;
        }

        private void ButStillWeFight(On.DeactivateIfPlayerdataTrue.orig_OnEnable orig, DeactivateIfPlayerdataTrue self)
        {
            if (self.gameObject.name == "Challenge Prompt")
                return;
            orig(self);
        }

        private void DisrespectfulLords(PlayMakerFSM fsm)
        {
            // Add a complain state that locks you in the cage if respect is unobtained
            fsm.AddState("Complain");
            fsm.AddAction("Complain", new CustomAudio("MantisLord"));

            // Grant an item after the battle.
            fsm.AddState("GiveItem");
            fsm.AddCustomAction("GiveItem", () => {
                ItemUtility.GiveSequentially(Placement.Items, Placement, new GiveInfo()
                {
                    FlingType = FlingType.DirectDeposit,
                    MessageType = MessageType.Corner,
                });
            });
            fsm.AddAction("GiveItem", new AccessBooleanFsmCheck("RespectObtained", "TRUE", "FALSE"));
            fsm.AddTransition("GiveItem", "TRUE", "Bow");
            fsm.AddTransition("GiveItem", "FINISHED", "Complain");

            // Defeating Mantis Lords no longer activates defeatedMantisLords bool, as it represents respect.
            fsm.RemoveAction("Return 2", 3);
            fsm.ChangeTransition("Return 2", "FINISHED", "GiveItem");
        }
    }
}