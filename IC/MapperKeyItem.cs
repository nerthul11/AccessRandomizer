using AccessRandomizer.Modules;
using ItemChanger;
using ItemChanger.Tags;
using ItemChanger.UIDefs;

namespace AccessRandomizer.IC
{
    public class MapperKeyItem : AbstractItem
    {
        public override bool Redundant() => AccessModule.Instance.UnlockedIselda;
        public override void GiveImmediate(GiveInfo info)
        {
            AccessModule.Instance.UnlockedIselda = true;
            AccessModule.Instance.CompletedChallenges();
        }

        public MapperKeyItem()
        {
            name = "Mapper_Key";
            UIDef = new MsgUIDef()
            {
                name = new BoxedString("Mapper Key"),
                shopDesc = new BoxedString("Now where did I leave this one? -Cornifer"),
                sprite = new AccessSprite("MapperKey")
            };
            tags = [MapperTag()];
        }
        private static Tag MapperTag()
        {
            InteropTag tag = new();
            tag.Properties["ModSource"] = "AccessRandomizer";
            tag.Properties["PoolGroup"] = "Keys";
            tag.Properties["PinSprite"] = new AccessSprite("MapperKey");
            tag.Message = "RandoSupplementalMetadata";
            return tag;
        }
    }
}