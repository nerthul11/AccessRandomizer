using System;
using AccessRandomizer.Fsm;
using AccessRandomizer.Modules;
using ItemChanger;
using ItemChanger.Tags;
using ItemChanger.UIDefs;
using Satchel;

namespace AccessRandomizer.IC
{
    public class RelicKeyItem : AbstractItem
    {
        public override bool Redundant() => AccessModule.Instance.UnlockedLemm;
        public override void GiveImmediate(GiveInfo info)
        {
            AccessModule.Instance.UnlockedLemm = true;
            AccessModule.Instance.CompletedChallenges();
        }

        public RelicKeyItem()
        {
            name = "Relic_Key";
            UIDef = new MsgUIDef()
            {
                name = new BoxedString("Relic Key"),
                shopDesc = new BoxedString("If you have any interesting relics, feel free to drop by.<br>-Lemm"),
                sprite = new AccessSprite("RelicKey")
            };
            tags = [RelicTag(), CurseTag()];
        }
        private static Tag RelicTag()
        {
            InteropTag tag = new();
            tag.Properties["ModSource"] = "AccessRandomizer";
            tag.Properties["PoolGroup"] = "Keys";
            tag.Properties["PinSprite"] = new AccessSprite("RelicKey");
            tag.Message = "RandoSupplementalMetadata";
            return tag;
        }

        private InteropTag CurseTag()
        {
            InteropTag tag = new();
            tag.Properties["CanMimic"] = new BoxedBool(true);
            tag.Properties["MimicNames"] = new string[] {"Relik Key", "Reelic Key", "Re1ic Key", "Relic Kee"};
            tag.Message = "CurseData";
            return tag;
        }

        protected override void OnLoad()
        {
            Events.AddFsmEdit(SceneNames.Ruins1_05b, new("Antique Dealer Door", "Door Control"), RefactorDoor);
            Events.AddLanguageEdit(new LanguageKey("Relic Dealer", "RELICDEALER_DOOR"), DoorMessage);
        }

        protected override void OnUnload()
        {
            Events.RemoveFsmEdit(SceneNames.Ruins1_05b, new("Antique Dealer Door", "Door Control"), RefactorDoor);
            Events.RemoveLanguageEdit(new LanguageKey("Relic Dealer", "RELICDEALER_DOOR"), DoorMessage);
        }

        private void RefactorDoor(PlayMakerFSM fsm)
        {
            fsm.AddState("Exist");
            fsm.AddFirstAction("Check", new AccessBooleanFsmCheck("UnlockedLemm", "DESTROY", "LEAVE"));
            fsm.AddTransition("Check", "LEAVE", "Exist");
        }

        private void DoorMessage(ref string value)
        {
            value = "The door is locked, and I lost the key. If you find it, please bring me some relics.<br><br>-Lemm";
        }
    }
}