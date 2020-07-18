namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class BindGamecenterAccountMessage : PiranhaMessage
    {
        private bool Force;
        private string GameCenterId;
        private string Url;
        private string BundleID;

        private byte[] Signature;
        private byte[] Salt;
        private byte[] Timestamp;

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
        /// Initializes a new instance of the <see cref="BindGamecenterAccountMessage"/> class.
        /// </summary>
        public BindGamecenterAccountMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // BindGamecenterAccountMessage.
        }

        internal override void Decode()
        {
            this.Force        = this.Stream.ReadBoolean();

            this.GameCenterId = this.Stream.ReadString();
            this.Url          = this.Stream.ReadString();
            this.BundleID     = this.Stream.ReadString();

            this.Signature    = this.Stream.ReadBytes();
            this.Salt         = this.Stream.ReadBytes();
            this.Timestamp    = this.Stream.ReadBytes();
        }

        internal override void Handle()
        {
            new GamecenterAccountBoundMessage(this.Connection, 1).Send();
        }
    }
}