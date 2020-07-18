namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class HomeLogicStoppedMessage : PiranhaMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HomeLogicStoppedMessage"/> class.
        /// </summary>
        public HomeLogicStoppedMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // HomeLogicStoppedMessage.
        }

        internal override void Decode()
        {
            this.Stream.ReadInt();
            this.Stream.ReadInt();
            this.Stream.ReadInt();
        }

        internal override void Handle()
        {
        }
    }
}
