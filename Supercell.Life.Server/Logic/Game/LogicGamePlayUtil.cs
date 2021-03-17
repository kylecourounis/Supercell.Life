namespace Supercell.Life.Server.Logic.Game
{
    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Titan.Logic.Math;

    internal class LogicGamePlayUtil
    {
        /// <summary>
        /// Gets the speed up cost.
        /// </summary>
        internal static int GetSpeedUpCost(int seconds, int multiplier)
        {
            if (seconds > 0)
            {
                if (seconds > 59)
                {
                    if (seconds > 3599)
                    {
                        if (seconds > 86399)
                        {
                            return Globals.SpeedUpDiamondCost24Hours * multiplier / 100
                                   + (Globals.SpeedUpDiamondCost1Week - Globals.SpeedUpDiamondCost24Hours)
                                   * (seconds - 86400)
                                   / 100
                                   * multiplier
                                   / 518400;
                        }

                        return Globals.SpeedUpDiamondCost1Hour * multiplier / 100
                               + (Globals.SpeedUpDiamondCost24Hours - Globals.SpeedUpDiamondCost1Hour)
                               * (seconds - 3600)
                               / 100
                               * multiplier
                               / 82800;
                    }

                    return Globals.SpeedUpDiamondCost1Min * multiplier / 100
                           + multiplier
                           * (Globals.SpeedUpDiamondCost1Hour - Globals.SpeedUpDiamondCost1Min)
                           * (seconds - 60)
                           / 354000;
                }

                return LogicMath.Max(Globals.SpeedUpDiamondCost1Min * multiplier / 100, 1);
            }

            return 0;
        }

        /// <summary>
        /// Gets the the speed up cost multiplier.
        /// </summary>
        internal static int GetSpeedUpCostMultiplier(int type)
        {
            switch (type)
            {
                case 0:
                {
                    return 100;
                }
                case 1:
                {
                    return ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SPEED_UP_MODIFIER_SHIP_CONSTRUCTION")).NumberValue;
                }
                case 2:
                {
                    return ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SPEED_UP_MODIFIER_SPELL_CONSTRUCTION")).NumberValue;
                }
                case 3:
                {
                    return ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SPEED_UP_MODIFIER_ITEM_AVAILABILITY")).NumberValue;
                }
                default:
                {
                    return 100;
                }
            }
        }
    }
}
