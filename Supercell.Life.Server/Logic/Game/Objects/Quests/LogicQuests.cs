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
            
            string lastStoredName   = "";

            Task.Run(() =>
            {
                for (int i = 0; i < questCsvTable.GetColumnRowCount(); i++)
                {
                    string name = questCsvTable.GetValue("Name", i);
                    string value = questCsvTable.GetValueAt(questCsvTable.GetColumnIndexByName("LevelFile"), i);

                    if (name.IsNullOrEmptyOrWhitespace())
                    {
                        name = lastStoredName;
                    }
                    else
                    {
                        lastStoredName = name;
                    }

                    LogicQuest.LogicLevel level = new LogicQuest.LogicLevel(LogicJSONParser.ParseObject(File.ReadAllText($"Gamefiles/{value}")));

                    if (LogicQuests.Quests.ContainsKey(questCsv.GetDataByName(lastStoredName).GlobalID))
                    {
                        LogicQuests.Quests[questCsv.GetDataByName(lastStoredName).GlobalID].Levels.Add(level);
                    }
                    else
                    {
                        LogicQuest quest = new LogicQuest
                        {
                            Name = lastStoredName,
                            Data = (LogicQuestData)questCsv.GetDataByName(lastStoredName)
                        };

                        quest.Levels.Add(level);

                        LogicQuests.Quests.Add(quest.GlobalID, quest);
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
                Debugger.Info($"GlobalID: {pair.Key}, Levels: {pair.Value.Levels.Count}");

                foreach (var battle in pair.Value.Levels.SelectMany(level => level.Battles))
                {
                    Debugger.Debug($"   -> {battle}");
                }
            }
        }
    }
}
