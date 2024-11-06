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
            PlayerData.instance.spiderCapture = false;
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
            tags = [TrapBenchItemTag(), CurseTag()];
        }
        private static Tag TrapBenchItemTag()
        {
            InteropTag tag = new();
            tag.Properties["ModSource"] = "AccessRandomizer";
            tag.Properties["PoolGroup"] = "Keys";
            tag.Message = "RandoSupplementalMetadata";
            return tag;
        }
        private InteropTag CurseTag()
        {
            InteropTag tag = new();
            tag.Properties["CanMimic"] = new BoxedBool(true);
            tag.Properties["MimicNames"] = new string[] {"Rap Bench", "Trap Becnh", "Tahm Kench"};
            tag.Message = "CurseData";
            return tag;
        }
    }
}