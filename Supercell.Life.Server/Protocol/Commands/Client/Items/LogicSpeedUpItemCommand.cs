namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Logic.Game;
    using Supercell.Life.Server.Network;

    internal class LogicSpeedUpItemCommand : LogicCommand
    {
        private LogicItemsData ItemData;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicSpeedUpItemCommand"/> class.
        /// </summary>
        public LogicSpeedUpItemCommand(Connection connection) : base(connection)
        {
            // LogicSpeedUpItemCommand.
        }

        internal override void Decode(ByteStream stream)
        {
            this.ItemData = stream.ReadDataReference<LogicItemsData>();

            base.Decode(stream);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            int cost = LogicGamePlayUtil.GetSpeedUpCost(gamemode.Avatar.ItemUnavailableTimer.Items[this.ItemData.GlobalID].RemainingSecs * 15, 3);

            if (gamemode.Avatar.Diamonds >= cost)
            {
                gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Diamonds, -cost);
                gamemode.Avatar.ItemUnavailableTimer.Finish(this.ItemData.GlobalID);
            }
        }
    }
}
