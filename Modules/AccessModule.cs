using System;
using System.Collections.Generic;
using System.Text;
using AccessRandomizer.Manager;
using ItemChanger;
using ItemChanger.Modules;

namespace AccessRandomizer.Modules
{
    public class AccessModule : Module
    {
        public SaveSettings Settings { get; set; } = new();
        public class SaveSettings 
        {
            public bool MantisRespect { get; set; } = AccessManager.Settings.Enabled && AccessManager.Settings.MantisRespect && RandomizerMod.RandomizerMod.IsRandoSave;
            public bool HollowKnightChains { get; set; } = AccessManager.Settings.Enabled && AccessManager.Settings.HollowKnightChains && RandomizerMod.RandomizerMod.IsRandoSave;
            public bool UniqueKeys { get; set; } = AccessManager.Settings.Enabled && AccessManager.Settings.UniqueKeys && RandomizerMod.RandomizerMod.IsRandoSave;
            public bool MapperKey { get; set; } = AccessManager.Settings.Enabled && AccessManager.Settings.MapperKey && RandomizerMod.RandomizerMod.IsRandoSave;
            public bool GladeAccess { get; set; } = AccessManager.Settings.Enabled && AccessManager.Settings.GladeAccess && RandomizerMod.RandomizerMod.IsRandoSave;
            public bool SplitTram { get; set; } = AccessManager.Settings.Enabled && AccessManager.Settings.SplitTram && RandomizerMod.RandomizerMod.IsRandoSave;
            public bool SplitElevator { get; set; } = AccessManager.Settings.Enabled && AccessManager.Settings.SplitElevator && RandomizerMod.RandomizerMod.IsRandoSave;
            public bool TrapBench { get; set; } = AccessManager.Settings.Enabled && AccessManager.Settings.TrapBench && RandomizerMod.RandomizerMod.IsRandoSave;
            public bool RelicKey { get; set; } = AccessManager.Settings.Enabled && AccessManager.Settings.RelicKey && RandomizerMod.RandomizerMod.IsRandoSave;
        }   
        public bool RespectObtained { get; set; } = false;
        public int ChainsBroken { get; set; } = 0;   
        public bool GraveyardKey { get; set; } = false;
        public bool WaterwaysKey { get; set; } = false;
        public bool PleasureKey { get; set; } = false;
        public bool CoffinKey { get; set; } = false;
        public bool UnlockedIselda { get; set; } = false;
        public bool UpperTram { get; set; } = false;
        public bool LowerTram { get; set; } = false;
        public bool LeftElevator { get; set; } = false;
        public bool RightElevator { get; set; } = false;
        public bool TrapBench { get; set; } = false;
        public bool RelicKey { get; set; } = false;
        public static AccessModule Instance => ItemChangerMod.Modules.GetOrAdd<AccessModule>();
        public override void Initialize() 
        {
            On.PlayerData.SetBool += Refresh;
            On.GameManager.BeginSceneTransition += ForceBools;
            if (Settings.MapperKey)
            {
                PlayerData.instance.openedMapperShop = UnlockedIselda;
            }
            if (Settings.SplitElevator)
            {
                if (ItemChangerMod.Modules?.Get<InventoryTracker>() is InventoryTracker it)
                    it.OnGenerateFocusDesc += ElevatorDesc;
            }
            if (Settings.SplitTram)
            {
                Events.AddLanguageEdit(new LanguageKey("UI", "INV_NAME_TRAM_PASS"), TramName);
                Events.AddLanguageEdit(new LanguageKey("UI", "INV_DESC_TRAM_PASS"), TramDesc);
            }
            if (Settings.TrapBench)
            {
                PlayerData.instance.spiderCapture = !TrapBench;
            }
        }

        public override void Unload()
        {
            On.PlayerData.SetBool -= Refresh;
            On.GameManager.BeginSceneTransition -= ForceBools;
            Events.RemoveLanguageEdit(new LanguageKey("UI", "INV_NAME_TRAM_PASS"), TramName);
            Events.RemoveLanguageEdit(new LanguageKey("UI", "INV_DESC_TRAM_PASS"), TramDesc);
            if (ItemChangerMod.Modules?.Get<InventoryTracker>() is InventoryTracker it)
                it.OnGenerateFocusDesc -= ElevatorDesc;
        }

        private void TramName(ref string value)
        {
            if (UpperTram && LowerTram)
                return;
            
            if (UpperTram)
                value = "Upper Tram Pass";
            if (LowerTram)
                value += "Lower Tram Pass";
        }

        private void TramDesc(ref string value)
        {
            if (UpperTram && LowerTram)
                return;
            
            value = "You've got a ticket to ride. But it only works for the ";
            if (UpperTram)
                value += "Upper Tram.";
            if (LowerTram)
                value += "Lower Tram.";
        }

        private void ElevatorDesc(StringBuilder builder)
        {
            if (LeftElevator && RightElevator)
                builder.AppendLine("You can ride large elevators.");
            else if (LeftElevator)
                builder.AppendLine("You can ride the left large elevator.");
            else if (RightElevator)
                builder.AppendLine("You can ride the right large elevator.");
            else
                builder.AppendLine("You cannot ride large elevators.");
        }

        private void ForceBools(On.GameManager.orig_BeginSceneTransition orig, GameManager self, GameManager.SceneLoadInfo info)
        {
            orig(self, info);

            // Unlock the shop door if accessed through its gate via Room Rando.
            if (info.SceneName == SceneNames.Town && Settings.MapperKey)
                PlayerData.instance.openedMapperShop = UnlockedIselda || info.EntryGateName == "door_mapper";

            // Lock/Unlock elevators based on the Split pass items.
            if (info.SceneName == SceneNames.Crossroads_49 && Settings.SplitElevator)
                PlayerData.instance.cityLift1 = LeftElevator;
            if (info.SceneName == SceneNames.Crossroads_49b && Settings.SplitElevator)
                PlayerData.instance.cityLift1 = LeftElevator;
            if (info.SceneName == SceneNames.Ruins2_10 && Settings.SplitElevator)
                PlayerData.instance.cityLift2 = RightElevator;
            if (info.SceneName == SceneNames.Ruins2_10b && Settings.SplitElevator)
                PlayerData.instance.cityLift2 = RightElevator;
            
            // Activate/Deactivate Beast Den bench based on Trap Bench item.
            if (info.SceneName == SceneNames.Deepnest_Spider_Town && Settings.TrapBench)
                PlayerData.instance.spiderCapture = !TrapBench;
        }
        public delegate void AccessObtained(List<string> marks);
        public event AccessObtained OnAccessObtained;

        private void Refresh(On.PlayerData.orig_SetBool orig, PlayerData self, string boolName, bool value)
        {
            orig(self, boolName, value);
            if (!Settings.MantisRespect && boolName == "defeatedMantisLords")
                CompletedChallenges();
            if (!Settings.GladeAccess && boolName == "gladeDoorOpened")
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
            
            if (UnlockedIselda && Settings.MapperKey)
                completed.Add("Mapper Key");
            
            if (PlayerData.instance.gladeDoorOpened)
                completed.Add("Glade Access");

            if (UpperTram)
                completed.Add("Upper Tram Pass");
            if (LowerTram)
                completed.Add("Lower Tram Pass");
            if (LeftElevator)
                completed.Add("Left Elevator Pass");
            if (RightElevator)
                completed.Add("Right Elevator Pass");
            
            if (TrapBench)
                completed.Add("Beast Den Access");

            if (RelicKey)
                completed.Add("Relic Key");
            
            OnAccessObtained?.Invoke(completed);
        }

        public T GetVariable<T>(string propertyName) {
            var property = typeof(AccessModule).GetProperty(propertyName) ?? throw new ArgumentException($"Property '{propertyName}' not found in AccessModule class.");
            return (T)property.GetValue(this);
        }

        public void SetVariable<T>(string propertyName, T value) {
            var property = typeof(AccessModule).GetProperty(propertyName) ?? throw new ArgumentException($"Property '{propertyName}' not found in AccessModule class.");
            property.SetValue(this, value);
        }
    }
}