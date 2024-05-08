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
                shopDesc = new BoxedString("Bow before the Knight."),
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