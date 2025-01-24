using AccessRandomizer.Modules;
using ItemChanger;
using ItemChanger.Tags;
using ItemChanger.UIDefs;

namespace AccessRandomizer.IC
{
    public class BrettaKeyItem : AbstractItem
    {
        public override bool Redundant() => AccessModule.Instance.UnlockedBretta;
        public override void GiveImmediate(GiveInfo info)
        {
            AccessModule.Instance.UnlockedBretta = true;
            AccessModule.Instance.CompletedChallenges();
        }

        public BrettaKeyItem()
        {
            name = "Bretta_Key";
            UIDef = new MsgUIDef()
            {
                name = new BoxedString("Bretta Key"),
                shopDesc = new BoxedString("Now where did I leave this one? -Cornifer"),
                sprite = new AccessSprite("BrettaKey")
            };
            tags = [ItemTag(), CurseTag()];
        }
        private static Tag ItemTag()
        {
            InteropTag tag = new();
            tag.Properties["ModSource"] = "AccessRandomizer";
            tag.Properties["PoolGroup"] = "Keys";
            tag.Properties["PinSprite"] = new AccessSprite("BrettaKey");
            tag.Message = "RandoSupplementalMetadata";
            return tag;
        }
        private InteropTag CurseTag()
        {
            InteropTag tag = new();
            tag.Properties["CanMimic"] = new BoxedBool(true);
            tag.Properties["MimicNames"] = new string[] {"Brettord Key", "Greta Key", "Brreta Key", "Baretta Key"};
            tag.Message = "CurseData";
            return tag;
        }
    }
}