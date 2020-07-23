namespace Supercell.Life.Server.Logic.Game.Objects
{
    using System;
    using System.Collections.Generic;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic.Enums;

    internal static class LogicCharacters
    {
        private static readonly Dictionary<LogicHeroData, int> MaxLevels = new Dictionary<LogicHeroData, int>();

        internal static readonly LogicHeroData AdventureBoy = (LogicHeroData)CSV.Tables.Get(Gamefile.Heroes).GetDataByName("AdvBoy");
        internal static readonly LogicHeroData Wizard       = (LogicHeroData)CSV.Tables.Get(Gamefile.Heroes).GetDataByName("Wizard");
        internal static readonly LogicHeroData Princess     = (LogicHeroData)CSV.Tables.Get(Gamefile.Heroes).GetDataByName("Princess");
        internal static readonly LogicHeroData Pirate       = (LogicHeroData)CSV.Tables.Get(Gamefile.Heroes).GetDataByName("Pirate");
        internal static readonly LogicHeroData Mummy        = (LogicHeroData)CSV.Tables.Get(Gamefile.Heroes).GetDataByName("Mummy");
        internal static readonly LogicHeroData Fairy        = (LogicHeroData)CSV.Tables.Get(Gamefile.Heroes).GetDataByName("Fairy");
        internal static readonly LogicHeroData Barrel       = (LogicHeroData)CSV.Tables.Get(Gamefile.Heroes).GetDataByName("Barrel");
        internal static readonly LogicHeroData SpaceGirl    = (LogicHeroData)CSV.Tables.Get(Gamefile.Heroes).GetDataByName("SpaceGirl");
        internal static readonly LogicHeroData Genie        = (LogicHeroData)CSV.Tables.Get(Gamefile.Heroes).GetDataByName("Genie");
        internal static readonly LogicHeroData Yeti         = (LogicHeroData)CSV.Tables.Get(Gamefile.Heroes).GetDataByName("Yeti");

        /// <summary>
        /// Gets a value indicating whether this instance of <see cref="LogicCharacters"/> has been initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the <see cref="LogicCharacters"/> class.
        /// </summary>
        internal static void Init()
        {
            if (LogicCharacters.Initialized)
            {
                return;
            }

            LogicCharacters.MaxLevels.Add(LogicCharacters.AdventureBoy, 14);
            LogicCharacters.MaxLevels.Add(LogicCharacters.Wizard,       14);
            LogicCharacters.MaxLevels.Add(LogicCharacters.Princess,     12);
            LogicCharacters.MaxLevels.Add(LogicCharacters.Pirate,       11);
            LogicCharacters.MaxLevels.Add(LogicCharacters.Fairy,        7);
            LogicCharacters.MaxLevels.Add(LogicCharacters.Mummy,        9);
            LogicCharacters.MaxLevels.Add(LogicCharacters.Barrel,       6);
            LogicCharacters.MaxLevels.Add(LogicCharacters.SpaceGirl,    5);
            LogicCharacters.MaxLevels.Add(LogicCharacters.Genie,        4);
            LogicCharacters.MaxLevels.Add(LogicCharacters.Yeti,         4);

            LogicCharacters.Initialized = true;
        }

        /// <summary>
        /// Executes an action on each of the heroes in the collection.
        /// </summary>
        internal static void ForEach(Action<LogicHeroData, int> action)
        {
            foreach (var (key, value) in LogicCharacters.MaxLevels)
            {
                action.Invoke(key, value);
            }
        }
    }
}