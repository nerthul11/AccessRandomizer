using ItemChanger;
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
                shopDesc = new BoxedString("Well fought."),
                sprite = new AccessSprite("Respect")
            };
        }
        
        public override void GiveImmediate(GiveInfo info)
        {
            // Item: Set the defeatedMantisLords flag as true to gain respect.
            PlayerData.instance.defeatedMantisLords = true;
        }
    }
}