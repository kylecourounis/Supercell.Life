namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Network;

    internal class UnknownMessage : PiranhaMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnknownMessage"/> class.
        /// </summary>
        public UnknownMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // UnknownMessage.
        }

        internal override void Decode()
        {
        }

        internal override void Handle()
        {
        }
    }
}