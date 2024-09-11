using AccessRandomizer.Modules;
using ItemChanger;
using ItemChanger.Tags;
using ItemChanger.UIDefs;

namespace AccessRandomizer.IC
{
    public class TrapBenchItem : AbstractItem
    {
        public override bool Redundant() => !PlayerData.instance.spiderCapture;
        public override void GiveImmediate(GiveInfo info)
        {
            AccessModule.Instance.TrapBench = true;
            AccessModule.Instance.CompletedChallenges();
        }

        public TrapBenchItem()
        {
            name = "Trap_Bench";
            UIDef = new MsgUIDef()
            {
                name = new BoxedString("Trap Bench"),
                shopDesc = new BoxedString("Rest here... We're goooooooood frieeeeeeeeends... ... ..."),
                sprite = new ItemChangerSprite("ShopIcons.BenchPin")
            };
            tags = [TrapBenchItemTag()];
        }
        private static Tag TrapBenchItemTag()
        {
            InteropTag tag = new();
            tag.Properties["ModSource"] = "AccessRandomizer";
            tag.Properties["PoolGroup"] = "Keys";
            tag.Message = "RandoSupplementalMetadata";
            return tag;
        }
    }
}