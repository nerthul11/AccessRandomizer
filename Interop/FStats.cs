using AccessRandomizer.Manager;
using AccessRandomizer.Modules;
using FStats;
using FStats.StatControllers;
using FStats.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccessRandomizer.Interop
{
    public static class FStats_Interop
    {
        public static void Hook()
        {
            API.OnGenerateFile += GenerateStats;
        }

        private static void GenerateStats(Action<StatController> generateStats)
        {
            if (!AccessManager.Settings.Enabled)
                return;

            generateStats(new AccessStats());
        }
    }

    public class AccessStats : StatController
    {
        public override void Initialize() 
        {
            AccessModule.Instance.OnAccessObtained += AddMarks;
        }
        public override void Unload() 
        {
            AccessModule.Instance.OnAccessObtained -= AddMarks;
        }

        private void AddMarks(List<string> marks)
        {
            foreach (string mark in marks)
                if (!OneTimeCheck.Contains(mark))
                {
                    OneTimeCheck.Add(mark);
                    AccessMarks.Add(new AccessMark(mark, FStatsMod.LS.Get<Common>().CountedTime));
                }
        }

        public record AccessMark(string Mark, float Timestamp);
        public List<AccessMark> AccessMarks = [];
        public List<string> OneTimeCheck = [];

        public override IEnumerable<DisplayInfo> GetDisplayInfos()
        {
            List<string> rows = AccessMarks.OrderBy(x => x.Timestamp).Select(x => $"{x.Mark}: {x.Timestamp.PlaytimeHHMMSS()}").ToList();
            AccessModule.SaveSettings settings = AccessModule.Instance.Settings;
            int rowTotal = 2;
            rowTotal += settings.HollowKnightChains ? 4 : 0;
            rowTotal += settings.UniqueKeys ? 4 : 0;
            rowTotal += settings.MapperKey ? 1 : 0;
            rowTotal += settings.SplitElevator ? 2 : 0;
            rowTotal += settings.SplitTram ? 2 : 0;            
            yield return new()
            {
                Title = "Access Randomizer Timeline",
                MainStat = $"{AccessMarks.Count} / {rowTotal}",
                StatColumns = Columnize(rows),
                Priority = BuiltinScreenPriorityValues.ExtensionStats
            };
        }
        private const int COL_SIZE = 10;

        private List<string> Columnize(List<string> rows)
        {
            int columnCount = (rows.Count + COL_SIZE - 1) / COL_SIZE;
            List<string> list = [];
            for (int i = 0; i < columnCount; i++)
            {
                list.Add(string.Join("\n", rows.Slice(i, columnCount)));
            }
            return list;
        }
    }
}