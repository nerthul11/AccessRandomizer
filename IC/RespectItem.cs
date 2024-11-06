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
            tags = [RespectTag(), CurseTag()];
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

        private InteropTag CurseTag()
        {
            InteropTag tag = new();
            tag.Properties["CanMimic"] = new BoxedBool(true);
            tag.Properties["MimicNames"] = new string[] {"Mant1s Respect", "Mantis Respekt", "Mantis' 'Respect'", "Mantis' Respect?'"};
            tag.Message = "CurseData";
            return tag;
        }
        
        public override bool Redundant() => PlayerData.instance.defeatedMantisLords;
        public override void GiveImmediate(GiveInfo info)
        {
            // Item: Set the defeatedMantisLords flag as true to gain respect.
            AccessModule.Instance.RespectObtained = true;
            PlayerData.instance.defeatedMantisLords = true;
            AccessModule.Instance.CompletedChallenges();
        }
    }
}