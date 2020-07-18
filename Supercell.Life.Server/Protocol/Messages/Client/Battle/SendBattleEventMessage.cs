namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class SendBattleEventMessage : PiranhaMessage
    {
        private int Unknown_1;
        private int Unknown_2;
        private int Unknown_3;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendBattleEventMessage"/> class.
        /// </summary>
        public SendBattleEventMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // SendBattleEventMessage.
        }

        internal override void Decode()
        {
            this.Unknown_1 = this.Stream.ReadInt();
            this.Unknown_2 = this.Stream.ReadInt();
            this.Unknown_3 = this.Stream.ReadInt();
        }

        internal override void Handle()
        {
            if (this.Connection.Avatar.Battle != null)
            {
            }
        }
    }
}
