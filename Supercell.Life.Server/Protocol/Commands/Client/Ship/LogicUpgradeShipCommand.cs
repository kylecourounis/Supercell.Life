namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvHelpers;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Titan.DataStream;
    
    using Supercell.Life.Server.Network;

    internal class LogicUpgradeShipCommand : LogicCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicUpgradeShipCommand"/> class.
        /// </summary>
        public LogicUpgradeShipCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicSpeedUpShipCommand.
        }

        internal override void Decode()
        {
            this.ReadHeader();
        }

        internal override void Execute()
        {
            LogicDataTable globals = CSV.Tables.Get(Gamefile.Globals);

            int cost  = ((LogicGlobalData)globals.GetDataByName("SHIP_UPGRADE_COST")).NumberArray.Find(value => value == this.Connection.Avatar.ShipLevel);
            int xpLvl = ((LogicGlobalData)globals.GetDataByName("SHIP_UPGRADE_REQUIRED_XP_LEVEL")).NumberArray.Find(value => value == this.Connection.Avatar.ExpLevel);

            if (this.Connection.Avatar.Gold >= cost && this.Connection.Avatar.ExpLevel >= xpLvl)
            {
                this.Connection.Avatar.Gold -= cost;
                this.Connection.Avatar.ShipUpgrade.Start();
            }
        }
    }
}
