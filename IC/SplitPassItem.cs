using AccessRandomizer.Modules;
using ItemChanger;

namespace AccessRandomizer.IC
{
    public class SplitPassItem : AbstractItem
    {
        public string passType;
        public string passSide;
        public override bool Redundant() => AccessModule.Instance.GetVariable<bool>($"{passSide}{passType}");
        public override void GiveImmediate(GiveInfo info)
        {
            AccessModule.Instance.SetVariable($"{passSide}{passType}", true);
            if (passType == "Tram")
                PlayerData.instance.hasTramPass = true; // Make it appear on the inventory
            AccessModule.Instance.CompletedChallenges();
        }
    }
}