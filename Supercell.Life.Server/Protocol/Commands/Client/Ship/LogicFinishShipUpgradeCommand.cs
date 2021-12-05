namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Logic.Game;
    using Supercell.Life.Server.Network;

    internal class LogicFinishShipUpgradeCommand : LogicCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicFinishShipUpgradeCommand"/> class.
        /// </summary>
        public LogicFinishShipUpgradeCommand(Connection connection) : base(connection)
        {
            // LogicFinishShipUpgradeCommand.
        }

        internal override void Decode(ByteStream stream)
        {
            base.Decode(stream);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            int cost = LogicGamePlayUtil.GetSpeedUpCost(gamemode.Avatar.ShipUpgrade.Timer.RemainingSecs, 1); // Still not perfectly accurate. Will figure out soon.
            
            if (gamemode.Avatar.Diamonds >= cost)
            {
                gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Diamonds, -cost);
                gamemode.Avatar.ShipUpgrade.Finish();
            }
        }
    }
}
