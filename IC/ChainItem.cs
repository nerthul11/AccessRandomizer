using AccessRandomizer.Manager;
using ItemChanger;
using ItemChanger.UIDefs;

namespace AccessRandomizer.IC
{
    public class ChainItem : AbstractItem
    {
        public ChainItem()
        {
            name = "Hollow_Knight_Chain";
            UIDef = new MsgUIDef()
            {
                name = new BoxedString("Hollow Knight Binding Chain"),
                shopDesc = new BoxedString("Hear the noise of broken chains."),
                sprite = new AccessSprite("Chain")
            };
        }
        
        public override void GiveImmediate(GiveInfo info) 
        {
            AccessManager.SaveSettings.ChainsBroken += 1;
        }
    }
}