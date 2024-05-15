using AccessRandomizer.Modules;
using ItemChanger;
using ItemChanger.Tags;
using ItemChanger.UIDefs;

namespace AccessRandomizer.IC
{
    public class RespectItem : AbstractItem
    {
        public RespectItem()
        {
            name = "Mantis_Respect";
            UIDef = new MsgUIDef()
            {
                name = new BoxedString("Mantis' Respect"),
                shopDesc = new BoxedString("Bow before the Knight."),
                sprite = new AccessSprite("Respect")
            };
            tags = [RespectTag()];
        }
        private static Tag RespectTag()
        {
            InteropTag tag = new();
            tag.Properties["ModSource"] = "AccessRandomizer";
            tag.Properties["PoolGroup"] = "Keys";
            tag.Properties["PinSprite"] = new AccessSprite("Respect");
            tag.Message = "RandoSupplementalMetadata";
            return tag;
        }
        
        public override void GiveImmediate(GiveInfo info)
        {
            // Item: Set the defeatedMantisLords flag as true to gain respect.
            PlayerData.instance.defeatedMantisLords = true;
            AccessModule.Instance.CompletedChallenges();
        }
    }
}