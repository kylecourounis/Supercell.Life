namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class SectorCommandMessage : PiranhaMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SectorCommandMessage"/> class.
        /// </summary>
        public SectorCommandMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // SectorCommandMessage.
        }

        internal override void Decode()
        {
        }

        internal override void Handle()
        {
        }
    }
}
