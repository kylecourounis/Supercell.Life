namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Network;

    internal class LogicSendTauntCommand : LogicCommand
    {
        internal LogicTauntData Taunt;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicSendTauntCommand"/> class.
        /// </summary>
        public LogicSendTauntCommand(Connection connection) : base(connection)
        {
            // LogicSendTauntCommand.
        }

        internal override void Decode(ByteStream stream)
        {
            base.Decode(stream);

            this.Taunt = stream.ReadDataReference<LogicTauntData>();
        }

        internal override void Execute(LogicGameMode gamemode)
        {
        }
    }
}