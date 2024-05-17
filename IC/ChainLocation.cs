using AccessRandomizer.Modules;
using ItemChanger;
using ItemChanger.Locations;
using ItemChanger.Util;
using ItemChanger.Tags;
using Satchel;

namespace AccessRandomizer.IC
{
    public class ChainLocation : AutoLocation
    {
        public int chainID;
        public string fsmName;
        public ChainLocation(int id, float x, float y)
        {
            name = $"Hollow_Knight_Chain-{id}";
            sceneName = SceneNames.Room_Final_Boss_Core;
            chainID = id;
            fsmName = chainID == 1 ? "hollow_knight_chain_base" : $"hollow_knight_chain_base {chainID}";
            flingType = FlingType.DirectDeposit;
            tags = [ChainTag(x, y)];
        }
        
        private static Tag ChainTag(float x, float y)
        {
            InteropTag tag = new();
            tag.Properties["ModSource"] = "AccessRandomizer";
            tag.Properties["PoolGroup"] = "Dreamers";
            tag.Properties["PinSprite"] = new AccessSprite("Chain");
            tag.Properties["VanillaItem"] = "Hollow_Knight_Chain";
            tag.Properties["MapLocations"] = new (string, float, float)[] {("Crossroads_02", x, y)};
            tag.Message = "RandoSupplementalMetadata";
            return tag;
        }

        protected override void OnUnload()
        {
            Events.RemoveFsmEdit(sceneName, new(fsmName, "Control"), UnbreakableChainCheck);
            Events.RemoveFsmEdit(sceneName, new("Gate", "Control"), ExitableGate);
            Events.RemoveFsmEdit(sceneName, new("Boss Control", "Battle Start"), FixChainCount);
        }
        protected override void OnLoad()
        {
            Events.AddFsmEdit(sceneName, new(fsmName, "Control"), UnbreakableChainCheck);
            Events.AddFsmEdit(sceneName, new("Gate", "Control"), ExitableGate);
            Events.AddFsmEdit(sceneName, new("Boss Control", "Battle Start"), FixChainCount);
        }

        private void FixChainCount(PlayMakerFSM fsm)
        {
            if (Placement.AllObtained())
                fsm.FsmVariables.GetFsmInt("Chains").Value -= 1;
            if (fsm.FsmVariables.GetFsmInt("Chains").Value == 0)
                PlayerData.instance.unchainedHollowKnight = true;
        }

        private void ExitableGate(PlayMakerFSM fsm)
        {
            if (AccessModule.Instance.ChainsBroken < 4)
                fsm.RemoveTransition("Idle", "ENTER");
        }

        private void UnbreakableChainCheck(PlayMakerFSM fsm)
        {
            if (Placement.AllObtained())
                fsm.ChangeTransition("Init", "FINISHED", "Disable");
            else
                PlayerData.instance.unchainedHollowKnight = false;
            
            fsm.AddState("GiveItem");
            fsm.AddCustomAction("GiveItem", () => {
                ItemUtility.GiveSequentially(Placement.Items, Placement, new GiveInfo()
                {
                    FlingType = FlingType.DirectDeposit,
                    MessageType = MessageType.Corner,
                });
            });

            // Hit counter should not change if item's not obtained
            if (AccessModule.Instance.ChainsBroken < chainID)
                fsm.RemoveAction("Check Hits", 0);
            
            fsm.ChangeTransition("Glow Pt", "FINISHED", "GiveItem");
            fsm.AddTransition("GiveItem", "FINISHED", "Chain Break");
        }
    }
}