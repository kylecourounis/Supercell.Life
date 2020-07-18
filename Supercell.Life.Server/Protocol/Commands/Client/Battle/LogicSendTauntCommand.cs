namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Network;

    internal class LogicSendTauntCommand : LogicCommand
    {
        internal LogicTauntData Taunt;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicSendTauntCommand"/> class.
        /// </summary>
        public LogicSendTauntCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicSendTauntCommand.
        }

        internal override void Decode()
        {
            this.ReadHeader();

            this.Taunt = this.Stream.ReadDataReference<LogicTauntData>();
        }

        internal override void Execute()
        {
        }
    }
}