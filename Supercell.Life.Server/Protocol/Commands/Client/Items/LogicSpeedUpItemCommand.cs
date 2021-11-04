namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Network;

    internal class LogicSpeedUpItemCommand : LogicCommand
    {
        private LogicItemsData ItemData;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicSpeedUpItemCommand"/> class.
        /// </summary>
        public LogicSpeedUpItemCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicSpeedUpItemCommand.
        }

        internal override void Decode()
        {
            this.ItemData = this.Stream.ReadDataReference<LogicItemsData>();

            this.ReadHeader();
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            gamemode.Avatar.ItemUnavailableTimer.Finish();
        }
    }
}
