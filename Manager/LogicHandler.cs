using System;
using System.Linq;
using Modding;
using RandomizerCore.Logic;
using RandomizerCore.LogicItems;
using RandomizerCore.StringItems;
using RandomizerMod.RC;
using RandomizerMod.Settings;


namespace AccessRandomizer.Manager
{
    public class LogicHandler
    {
        public static void Hook()
        {
            RCData.RuntimeLogicOverride.Subscribe(11f, ApplyLogic);
            if (ModHooks.GetMod("MoreDoors") is Mod)
                RCData.RuntimeLogicOverride.Subscribe(128f, MoreDoorsInterop);
        }

        private static void MoreDoorsInterop(GenerationSettings gs, LogicManagerBuilder lmb)
        {
            if (!AccessManager.Settings.Enabled)
                return;

            if (AccessManager.Settings.MantisRespect)
            {
                bool isActive = lmb.LogicLookup.TryGetValue("MoreDoors-Core_Key-Mantis_Vault_Guardian", out _);
                if (isActive)
                    lmb.DoSubst(new("MoreDoors-Core_Key-Mantis_Vault_Guardian", "Defeated_Mantis_Lords", "RESPECT"));
            }
        }

        private static void ApplyLogic(GenerationSettings gs, LogicManagerBuilder lmb)
        {
            if (!AccessManager.Settings.Enabled)
                return;
            
            if (AccessManager.Settings.MantisRespect)
            {
                lmb.AddItem(new BoolItem("Mantis_Respect", lmb.GetOrAddTerm("RESPECT", TermType.SignedByte)));
                lmb.AddLogicDef(new("Mantis_Respect", "Defeated_Mantis_Lords"));

                // Access to Deepnest, storage room, bench and items now requires RESPECT instead of defeating the lords.
                lmb.DoSubst(new("Fungus2_15[left1]", "Defeated_Mantis_Lords", "RESPECT"));
                lmb.DoSubst(new("Fungus2_25[right1]", "Defeated_Mantis_Lords", "RESPECT"));
                lmb.DoSubst(new("Fungus2_31[left1]", "Defeated_Mantis_Lords", "RESPECT"));
                lmb.DoSubst(new("Bench-Mantis_Village", "Defeated_Mantis_Lords", "RESPECT"));
                lmb.DoSubst(new("Mark_of_Pride", "Defeated_Mantis_Lords", "RESPECT"));
                lmb.DoSubst(new("Geo_Chest-Mantis_Lords", "Defeated_Mantis_Lords", "RESPECT"));
                lmb.DoSubst(new("Hallownest_Seal-Mantis_Lords", "Defeated_Mantis_Lords", "RESPECT"));
            }

            if (AccessManager.Settings.HollowKnightChains)
            {
                lmb.GetOrAddTerm("CHAINS", TermType.Int);
                lmb.AddItem(new StringItemTemplate("Hollow_Knight_Chain", "CHAINS++"));
                foreach (int i in Enumerable.Range(1, 4))
                {
                    lmb.AddLogicDef(new($"Hollow_Knight_Chain-{i}", $"Opened_Black_Egg_Temple + CHAINS>{i-1}"));
                }
                if (lmb.LogicLookup.TryGetValue("Defeated_Any_Hollow_Knight", out _))
                    lmb.DoLogicEdit(new("Defeated_Any_Hollow_Knight", "ORIG + CHAINS>3"));
            }

            if (AccessManager.Settings.UniqueKeys && gs.PoolSettings.Keys)
            {
                lmb.GetOrAddTerm("GRAVEYARDKEY", TermType.SignedByte);
                lmb.GetOrAddTerm("WATERWAYSKEY", TermType.SignedByte);
                lmb.GetOrAddTerm("PLEASUREKEY", TermType.SignedByte);
                lmb.GetOrAddTerm("COFFINKEY", TermType.SignedByte);
                lmb.AddItem(new StringItemTemplate("Graveyard_Key", "SIMPLE++ >> GRAVEYARDKEY++"));
                lmb.AddItem(new StringItemTemplate("Waterways_Key", "SIMPLE++ >> WATERWAYSKEY++"));
                lmb.AddItem(new StringItemTemplate("Pleasure_Key", "SIMPLE++ >> PLEASUREKEY++"));
                lmb.AddItem(new StringItemTemplate("Coffin_Key", "SIMPLE++ >> COFFINKEY++"));
                lmb.DoMacroEdit(new("JIJIUNLOCK", "GRAVEYARDKEY"));
                lmb.DoMacroEdit(new("WATERWAYSUNLOCK", "WATERWAYSKEY"));
                lmb.DoMacroEdit(new("PLEASUREHOUSEUNLOCK", "PLEASUREKEY"));
                lmb.DoMacroEdit(new("GODTUNERUNLOCK", "COFFINKEY"));
            }

            if (AccessManager.Settings.MapperKey)
            {
                lmb.GetOrAddTerm("MAPPERKEY", TermType.SignedByte);
                lmb.AddItem(new StringItemTemplate("Mapper_Key", "MAPPERKEY++"));
                lmb.AddLogicDef(new("Mapper_Key", "Crossroads_33"));
                lmb.DoLogicEdit(new("Town[door_mapper]", "Town[door_mapper] | Town + MAPPERKEY"));
            }
        }
    }
}