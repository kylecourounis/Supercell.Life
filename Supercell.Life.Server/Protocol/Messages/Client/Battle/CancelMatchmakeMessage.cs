namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class CancelMatchmakeMessage : PiranhaMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CancelMatchmakeMessage"/> class.
        /// </summary>
        public CancelMatchmakeMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // CancelMatchmakeMessage.
        }

        internal override void Handle()
        {
            new OwnAvatarDataMessage(this.Connection).Send();
        }
    }
}
