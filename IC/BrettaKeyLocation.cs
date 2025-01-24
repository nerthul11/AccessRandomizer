using AccessRandomizer.Fsm;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using ItemChanger;
using ItemChanger.Extensions;
using ItemChanger.Locations;
using ItemChanger.Tags;

namespace AccessRandomizer.IC
{
    public class BrettaKeyLocation : CoordinateLocation
    {
        public BrettaKeyLocation()
        {
            name = "Bretta_Key";
            sceneName = SceneNames.Fungus2_23;
            x = 62.4f;
            y = 56.4f;
            tags = [LocationTag()];
        }
        
        private static Tag LocationTag()
        {
            InteropTag tag = new();
            tag.Properties["ModSource"] = "AccessRandomizer";
            tag.Properties["PoolGroup"] = "Keys";
            tag.Properties["PinSprite"] = new AccessSprite("BrettaKey");
            tag.Properties["VanillaItem"] = "Bretta_Key";
            tag.Properties["MapLocations"] = new (string, float, float)[] {("Fungus2_23", -0.25f, 0.1f)};
            tag.Message = "RandoSupplementalMetadata";
            return tag;
        }

        protected override void OnLoad()
        {
            base.OnLoad();
            Events.AddFsmEdit(SceneNames.Town, new("bretta_house", "Check Opened"), ToggleDoor);
            Events.AddFsmEdit(SceneNames.Town, new("door_bretta", "Door Control"), ToggleTP);
        }

        protected override void OnUnload()
        {
            base.OnUnload();
            Events.RemoveFsmEdit(SceneNames.Town, new("bretta_house", "Check Opened"), ToggleDoor);
            Events.RemoveFsmEdit(SceneNames.Town, new("door_bretta", "Door Control"), ToggleTP);
        }

        private void ToggleDoor(PlayMakerFSM fsm)
        {
            AccessBooleanFsmCheck check = new("UnlockedBretta", "UNLOCKED", "LOCKED");
            FsmState init = fsm.GetState("Init");
            init.RemoveActionsOfType<PlayerDataBoolTest>();
            init.AddLastAction(check);
            init.AddTransition("UNLOCKED", "Opened");
            init.AddTransition("LOCKED", "Closed");
        }

        private void ToggleTP(PlayMakerFSM fsm)
        {
            fsm.AddState("Is Locked?");
            FsmState idle = fsm.GetState("Idle");
            FsmState isLocked = fsm.GetState("Is Locked?");
            isLocked.AddLastAction(new AccessBooleanFsmCheck("UnlockedBretta", "UNLOCKED", "LOCKED"));
            idle.ClearTransitions();
            idle.AddTransition("IN RANGE", "Is Locked?");
            isLocked.AddTransition("UNLOCKED", "In Range");
            isLocked.AddTransition("LOCKED", "Idle");
        }
    }
}