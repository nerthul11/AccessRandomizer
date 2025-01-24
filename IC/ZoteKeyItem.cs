using AccessRandomizer.Modules;
using ItemChanger;
using ItemChanger.Tags;
using ItemChanger.UIDefs;

namespace AccessRandomizer.IC
{
    public class ZoteKeyItem : AbstractItem
    {
        public override bool Redundant() => AccessModule.Instance.UnlockedZote;
        public override void GiveImmediate(GiveInfo info)
        {
            AccessModule.Instance.UnlockedZote = true;
            AccessModule.Instance.CompletedChallenges();
        }

        public ZoteKeyItem()
        {
            name = "Zote_Key";
            UIDef = new MsgUIDef()
            {
                name = new BoxedString("Zote Key"),
                shopDesc = new BoxedString("Now where did I leave this one? -Cornifer"),
                sprite = new AccessSprite("ZoteKey")
            };
            tags = [ItemTag(), CurseTag()];
        }
        private static Tag ItemTag()
        {
            InteropTag tag = new();
            tag.Properties["ModSource"] = "AccessRandomizer";
            tag.Properties["PoolGroup"] = "Keys";
            tag.Properties["PinSprite"] = new AccessSprite("ZoteKey");
            tag.Message = "RandoSupplementalMetadata";
            return tag;
        }
        private InteropTag CurseTag()
        {
            InteropTag tag = new();
            tag.Properties["CanMimic"] = new BoxedBool(true);
            tag.Properties["MimicNames"] = new string[] {"Zone Key", "Zotte Key", "Invincible fearless etc key", "Z o t e"};
            tag.Message = "CurseData";
            return tag;
        }
    }
}