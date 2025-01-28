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
                    lmb.DoSubst(new("MoreDoors-Core_Key-Mantis_Vault_Guardian", "Defeated_Mantis_Lords", "Mantis_Respect"));
            }
        }

        private static void ApplyLogic(GenerationSettings gs, LogicManagerBuilder lmb)
        {
            if (!AccessManager.Settings.Enabled)
                return;
            
            if (AccessManager.Settings.MantisRespect)
            {
                lmb.AddItem(new BoolItem("Mantis_Respect", lmb.GetOrAddTerm("Mantis_Respect", TermType.SignedByte)));
                lmb.AddLogicDef(new("Mantis_Respect", "Defeated_Mantis_Lords"));

                // Access to Deepnest, storage room, bench and items now requires RESPECT instead of defeating the lords.
                lmb.DoSubst(new("Fungus2_15[left1]", "Defeated_Mantis_Lords", "Mantis_Respect"));
                lmb.DoSubst(new("Fungus2_25[right1]", "Defeated_Mantis_Lords", "Mantis_Respect"));
                lmb.DoSubst(new("Fungus2_31[left1]", "Defeated_Mantis_Lords", "Mantis_Respect"));
                lmb.DoSubst(new("Bench-Mantis_Village", "Defeated_Mantis_Lords", "Mantis_Respect"));
                lmb.DoSubst(new("Mark_of_Pride", "Defeated_Mantis_Lords", "Mantis_Respect"));
                lmb.DoSubst(new("Geo_Chest-Mantis_Lords", "Defeated_Mantis_Lords", "Mantis_Respect"));
                lmb.DoSubst(new("Hallownest_Seal-Mantis_Lords", "Defeated_Mantis_Lords", "Mantis_Respect"));
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
                lmb.GetOrAddTerm("Graveyard_Key", TermType.SignedByte);
                lmb.GetOrAddTerm("Waterways_Key", TermType.SignedByte);
                lmb.GetOrAddTerm("Pleasure_Key", TermType.SignedByte);
                lmb.GetOrAddTerm("Coffin_Key", TermType.SignedByte);
                lmb.AddItem(new StringItemTemplate("Graveyard_Key", "SIMPLE++ >> Graveyard_Key++"));
                lmb.AddItem(new StringItemTemplate("Waterways_Key", "SIMPLE++ >> Waterways_Key++"));
                lmb.AddItem(new StringItemTemplate("Pleasure_Key", "SIMPLE++ >> Pleasure_Key++"));
                lmb.AddItem(new StringItemTemplate("Coffin_Key", "SIMPLE++ >> Coffin_Key++"));
                lmb.DoMacroEdit(new("JIJIUNLOCK", "Graveyard_Key"));
                lmb.DoMacroEdit(new("WATERWAYSUNLOCK", "Waterways_Key"));
                lmb.DoMacroEdit(new("PLEASUREHOUSEUNLOCK", "Pleasure_Key"));
                lmb.DoMacroEdit(new("GODTUNERUNLOCK", "Coffin_Key"));
            }

            if (AccessManager.Settings.CustomKeys.MapperKey)
            {
                lmb.GetOrAddTerm("Mapper_Key", TermType.SignedByte);
                lmb.AddItem(new StringItemTemplate("Mapper_Key", "Mapper_Key++"));
                lmb.AddLogicDef(new("Mapper_Key", "Crossroads_33"));
                lmb.DoLogicEdit(new("Town[door_mapper]", "Town[door_mapper] | Town + Mapper_Key"));
            }

            if (AccessManager.Settings.CustomKeys.SlyKey)
            {
                lmb.GetOrAddTerm("Sly_Key", TermType.SignedByte);
                lmb.AddItem(new StringItemTemplate("Sly_Key", "Sly_Key++"));
                lmb.AddLogicDef(new("Sly_Key", "Crossroads_04[door1] | Crossroads_04[door_Mender_House] | Crossroads_04[door_charmshop] | Crossroads_04[right1] | (Crossroads_04[left1] | Crossroads_04[top1]) + Defeated_Gruz_Mother"));
                lmb.DoLogicEdit(new("Crossroads_04[door1]", "Crossroads_04[door1] | ORIG + Sly_Key"));
            }

            if (AccessManager.Settings.CustomKeys.BrettaKey)
            {
                lmb.GetOrAddTerm("Bretta_Key", TermType.SignedByte);
                lmb.AddItem(new StringItemTemplate("Bretta_Key", "Bretta_Key++"));
                lmb.AddLogicDef(new("Bretta_Key", "Fungus2_23 + (FULLCLAW + FULLDASH | FULLCLAW + FULLSUPERDASH | LEFTCLAW + WINGS | RIGHTCLAW + ENEMYPOGOS + WINGS | COMPLEXSKIPS + FULLCLAW + $SHADESKIP[2HITS] + SPELLAIRSTALL + $CASTSPELL[1,1,before:ROOMSOUL] + $TAKEDAMAGE[2])"));
                lmb.DoSubst(new("Town[door_bretta]", "Rescued_Bretta", "Bretta_Key"));
            }

            if (AccessManager.Settings.CustomKeys.ZoteKey)
            {
                lmb.GetOrAddTerm("Zote_Key", TermType.SignedByte);
                lmb.AddItem(new StringItemTemplate("Zote_Key", "Zote_Key++"));
                lmb.AddLogicDef(new("Zote_Key", "Deepnest_33[top1]"));
                lmb.DoLogicEdit(new("Boss_Essence-Grey_Prince_Zote", "Room_Bretta[right1] + DREAMNAIL + Zote_Key + Defeated_Colosseum_Zote + Defeated_Grey_Prince_Zote"));
                lmb.DoLogicEdit(new("Defeated_Grey_Prince_Zote", "Room_Bretta[right1] + DREAMNAIL + Zote_Key + Defeated_Colosseum_Zote + COMBAT[Grey_Prince_Zote]"));
            }

            if (AccessManager.Settings.CustomKeys.RelicKey)
            {
                lmb.GetOrAddTerm("Relic_Key", TermType.SignedByte);
                lmb.AddItem(new StringItemTemplate("Relic_Key", "Relic_Key++"));
                lmb.AddLogicDef(new("Relic_Key", "Waterways_01[top1] | Waterways_01[right1] | Waterways_01 + (ANYCLAW | WINGS | ENEMYPOGOS)"));
                lmb.DoLogicEdit(new("Can_Visit_Lemm", "(Ruins1_05b[top1] | Ruins1_05b + Lever-City_Lemm?TRUE) + Relic_Key"));
            }

            if (AccessManager.Settings.CustomKeys.GladeKey)
            {
                lmb.GetOrAddTerm("Glade_Key", TermType.SignedByte);
                lmb.AddItem(new StringItemTemplate("Glade_Key", "Glade_Key++"));
                lmb.DoLogicEdit(new("Opened_Glade_Door", "RestingGrounds_05 + Glade_Key"));
            }

            if (AccessManager.Settings.SplitTram && gs.PoolSettings.Keys)
            {
                lmb.GetOrAddTerm("Upper_Tram_Pass", TermType.SignedByte);
                lmb.AddItem(new StringItemTemplate("Upper_Tram_Pass", "Upper_Tram_Pass++"));
                lmb.GetOrAddTerm("Lower_Tram_Pass", TermType.SignedByte);
                lmb.AddItem(new StringItemTemplate("Lower_Tram_Pass", "Lower_Tram_Pass++"));
                lmb.AddLogicDef(new RawLogicDef("Split_Tram_Pass", "*Tram_Pass"));

                lmb.DoSubst(new RawSubstDef("Upper_Tram", "TRAM", "Upper_Tram_Pass"));
                lmb.DoSubst(new RawSubstDef("Lower_Tram", "TRAM", "Lower_Tram_Pass"));
            }

            if (AccessManager.Settings.SplitElevator && gs.NoveltySettings.RandomizeElevatorPass)
            {
                lmb.GetOrAddTerm("Left_Elevator_Pass", TermType.SignedByte);
                lmb.AddItem(new StringItemTemplate("Left_Elevator_Pass", "Left_Elevator_Pass++"));
                lmb.GetOrAddTerm("Right_Elevator_Pass", TermType.SignedByte);
                lmb.AddItem(new StringItemTemplate("Right_Elevator_Pass", "Right_Elevator_Pass++"));
                lmb.AddLogicDef(new RawLogicDef("Split_Elevator_Pass", "(Ruins2_10b[left1] | Ruins2_10b[right1] | Ruins2_10b[right2] | Right_Elevator) + Can_Replenish_Geo"));

                lmb.DoSubst(new RawSubstDef("Left_Elevator", "Elevator_Pass", "Left_Elevator_Pass"));
                lmb.DoSubst(new RawSubstDef("Right_Elevator", "Elevator_Pass", "Right_Elevator_Pass"));
            }

            if (AccessManager.Settings.TrapBench)
            {
                lmb.GetOrAddTerm("Trap_Bench", TermType.SignedByte);
                lmb.AddItem(new StringItemTemplate("Trap_Bench", "Trap_Bench++"));
                lmb.AddLogicDef(new RawLogicDef("Trap_Bench", "Deepnest_Spider_Town[left1]"));

                // Objects inside Beast Den require either secret entrance access or the trap item
                string subst = "Deepnest_Spider_Town[left1] + (Trap_Bench | WINGS + (RIGHTCLAW + (LEFTDASH | LEFTSUPERDASH | FIREBALLSKIPS + (RIGHTFIREBALL | SCREAM) + $CASTSPELL[1]) | LEFTCLAW + RIGHTSUPERDASH) + Plank-Den_Secret_Entrance?TRUE)";
                
                lmb.DoSubst(new RawSubstDef("Herrah", "Deepnest_Spider_Town[left1]", subst));
                lmb.DoSubst(new RawSubstDef("Bench-Beast's_Den", "Deepnest_Spider_Town[left1]", subst));
                lmb.DoSubst(new RawSubstDef("Rancid_Egg-Beast's_Den", "Deepnest_Spider_Town[left1]", subst));
                lmb.DoSubst(new RawSubstDef("Hallownest_Seal-Beast's_Den", "Deepnest_Spider_Town[left1]", subst));
                lmb.DoSubst(new RawSubstDef("Grub-Beast's_Den", "Deepnest_Spider_Town[left1]", subst));
                lmb.DoSubst(new RawSubstDef("Geo_Rock-Beast's_Den_Above_Trilobite", "Deepnest_Spider_Town[left1]", subst));
                lmb.DoSubst(new RawSubstDef("Geo_Rock-Beast's_Den_After_Herrah", "Deepnest_Spider_Town[left1]", subst));
                lmb.DoSubst(new RawSubstDef("Geo_Rock-Beast's_Den_Below_Herrah", "Deepnest_Spider_Town[left1]", subst));
                lmb.DoSubst(new RawSubstDef("Geo_Rock-Beast's_Den_Below_Egg", "Deepnest_Spider_Town[left1]", subst));
                lmb.DoSubst(new RawSubstDef("Geo_Rock-Beast's_Den_Bottom", "Deepnest_Spider_Town[left1]", subst));
                lmb.DoSubst(new RawSubstDef("Soul_Totem-Beast's_Den", "Deepnest_Spider_Town[left1]", subst));

                if (lmb.LogicLookup.TryGetValue("Defeated_Any_Corpse_Creeper", out _))
                {
                    lmb.DoSubst(new RawSubstDef("Defeated_Any_Corpse_Creeper", "Deepnest_Spider_Town[left1]", subst));
                    lmb.DoSubst(new RawSubstDef("Defeated_Any_Deepling", "Deepnest_Spider_Town[left1]", subst));
                    lmb.DoSubst(new RawSubstDef("Defeated_Any_Deephunter", "Deepnest_Spider_Town[left1]", subst));
                    lmb.DoSubst(new RawSubstDef("Defeated_Any_Stalking_Devout", "Deepnest_Spider_Town[left1]", subst));
                }
            }
        }
    }
}