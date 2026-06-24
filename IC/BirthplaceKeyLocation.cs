using AccessRandomizer.Fsm;
using AccessRandomizer.Manager;
using AccessRandomizer.Modules;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.Locations;
using ItemChanger.Tags;
using Satchel;

namespace AccessRandomizer.IC
{
    public class BirthplaceKeyLocation : CoordinateLocation
    {
        public BirthplaceKeyLocation()
        {
            name = "Birthplace_Key";
            sceneName = SceneNames.Abyss_06_Core;
            x = 17.4f;
            y = 5.4f;
            tags = [LocationTag(x, y)];
        }
        
        private static Tag LocationTag(float x, float y)
        {
            InteropTag tag = new();
            tag.Properties["ModSource"] = "AccessRandomizer";
            tag.Properties["PoolGroup"] = "Keys";
            tag.Properties["PinSprite"] = new AccessSprite("BirthplaceKey");
            tag.Properties["VanillaItem"] = "Birthplace_Key";
            tag.Properties["WorldMapLocations"] = new (string, float, float)[] {("Abyss_06_Core", x, y)};
            tag.Message = "RandoSupplementalMetadata";
            return tag;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            Events.AddFsmEdit(SceneNames.Abyss_06_Core, new("floor_closed", "Disappear"), FromTheDepths);
            Events.AddFsmEdit(SceneNames.Abyss_06_Core, new("floor_full_open", "FSM"), OfTheAbyss);
            Events.AddFsmEdit(SceneNames.Abyss_06_Core, new("Floor Opening Animation", "FSM"), OfTheAbyss);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            Events.RemoveFsmEdit(SceneNames.Abyss_06_Core, new("floor_closed", "Disappear"), FromTheDepths);
            Events.RemoveFsmEdit(SceneNames.Abyss_06_Core, new("floor_full_open", "FSM"), OfTheAbyss);
            Events.RemoveFsmEdit(SceneNames.Abyss_06_Core, new("Floor Opening Animation", "FSM"), OfTheAbyss);
        }

        private void FromTheDepths(PlayMakerFSM fsm)
        {
            if (!AccessManager.Settings.Enabled || !AccessManager.Settings.CustomKeys.BirthplaceKey)
                return;

            if (PlayerData.instance.openedBlackEggPath)
                fsm.gameObject.SetActive(false);

            FsmState state1 = fsm.GetValidState("State 1");
            state1.ClearActions();
            state1.RemoveTransitionsOn("BLACK FLOOR OPEN");
        }

        private void OfTheAbyss(PlayMakerFSM fsm)
        {
            if (!AccessManager.Settings.Enabled || !AccessManager.Settings.CustomKeys.BirthplaceKey)
                return;

            fsm.enabled = false;
        }
    }
}