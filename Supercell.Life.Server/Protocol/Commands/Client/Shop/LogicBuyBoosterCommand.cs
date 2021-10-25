namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
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

        internal override void Execute()
        {
            if (this.Booster != null)
            {
                int cost = this.Booster.Diamonds;

                if (cost > 0)
                {
                    if (this.Connection.Avatar.Diamonds < cost)
                    {
                        Debugger.Error($"Unable to buy the XP Booster - {this.Connection.Avatar.Name} does not enough diamonds. (Diamonds : {this.Connection.Avatar.Diamonds}, Requires : {cost}).");
                        return;
                    }
                }

                this.Connection.Avatar.Diamonds -= cost;

                this.Connection.Avatar.Booster.BoostPackage = this.Booster;
                this.Connection.Avatar.Booster.Start();

                this.Connection.Avatar.Save();
            }
            else Debugger.Error("Unable to buy the XP Booster - The package data does not exist or is invalid.");
        }
    }
}