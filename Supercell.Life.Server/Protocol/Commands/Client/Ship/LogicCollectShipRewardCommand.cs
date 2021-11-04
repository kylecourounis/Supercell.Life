﻿namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using System.Linq;

    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Avatar.Slots;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Network;

    internal class LogicCollectShipRewardCommand : LogicCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicCollectShipRewardCommand"/> class.
        /// </summary>
        public LogicCollectShipRewardCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicCollectShipRewardCommand.
        }

        internal override void Decode()
        {
            this.ReadHeader();
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            this.CalculateLoot(gamemode);

            gamemode.Avatar.Variables.Remove(LogicVariables.SailRewardUnclaimed.GlobalID);

            gamemode.Avatar.Sailing.Heroes.Clear();
            gamemode.Avatar.Sailing.HeroLevels.Clear();
        }

        private void CalculateLoot(LogicGameMode gamemode)
        {
            if (CSV.Tables.Get(Gamefile.Globals).GetDataByName("SHIP_GOLD_PER_HERO_LVL") is LogicGlobalData shipGold)
            {
                if (CSV.Tables.Get(Gamefile.Globals).GetDataByName("SHIP_XP_PER_HERO_LVL") is LogicGlobalData shipXp)
                {
                    int gold = 0;
                    int xp = 0;

                    foreach (var level in gamemode.Avatar.Sailing.HeroLevels.Select(hero => gamemode.Avatar.HeroLevels.Get(hero.Value.Id)).Select(item => item.Count))
                    {
                        gold += shipGold.NumberArray[level];
                        xp   += shipXp.NumberArray[level];
                    }

                    gamemode.Avatar.AddGold(gold);
                    gamemode.Avatar.AddXP(xp);
                }
            }
        }
    }
}
