namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Commands;

    internal class SectorCommandMessage : PiranhaMessage
    {
        internal int Command;

        /// <summary>
        /// Initializes a new instance of the <see cref="SectorCommandMessage"/> class.
        /// </summary>
        public SectorCommandMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // SectorCommandMessage.
        }

        internal override void Decode()
        {
            this.Command = this.Stream.ReadInt();
        }

        internal override void Handle()
        {
            var command = LogicCommandManager.CreateCommand(this.Command, this.Connection, this.Stream);
            command.Decode();
            command.Execute(this.Connection.GameMode);
        }
    }
}
