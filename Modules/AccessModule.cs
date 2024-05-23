using System;
using System.Collections.Generic;
using AccessRandomizer.Manager;
using ItemChanger;

namespace AccessRandomizer.Modules
{
    public class AccessModule : ItemChanger.Modules.Module
    {
        public SaveSettings Settings { get; set; } = new();
        public class SaveSettings 
        {
            public bool MantisRespect { get; set; } = AccessManager.Settings.Enabled && AccessManager.Settings.MantisRespect;
            public bool HollowKnightChains { get; set; } = AccessManager.Settings.Enabled && AccessManager.Settings.HollowKnightChains;
            public bool UniqueKeys { get; set; } = AccessManager.Settings.Enabled && AccessManager.Settings.UniqueKeys;
        }   
        public int ChainsBroken { get; set; } = 0;   
        public bool GraveyardKey { get; set;} = false;
        public bool WaterwaysKey { get; set;} = false;
        public bool PleasureKey { get; set;} = false;
        public bool CoffinKey { get; set;} = false;
        public static AccessModule Instance => ItemChangerMod.Modules.GetOrAdd<AccessModule>();
        public override void Initialize() 
        {
            On.PlayerData.SetBool += Refresh;
        }

        public override void Unload()
        {
            On.PlayerData.SetBool -= Refresh;
        }
        public delegate void AccessObtained(List<string> marks);
        public event AccessObtained OnAccessObtained;

        private void Refresh(On.PlayerData.orig_SetBool orig, PlayerData self, string boolName, bool value)
        {
            orig(self, boolName, value);
            if (!Settings.MantisRespect && boolName == "defeatedMantisLords")
                CompletedChallenges();
        }
        public void CompletedChallenges()
        {
            List<string> completed = [];

            // Check for item state if randomized
            if (PlayerData.instance.defeatedMantisLords)
                completed.Add("Mantis Respect");
            
            if (CoffinKey)
                completed.Add("Coffin Key");
            if (GraveyardKey)
                completed.Add("Graveyard Key");
            if (PleasureKey)
                completed.Add("Pleasure Key");
            if (WaterwaysKey)
                completed.Add("Waterways Key");
            
            if (ChainsBroken >= 1)
                completed.Add("First Chain");
            if (ChainsBroken >= 2)
                completed.Add("Second Chain");
            if (ChainsBroken >= 3)
                completed.Add("Third Chain");
            if (ChainsBroken >= 4)
                completed.Add("Fourth Chain");
            OnAccessObtained?.Invoke(completed);
        }

        public T GetVariable<T>(string propertyName) {
            var property = typeof(AccessModule).GetProperty(propertyName);
            if (property == null) {
                throw new ArgumentException($"Property '{propertyName}' not found in KeyModule class.");
            }
            return (T)property.GetValue(this);
        }

        public void SetVariable<T>(string propertyName, T value) {
            var property = typeof(AccessModule).GetProperty(propertyName);
            if (property == null) {
                throw new ArgumentException($"Property '{propertyName}' not found in KeyModule class.");
            }
            property.SetValue(this, value);
        }
    }
}