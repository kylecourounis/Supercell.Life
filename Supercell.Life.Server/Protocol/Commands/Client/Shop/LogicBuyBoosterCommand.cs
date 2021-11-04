﻿namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Network;

    internal class LogicBuyBoosterCommand : LogicCommand
    {
        internal LogicBoosterData Booster;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicBuyBoosterCommand"/> class.
        /// </summary>
        public LogicBuyBoosterCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicBuyBoosterCommand.
        }

        internal override void Decode()
        {
            this.Booster = this.Stream.ReadDataReference<LogicBoosterData>();

            this.ReadHeader();
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            if (this.Booster != null)
            {
                int cost = this.Booster.Diamonds;

                if (cost > 0)
                {
                    if (gamemode.Avatar.Diamonds < cost)
                    {
                        Debugger.Error($"Unable to buy the XP Booster - {gamemode.Avatar.Name} does not enough diamonds. (Diamonds : {gamemode.Avatar.Diamonds}, Requires : {cost}).");
                        return;
                    }
                }

                gamemode.Avatar.Diamonds -= cost;

                gamemode.Avatar.Booster.BoostPackage = this.Booster;
                gamemode.Avatar.Booster.Start();
            }
            else Debugger.Error("Unable to buy the XP Booster - The package data does not exist or is invalid.");
        }
    }
}