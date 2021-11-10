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
            gamemode.Avatar.ItemUnavailableTimer.Finish();
        }
    }
}
