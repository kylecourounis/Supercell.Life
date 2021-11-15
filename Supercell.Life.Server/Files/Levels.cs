namespace Supercell.Life.Server.Files
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Supercell.Life.Titan.Files.CsvReader;
    using Supercell.Life.Titan.Logic.Json;
    using Supercell.Life.Titan.Logic.Utils;

    using Supercell.Life.Server.Files.CsvHelpers;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Logic.Game.Objects.Quests;

    internal static class Levels
    {
        internal static readonly Dictionary<int, LogicQuest> Quests = new Dictionary<int, LogicQuest>();

        /// <summary>
        /// Gets a value indicating whether <see cref="Levels"/> is initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the <see cref="Levels"/> class.
        /// </summary>
        internal static void Init()
        {
            if (Levels.Initialized)
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

                        LogicLevel level = new LogicLevel(null, LogicJSONParser.ParseObject(File.ReadAllText($"Gamefiles/{value}")));

                        if (Levels.Quests.ContainsKey(questCsv.GetDataByName(name).GlobalID))
                        {
                            LogicQuest quest = Levels.Quests[questCsv.GetDataByName(name).GlobalID];
                            level.Quest = quest;

                            Levels.Quests[questCsv.GetDataByName(name).GlobalID].Levels.Add(level);
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

                            Levels.Quests.Add(quest.GlobalID, quest);
                        }
                    }
                }

                Console.WriteLine($"Loaded {Levels.Quests.Count} Quests." + Environment.NewLine);
            }).Wait();

            Levels.Initialized = true;
        }

        /// <summary>
        /// Debugs this instance.
        /// </summary>
        private static void Debug()
        {
            foreach (var (id, quest) in Levels.Quests)
            {
                Debugger.Info($"GlobalID: {id}, Levels: {quest.Levels.Count}, Gold Reward: {quest.GoldReward}, XP Reward: {quest.XPReward}");

                foreach (var battle in quest.Levels.Select(level => level.Battles))
                {
                    Debugger.Debug($"   -> {battle[0].JSON}");
                }
            }
        }
    }
}
