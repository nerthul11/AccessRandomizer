using System;
using AccessRandomizer.Fsm;
using AccessRandomizer.Manager;
using AccessRandomizer.Settings;
using ItemChanger;
using Modding;
using Satchel;

namespace AccessRandomizer.IC
{
    public class KeyItem : AbstractItem
    {
        public string doorScene;       
        public string keyName; 
        public string objectName;
        public string fsmName;
        public string fsmState;
        public string fsmTrueEvent;
        public string fsmFalseEvent;
        public string dialogueName;
        public string previewWord;
        public override bool Redundant() => AccessManager.SaveSettings.GetVariable<bool>(keyName);
        public override void GiveImmediate(GiveInfo info) 
        {
            AccessManager.SaveSettings.SetVariable(keyName, true);
        }

        protected override void OnUnload()
        {
            Events.RemoveFsmEdit(doorScene, new(objectName, fsmName), DoorOverride);
            Events.RemoveLanguageEdit(new LanguageKey(name == "Coffin_Key" ? "CP3" : "Prompts", $"{dialogueName}_KEY"), InsertKeyPreview);
            Events.RemoveLanguageEdit(new LanguageKey(name == "Coffin_Key" ? "CP3" : "Prompts", $"{dialogueName}_NOKEY"), NoKeyPreview);
        }

        protected override void OnLoad()
        {
            Events.AddFsmEdit(doorScene, new(objectName, fsmName), DoorOverride);
            if (name == "Coffin_Key")
                ModHooks.LanguageGetHook += EditCoffin;
            Events.AddLanguageEdit(new LanguageKey(name == "Coffin_Key" ? "CP3" : "Prompts", $"{dialogueName}_KEY"), InsertKeyPreview);
            Events.AddLanguageEdit(new LanguageKey(name == "Coffin_Key" ? "CP3" : "Prompts", $"{dialogueName}_NOKEY"), NoKeyPreview);
            Events.AddLanguageEdit(new LanguageKey("UI", "INV_NAME_SIMPLEKEY"), KeyTrackerName);
            Events.AddLanguageEdit(new LanguageKey("UI", "INV_DESC_SIMPLEKEY"), KeyTrackerDesc);
            On.PlayerData.SetBool += AlwaysOneKey;
        }

        private void AlwaysOneKey(On.PlayerData.orig_SetBool orig, PlayerData self, string boolName, bool value)
        {
            orig(self, boolName, value);
            PlayerData.instance.simpleKeys = 1;
            PlayerData.instance.hasWaterwaysKey = true;
            PlayerData.instance.hasSpaKey = true;
        }

        private void KeyTrackerName(ref string value)
        {
            value = "Key Tracker";
        }

        private void KeyTrackerDesc(ref string value)
        {
            value = "A tracker for keys that open a few Hallownest spots.";
            LocalSettings state = AccessManager.SaveSettings;
            bool anyKey = state.CoffinKey || state.GraveyardKey || state.PleasureKey || state.WaterwaysKey;
            if (anyKey)
                value += "<br>The following keys have been obtained:";
            else
                value += "<br>No keys have been obtained yet.";
            if (state.CoffinKey)
            {
                value += "<br>-Coffin Key.";
                if (PlayerData.instance.godseekerUnlocked)
                    value += " (used)";
            }
            if (state.GraveyardKey)
            {
                value += "<br>-Graveyard Key.";
                if (PlayerData.instance.jijiDoorUnlocked)
                    value += " (used)";
            }
            if (state.PleasureKey)
            {
                value += "<br>-Pleasure Key.";
                if (PlayerData.instance.bathHouseOpened)
                    value += " (used)";
            }
            if (state.WaterwaysKey)
            {
                value += "<br>-Waterways Key.";
                if (PlayerData.instance.openedWaterwaysManhole)
                    value += " (used)";
            }
        }

        private string EditCoffin(string key, string sheetTitle, string orig)
        {
            if (key == "GODSEEKER_COFFIN_KEY")
            {
                orig = orig.Replace("simple key", $"{name.Replace('_', ' ')}");
            }
            else if (key == "GODSEEKER_COFFIN_NOKEY")
            {
                orig = orig.Replace("simple", previewWord);
            }
            
            return orig;
        }

        private void InsertKeyPreview(ref string value)
        {
            AccessRandomizer.Instance.Log(value);
            value = value.Replace("a Simple Key", $"the {name.Replace('_', ' ')}");
            value = value.Replace("simple", previewWord);
        }

        private void NoKeyPreview(ref string value)
        {
            AccessRandomizer.Instance.Log(value);
            value = value.Replace("simple", previewWord);
        }
        
        private void DoorOverride(PlayMakerFSM fsm)
        {
            PlayerData.instance.hasWaterwaysKey = true;
            PlayerData.instance.hasSpaKey = true;
            fsm.AddFirstAction(fsmState, new KeyBooleanFsmCheck(keyName, fsmTrueEvent, fsmFalseEvent));
            if (keyName == "Pleasure_Key")
                fsm.AddCustomAction("Open", () => PlayerData.instance.simpleKeys = 1);
        }
    }
}