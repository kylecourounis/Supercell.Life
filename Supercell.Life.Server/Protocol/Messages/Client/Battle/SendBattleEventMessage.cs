namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class SendBattleEventMessage : PiranhaMessage
    {
        private int Unknown;

        private int X;
        private int Y;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendBattleEventMessage"/> class.
        /// </summary>
        public SendBattleEventMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // SendBattleEventMessage.
        }

        internal override void Decode()
        {
            this.Unknown = this.Stream.ReadInt();
            this.X       = this.Stream.ReadInt();
            this.Y       = this.Stream.ReadInt();
        }

        internal override void Handle()
        {
            this.ShowValues();

            if (this.X != 0 && this.Y != 0)
            {
                if (this.Connection.GameMode.Avatar.Battle != null)
                {
                    new BattleEventMessage(this.Connection)
                    {
                        Unknown = this.Unknown,
                        X       = this.X,
                        Y       = this.Y
                    }.Send();

                    new BattleEventMessage(this.Connection.GameMode.Avatar.Battle.Avatars.Find(avatar => avatar.Identifier != this.Connection.GameMode.Avatar.Identifier).Connection)
                    {
                        Unknown = this.Unknown,
                        X       = -this.X,
                        Y       = -this.Y
                    }.Send();
                }
            }
        }
    }
}
