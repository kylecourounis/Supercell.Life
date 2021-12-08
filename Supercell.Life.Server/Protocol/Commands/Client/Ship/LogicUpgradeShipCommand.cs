namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Logic.Game;
    using Supercell.Life.Server.Network;

    internal class LogicUpgradeShipCommand : LogicCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicUpgradeShipCommand"/> class.
        /// </summary>
        public LogicUpgradeShipCommand(Connection connection) : base(connection)
        {
            // LogicSpeedUpShipCommand.
        }

        internal override void Decode(ByteStream stream)
        {
            base.Decode(stream);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            if (gamemode.Avatar.ShipLevel == 0)
            {
                gamemode.Avatar.ShipLevel = 1;
            }
            else
            {
                int cost  = Globals.ShipUpgradeCost[gamemode.Avatar.ShipLevel];
                int xpLvl = Globals.ShipUpgradeRequiredXPLevel[gamemode.Avatar.ShipLevel];

                if (gamemode.Avatar.ExpLevel >= xpLvl && gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Gold, -cost))
                {
                    gamemode.Avatar.ShipUpgrade.Start();
                }
            }
        }
    }
}
