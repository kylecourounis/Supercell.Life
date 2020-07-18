namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class BindFacebookAccountMessage : PiranhaMessage
    {
        private bool Force;
        private string FacebookID;
        private string Token;

        /// <summary>
        /// The service node for this message.
        /// </summary>
        internal override ServiceNode Node
        {
            get
            {
                return ServiceNode.Account;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BindFacebookAccountMessage"/> class.
        /// </summary>
        public BindFacebookAccountMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // BindFacebookAccountMessage.
        }

        internal override void Decode()
        {
            this.Force      = this.Stream.ReadBoolean();
            this.FacebookID = this.Stream.ReadString();
            this.Token      = this.Stream.ReadString();
        }

        internal override void Handle()
        {
            new FacebookAccountBoundMessage(this.Connection, 1).Send();
        }
    }
}