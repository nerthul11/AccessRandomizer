using System;
using AccessRandomizer.Modules;
using ItemChanger;
using ItemChanger.Tags;
using ItemChanger.UIDefs;

namespace AccessRandomizer.IC
{
    public class GateItem : AbstractItem
    {
        public string gate;
        public GateItem(string gate)
        {
            name = $"Shade_Gate-{gate}";
            gate = gate.Replace("_", "");
            UIDef = new MsgUIDef()
            {
                name = new BoxedString($"Shade Gate - {gate.Replace('_', ' ')}"),
                shopDesc = new BoxedString(""),
                sprite = new AccessSprite("Gate"),
                
            };
            tags = [ItemTag()];
        }

        private InteropTag ItemTag()
        {
            InteropTag tag = new();
            tag.Properties["ModSource"] = "AccessRandomizer";
            tag.Properties["PoolGroup"] = "Keys";
            tag.Properties["PinSprite"] = new AccessSprite("Gate");
            tag.Message = "RandoSupplementalMetadata";
            return tag;
        }

        public override bool Redundant() => AccessModule.Instance.ShadeGates.GetVariable<bool>(gate) == true;
        public override void GiveImmediate(GiveInfo info) 
        {
            AccessModule module = AccessModule.Instance;
            module.ShadeGates.SetVariable(gate, true);
            module.CompletedChallenges();
        }
    }

    public class ShadeGates
    {
        public bool Birthplace { get; set; }
        public bool FogCanyon { get; set; }
        public bool Markoth { get; set; }
        public bool OvergrownMound { get; set; }
        public bool SharpShadow { get; set; }
        public bool TraitorLord { get; set; }
        public bool VoidTendrils { get; set; }

        public T GetVariable<T>(string propertyName) {
            var property = typeof(ShadeGates).GetProperty(propertyName);
            if (property == null) {
                throw new ArgumentException($"Property '{propertyName}' not found in ShadeGates class.");
            }
            return (T)property.GetValue(this);
        }

        public void SetVariable<T>(string propertyName, T value) {
            var property = typeof(ShadeGates).GetProperty(propertyName);
            if (property == null) {
                throw new ArgumentException($"Property '{propertyName}' not found in ShadeGates class.");
            }
            property.SetValue(this, value);
        }
    }
}