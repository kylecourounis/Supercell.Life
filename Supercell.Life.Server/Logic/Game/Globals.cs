namespace Supercell.Life.Server.Logic.Game
{
    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Logic.Game.Objects;

    internal static class Globals
    {
        internal static int StartingGold;
        internal static int StartingDiamonds;
        internal static int EnergyRegenateSeconds;
        
        internal static LogicHeroData StartingCharacter;
        internal static LogicQuestData StartingQuest;

        internal static int ShipSailDurationHours;

        internal static int TeamGoalSeasonDurationHours;

        internal static int SpeedUpDiamondCost1Min;
        internal static int SpeedUpDiamondCost1Hour;
        internal static int SpeedUpDiamondCost24Hours;
        internal static int SpeedUpDiamondCost1Week;

        internal static int ResourceDiamondCost10;
        internal static int ResourceDiamondCost100;
        internal static int ResourceDiamondCost1000;
        internal static int ResourceDiamondCost10000;
        internal static int ResourceDiamondCost50000;
        internal static int ResourceDiamondCost100000;
        internal static int ResourceDiamondCost500000;
        internal static int ResourceDiamondCost1000000;

        internal static int PVPFirstTurnTimeSeconds;
        internal static int PVPMaxTurnTimeSeconds;

        /// <summary>
        /// Gets a value indicating whether this instance of <see cref="Globals"/> has been initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the <see cref="Globals"/> class.
        /// </summary>
        internal static void Init()
        {
            if (Globals.Initialized)
            {
                return;
            }

            Globals.StartingGold                = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("STARTING_GOLD")).NumberValue;
            Globals.StartingDiamonds            = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("STARTING_DIAMONDS")).NumberValue;
            Globals.EnergyRegenateSeconds       = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("ENERGY_REGENERATE_SECONDS")).NumberValue;

            Globals.StartingCharacter           = (LogicHeroData)CSV.Tables.Get(Gamefile.Heroes).GetDataByName(((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("STARTING_CHARACTER")).TextValue);
            Globals.StartingQuest               = (LogicQuestData)CSV.Tables.Get(Gamefile.Quests).GetDataByName(((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("STARTING_QUEST")).TextValue);

            Globals.ShipSailDurationHours       = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SHIP_SAIL_DURATION_HOURS")).NumberValue;

            Globals.TeamGoalSeasonDurationHours = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("TEAM_GOAL_SEASON_DURATION_HOURS")).NumberValue;

            Globals.SpeedUpDiamondCost1Min      = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SPEED_UP_DIAMOND_COST_1_MIN")).NumberValue;
            Globals.SpeedUpDiamondCost1Hour     = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SPEED_UP_DIAMOND_COST_1_HOUR")).NumberValue;
            Globals.SpeedUpDiamondCost24Hours   = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SPEED_UP_DIAMOND_COST_24_HOURS")).NumberValue;
            Globals.SpeedUpDiamondCost1Week     = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SPEED_UP_DIAMOND_COST_1_WEEK")).NumberValue;
            
            Globals.ResourceDiamondCost10       = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("RESOURCE_DIAMOND_COST_10")).NumberValue;
            Globals.ResourceDiamondCost100      = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("RESOURCE_DIAMOND_COST_100")).NumberValue;
            Globals.ResourceDiamondCost1000     = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("RESOURCE_DIAMOND_COST_1000")).NumberValue;
            Globals.ResourceDiamondCost10000    = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("RESOURCE_DIAMOND_COST_10000")).NumberValue;
            Globals.ResourceDiamondCost50000    = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("RESOURCE_DIAMOND_COST_50000")).NumberValue;
            Globals.ResourceDiamondCost100000   = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("RESOURCE_DIAMOND_COST_100000")).NumberValue;
            Globals.ResourceDiamondCost500000   = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("RESOURCE_DIAMOND_COST_500000")).NumberValue;
            Globals.ResourceDiamondCost1000000  = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("RESOURCE_DIAMOND_COST_1000000")).NumberValue;

            Globals.PVPFirstTurnTimeSeconds     = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("PVP_FIRST_TURN_MAX_TURN_TIME_SECONDS")).NumberValue;
            Globals.PVPMaxTurnTimeSeconds       = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("PVP_MAX_TURN_TIME_SECONDS")).NumberValue;

            LogicCharacters.Init();

            Globals.Initialized = true;
        }
    }
}