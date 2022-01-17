namespace Supercell.Life.Server.Logic.Game
{
    using Supercell.Life.Titan.Logic;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Logic.Game.Objects;

    internal static class Globals
    {
        internal static int StartingGold;
        internal static int StartingDiamonds;
        internal static int InitialMaxEnergy;
        internal static int EnergyRegenerateSeconds;
        
        internal static LogicHeroData StartingCharacter;
        internal static LogicQuestData StartingQuest;

        internal static LogicArrayList<int> ShipUpgradeRequiredXPLevel;
        internal static LogicArrayList<int> ShipUpgradeCost;
        internal static LogicArrayList<int> ShipUpgradeDurationHours;

        internal static int ShipSailDurationHours;
        internal static int ShipSeasickDurationHours;

        internal static LogicArrayList<int> ShipGoldPerHeroLevel;
        internal static LogicArrayList<int> ShipXPPerHeroLevel;
        internal static int ShipGoldVariation;
        internal static int ShipXPVariation;

        internal static int AllianceCreateCost;

        internal static int TeamMailSendCooldownTime;
        internal static int TeamGoalSeasonDurationHours;

        internal static int SpeedUpDiamondCost1Min;
        internal static int SpeedUpDiamondCost1Hour;
        internal static int SpeedUpDiamondCost24Hours;
        internal static int SpeedUpDiamondCost1Week;

        internal static int SpeedUpModifierGeneric;
        internal static int SpeedUpModifierShipConstruction;
        internal static int SpeedUpModifierSpellConstruction;
        internal static int SpeedUpModifierItemAvailability;

        internal static int ShipSpeedUpResourceCost100;

        internal static int SpellSlotsAtStart;
        internal static LogicArrayList<int> SpellSlotCost;

        internal static int ResourceDiamondCost10;
        internal static int ResourceDiamondCost100;
        internal static int ResourceDiamondCost1000;
        internal static int ResourceDiamondCost10000;
        internal static int ResourceDiamondCost50000;
        internal static int ResourceDiamondCost100000;
        internal static int ResourceDiamondCost500000;
        internal static int ResourceDiamondCost1000000;

        internal static int MapChestRespawnTime;

        internal static int ReplayChestAvailableOnXPLevel;
        internal static int ReplayChestRespawnHours;

        internal static int SmallChestModifierMin;
        internal static int SmallChestModifierMax;

        internal static int MedChestModifierMin;
        internal static int MedChestModifierMax;

        internal static int BigChestModifierMin;
        internal static int BigChestModifierMax;

        internal static int MultiplayerChestWinCount;
        internal static int MultiplayerChestModifier;

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

            Globals.StartingGold                     = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("STARTING_GOLD")).NumberValue;
            Globals.StartingDiamonds                 = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("STARTING_DIAMONDS")).NumberValue;
            Globals.InitialMaxEnergy                 = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("INITIAL_MAX_ENERGY")).NumberValue;
            Globals.EnergyRegenerateSeconds          = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("ENERGY_REGENERATE_SECONDS")).NumberValue;

            Globals.StartingCharacter                = (LogicHeroData)CSV.Tables.Get(Gamefile.Heroes).GetDataByName(((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("STARTING_CHARACTER")).TextValue);
            Globals.StartingQuest                    = (LogicQuestData)CSV.Tables.Get(Gamefile.Quests).GetDataByName(((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("STARTING_QUEST")).TextValue);
            
            Globals.ShipUpgradeRequiredXPLevel       = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SHIP_UPGRADE_REQUIRED_XP_LEVEL")).NumberArray;
            Globals.ShipUpgradeCost                  = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SHIP_UPGRADE_COST")).NumberArray;
            Globals.ShipUpgradeDurationHours         = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SHIP_UPGRADE_DURATION_HOURS")).NumberArray;

            Globals.ShipSailDurationHours            = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SHIP_SAIL_DURATION_HOURS")).NumberValue;
            Globals.ShipSeasickDurationHours         = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SHIP_SEASICK_DURATION_HOURS")).NumberValue;

            Globals.ShipGoldPerHeroLevel             = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SHIP_GOLD_PER_HERO_LVL")).NumberArray;
            Globals.ShipXPPerHeroLevel               = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SHIP_XP_PER_HERO_LVL")).NumberArray;
            Globals.ShipGoldVariation                = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SHIP_GOLD_VARIATION")).NumberValue;
            Globals.ShipXPVariation                  = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SHIP_XP_VARIATION")).NumberValue;

            Globals.AllianceCreateCost               = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("ALLIANCE_CREATE_COST")).NumberValue;

            Globals.TeamMailSendCooldownTime         = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("TEAM_MAIL_SEND_COOLDOWN_TIME_SECONDS")).NumberValue;
            Globals.TeamGoalSeasonDurationHours      = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("TEAM_GOAL_SEASON_DURATION_HOURS")).NumberValue;

            Globals.SpeedUpDiamondCost1Min           = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SPEED_UP_DIAMOND_COST_1_MIN")).NumberValue;
            Globals.SpeedUpDiamondCost1Hour          = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SPEED_UP_DIAMOND_COST_1_HOUR")).NumberValue;
            Globals.SpeedUpDiamondCost24Hours        = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SPEED_UP_DIAMOND_COST_24_HOURS")).NumberValue;
            Globals.SpeedUpDiamondCost1Week          = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SPEED_UP_DIAMOND_COST_1_WEEK")).NumberValue;
            
            Globals.SpeedUpModifierGeneric           = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SPEED_UP_MODIFIER_GENERIC")).NumberValue;
            Globals.SpeedUpModifierShipConstruction  = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SPEED_UP_MODIFIER_SHIP_CONSTRUCTION")).NumberValue;
            Globals.SpeedUpModifierSpellConstruction = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SPEED_UP_MODIFIER_SPELL_CONSTRUCTION")).NumberValue;
            Globals.SpeedUpModifierItemAvailability  = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SPEED_UP_MODIFIER_ITEM_AVAILABILITY")).NumberValue;

            Globals.SpellSlotsAtStart                = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SPELL_SLOTS_AT_START")).NumberValue;
            Globals.SpellSlotCost                    = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SPELL_SLOT_COST")).NumberArray;

            Globals.ShipSpeedUpResourceCost100       = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SHIP_SPEEDUP_RESOURCE_COST_100")).NumberValue;

            Globals.ResourceDiamondCost10            = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("RESOURCE_DIAMOND_COST_10")).NumberValue;
            Globals.ResourceDiamondCost100           = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("RESOURCE_DIAMOND_COST_100")).NumberValue;
            Globals.ResourceDiamondCost1000          = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("RESOURCE_DIAMOND_COST_1000")).NumberValue;
            Globals.ResourceDiamondCost10000         = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("RESOURCE_DIAMOND_COST_10000")).NumberValue;
            Globals.ResourceDiamondCost50000         = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("RESOURCE_DIAMOND_COST_50000")).NumberValue;
            Globals.ResourceDiamondCost100000        = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("RESOURCE_DIAMOND_COST_100000")).NumberValue;
            Globals.ResourceDiamondCost500000        = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("RESOURCE_DIAMOND_COST_500000")).NumberValue;
            Globals.ResourceDiamondCost1000000       = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("RESOURCE_DIAMOND_COST_1000000")).NumberValue;
            
            Globals.MapChestRespawnTime              = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("MAP_CHEST_RESPAWN_TIME")).NumberValue;
            
            Globals.ReplayChestAvailableOnXPLevel    = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("QUEST_REPLAY_CHEST_AVAILABLE_ON_XP_LEVEL")).NumberValue;
            Globals.ReplayChestRespawnHours          = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("QUEST_REPLAY_CHEST_MOVE_TO_NEW_ISLAND_AFTER_HOURS")).NumberValue;

            Globals.SmallChestModifierMin            = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SMALL_CHEST_MODIFIER_MIN")).NumberValue;
            Globals.SmallChestModifierMax            = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SMALL_CHEST_MODIFIER_MAX")).NumberValue;

            Globals.MedChestModifierMin              = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("MED_CHEST_MODIFIER_MIN")).NumberValue;
            Globals.MedChestModifierMax              = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("MED_CHEST_MODIFIER_MAX")).NumberValue;

            Globals.BigChestModifierMin              = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("BIG_CHEST_MODIFIER_MIN")).NumberValue;
            Globals.BigChestModifierMax              = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("BIG_CHEST_MODIFIER_MAX")).NumberValue;
            
            Globals.MultiplayerChestWinCount         = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("MULTIPLAYER_CHEST_WIN_COUNT")).NumberValue;
            Globals.MultiplayerChestModifier         = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("MULTIPLAYER_CHEST_MODIFIER")).NumberValue;

            Globals.PVPFirstTurnTimeSeconds          = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("PVP_FIRST_TURN_MAX_TURN_TIME_SECONDS")).NumberValue;
            Globals.PVPMaxTurnTimeSeconds            = ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("PVP_MAX_TURN_TIME_SECONDS")).NumberValue;
            
            LogicCharacters.Init();

            Globals.Initialized = true;
        }
    }
}