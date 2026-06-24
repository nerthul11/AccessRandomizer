using AccessRandomizer.Modules;
using ItemChanger;
using ItemChanger.Tags;
using ItemChanger.UIDefs;

namespace AccessRandomizer.IC
{
    public class BirthplaceKeyItem : AbstractItem
    {
        public override bool Redundant() => PlayerData.instance.openedBlackEggPath;
        public override void GiveImmediate(GiveInfo info)
        {
            PlayerData.instance.openedBlackEggPath = true;
            AccessModule.Instance.CompletedChallenges();
        }

        public BirthplaceKeyItem()
        {
            name = "Birthplace_Key";
            UIDef = new MsgUIDef()
            {
                name = new BoxedString("Birthplace Key"),
                shopDesc = new BoxedString("I am the shadow, the keeper of light. If you want the sun's power, then show me your own."),
                sprite = new AccessSprite("BirthplaceKey")
            };
            tags = [ItemTag(), CurseTag()];
        }
        private static Tag ItemTag()
        {
            InteropTag tag = new();
            tag.Properties["ModSource"] = "AccessRandomizer";
            tag.Properties["PoolGroup"] = "Keys";
            tag.Properties["PinSprite"] = new AccessSprite("BirthplaceKey");
            tag.Message = "RandoSupplementalMetadata";
            return tag;
        }
        private InteropTag CurseTag()
        {
            InteropTag tag = new();
            tag.Properties["CanMimic"] = new BoxedBool(true);
            tag.Properties["MimicNames"] = new string[] {"Birthday Key", "Void Heart [67]", "Birtplace Key", "Birthplace"};
            tag.Message = "CurseData";
            return tag;
        }
    }
}