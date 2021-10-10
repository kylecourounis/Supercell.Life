namespace Supercell.Life.Server.Logic.Game.Objects.Quests
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Supercell.Life.Titan.Files.CsvReader;
    using Supercell.Life.Titan.Logic.Json;
    using Supercell.Life.Titan.Logic.Utils;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvHelpers;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic.Enums;

    internal static class LogicQuests
    {
        internal static readonly Dictionary<int, LogicQuest> Quests = new Dictionary<int, LogicQuest>();

        /// <summary>
        /// Gets a value indicating whether <see cref="LogicQuests"/> is initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the <see cref="LogicQuests"/> class.
        /// </summary>
        internal static void Init()
        {
            if (LogicQuests.Initialized)
            {
                return;
            }

            LogicDataTable questCsv = CSV.Tables.Get(Gamefile.Quests);
            Table questCsvTable     = questCsv.Table;
            
            Task.Run(() =>
            {
                var quests = questCsvTable.Rows.Where(row => !row.Name.IsNullOrEmptyOrWhitespace()).ToList();

                for (int i = 0; i < quests.Count; i++)
                {
                    Row currentQuest = questCsvTable.GetRowAt(i);
                    Row nextQuest = null;

                    if (i < quests.Count - 1)
                    { 
                        nextQuest = questCsvTable.GetRowAt(quests.FindIndex(row => row.Name.Equals(quests[i + 1].Name)));
                    }
                    
                    string name = currentQuest.Name;

                    int startIdx = 1;

                    if (nextQuest != null)
                    {
                        startIdx = (nextQuest.Offset - currentQuest.Offset);
                    }

                    for (int j = 0; j < startIdx; j++)
                    {
                        string value = questCsvTable.GetValue("LevelFile", currentQuest.Offset + j);

                        LogicQuest.LogicLevel level = new LogicQuest.LogicLevel(null, LogicJSONParser.ParseObject(File.ReadAllText($"Gamefiles/{value}")));

                        if (LogicQuests.Quests.ContainsKey(questCsv.GetDataByName(name).GlobalID))
                        {
                            LogicQuest quest = LogicQuests.Quests[questCsv.GetDataByName(name).GlobalID];
                            level.Quest = quest;
                            
                            LogicQuests.Quests[questCsv.GetDataByName(name).GlobalID].Levels.Add(level);
                        }
                        else
                        {
                            LogicQuest quest = new LogicQuest
                            {
                                Name = name,
                                Data = (LogicQuestData)questCsv.GetDataByName(name)
                            };

                            var replayRewards = LogicJSONParser.ParseObject(File.ReadAllText("Gamefiles/replay-reward-values.json"));
                            var questData     = replayRewards?.GetJsonObject(name);

                            if (questData != null)
                            {
                                quest.ReplayGoldReward = questData.GetJsonNumber("Gold").GetIntValue();
                                quest.ReplayXPReward   = questData.GetJsonNumber("XP").GetIntValue();
                            }

                            level.Quest = quest;
                            quest.Levels.Add(level);

                            LogicQuests.Quests.Add(quest.GlobalID, quest);
                        }
                    }
                }

                Console.WriteLine($"Loaded {LogicQuests.Quests.Count} Quests." + Environment.NewLine);
            }).Wait();
            
            LogicQuests.Initialized = true;
        }

        /// <summary>
        /// Debugs this instance.
        /// </summary>
        private static void Debug()
        {
            foreach (var pair in LogicQuests.Quests)
            {
                Debugger.Info($"GlobalID: {pair.Key}, Levels: {pair.Value.Levels.Count}, Gold Reward: {pair.Value.GoldReward}, XP Reward: {pair.Value.XPReward}");

                foreach (var battle in pair.Value.Levels.Select(level => level.Battles))
                {
                    Debugger.Debug($"   -> {battle[0].JSON}");
                }
            }
        }
    }
}
