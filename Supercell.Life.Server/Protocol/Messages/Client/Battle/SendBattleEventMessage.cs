namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class SendBattleEventMessage : PiranhaMessage
    {
        private int EventType;

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
            this.EventType = this.Stream.ReadInt();
            this.X         = this.Stream.ReadInt();
            this.Y         = this.Stream.ReadInt();
        }

        internal override void Handle()
        {
            this.ShowValues();

            if (this.Connection.GameMode.Battle != null)
            {
                switch (this.EventType)
                {
                    case 0: // Emote??
                    {
                        new BattleEventMessage(this.Connection)
                        {
                            EventType = this.EventType,
                            X         = this.X,
                            Y         = this.Y
                        }.Send();

                        new BattleEventMessage(this.Connection.GameMode.Battle.Avatars.Find(avatar => avatar.Identifier != this.Connection.GameMode.Avatar.Identifier).Connection)
                        {
                            EventType = this.EventType,
                            X         = this.X,
                            Y         = this.Y
                        }.Send(); 
                        
                        break;
                    }
                    case 1: // Aiming
                    {
                        new BattleEventMessage(this.Connection)
                        {
                            EventType = this.EventType,
                            X         = this.X,
                            Y         = this.Y
                        }.Send();

                        new BattleEventMessage(this.Connection.GameMode.Battle.Avatars.Find(avatar => avatar.Identifier != this.Connection.GameMode.Avatar.Identifier).Connection)
                        {
                            EventType = this.EventType,
                            X         = -this.X,
                            Y         = -this.Y
                        }.Send();
                        
                        break;
                    }
                }
            }
        }
    }
}
