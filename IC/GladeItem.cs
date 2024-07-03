using AccessRandomizer.Modules;
using ItemChanger;
using ItemChanger.Tags;
using ItemChanger.UIDefs;

namespace AccessRandomizer.IC
{
    public class GladeItem : AbstractItem
    {
        public override bool Redundant() => PlayerData.instance.gladeDoorOpened;
        public override void GiveImmediate(GiveInfo info)
        {
            PlayerData.instance.gladeDoorOpened = true;
            AccessModule.Instance.CompletedChallenges();
        }

        public GladeItem()
        {
            name = "Glade_Key";
            UIDef = new MsgUIDef()
            {
                name = new BoxedString("Glade Key"),
                shopDesc = new BoxedString("Opens the way to pay respect to those who passed. It does resemble a Dream Nail."),
                sprite = new AccessSprite("GladeKey")
            };
            tags = [GladeItemTag()];
        }
        private static Tag GladeItemTag()
        {
            InteropTag tag = new();
            tag.Properties["ModSource"] = "AccessRandomizer";
            tag.Properties["PoolGroup"] = "Keys";
            tag.Properties["PinSprite"] = new AccessSprite("GladeKey");
            tag.Message = "RandoSupplementalMetadata";
            return tag;
        }
    }
}