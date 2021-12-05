namespace Supercell.Life.Server.Logic.Game.Objects
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic.Enums;

    internal static class LogicCharacters
    {
        internal static readonly Dictionary<LogicHeroData, int> Characters = new Dictionary<LogicHeroData, int>();

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

            LogicCharacters.Characters.Add(LogicCharacters.AdventureBoy, 14);
            LogicCharacters.Characters.Add(LogicCharacters.Wizard,       14);
            LogicCharacters.Characters.Add(LogicCharacters.Princess,     12);
            LogicCharacters.Characters.Add(LogicCharacters.Pirate,       11);
            LogicCharacters.Characters.Add(LogicCharacters.Fairy,        7);
            LogicCharacters.Characters.Add(LogicCharacters.Mummy,        9);
            LogicCharacters.Characters.Add(LogicCharacters.Barrel,       6);
            LogicCharacters.Characters.Add(LogicCharacters.SpaceGirl,    5);
            LogicCharacters.Characters.Add(LogicCharacters.Genie,        4);
            LogicCharacters.Characters.Add(LogicCharacters.Yeti,         4);

            LogicCharacters.Initialized = true;
        }

        /// <summary>
        /// Gets the loading index of a hero.
        /// </summary>
        internal static int GetIndex(LogicHeroData hero)
        {
            return LogicCharacters.Characters.Keys.ToList().FindIndex(h => h.GlobalID == hero.GlobalID);
        }

        /// <summary>
        /// Executes an action on each of the heroes in the collection.
        /// </summary>
        internal static void ForEach(Action<LogicHeroData, int> action)
        {
            foreach (var (key, value) in LogicCharacters.Characters)
            {
                action.Invoke(key, value);
            }
        }
    }
}