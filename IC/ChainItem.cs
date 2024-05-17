using AccessRandomizer.Modules;
using ItemChanger;
using ItemChanger.Tags;
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
                name = new BoxedString("Hollow Knight Chain"),
                shopDesc = new BoxedString("Hear the noise of broken chains."),
                sprite = new AccessSprite("Chain"),
                
            };
            tags = [ChainTag()];
        }

        private InteropTag ChainTag()
        {
            InteropTag tag = new();
            tag.Properties["ModSource"] = "AccessRandomizer";
            tag.Properties["PoolGroup"] = "Dreamers";
            tag.Properties["PinSprite"] = new AccessSprite("Chain");
            tag.Message = "RandoSupplementalMetadata";
            return tag;
        }

        public override bool Redundant() => AccessModule.Instance.ChainsBroken >= 5;
        public override void GiveImmediate(GiveInfo info) 
        {
            AccessModule module = AccessModule.Instance;
            module.ChainsBroken += 1;
            int current = module.ChainsBroken;
            if (UIDef is MsgUIDef ui && !Redundant())
                ui.name = new BoxedString($"{ui.name.Value} (#{current})");
            module.CompletedChallenges();
        }
    }
}