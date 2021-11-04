namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Game;
    using Supercell.Life.Server.Network;

    internal class LogicFinishShipUpgradeCommand : LogicCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicFinishShipUpgradeCommand"/> class.
        /// </summary>
        public LogicFinishShipUpgradeCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicFinishShipUpgradeCommand.
        }

        internal override void Decode()
        {
            this.ReadHeader();
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            var cost = LogicGamePlayUtil.GetSpeedUpCost(86400, LogicGamePlayUtil.GetSpeedUpCostMultiplier(1));
            Debugger.Debug(cost);

            gamemode.Avatar.Diamonds -= cost;

            gamemode.Avatar.ShipUpgrade.Finish();
        }
    }
}
