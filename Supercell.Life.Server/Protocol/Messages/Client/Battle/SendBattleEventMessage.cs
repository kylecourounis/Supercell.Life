namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class SendBattleEventMessage : PiranhaMessage
    {
        private BattleEvent EventType;

        private int Value1;
        private int Value2;

        /// <summary>
        /// Initializes a new instance of the <see cref="SendBattleEventMessage"/> class.
        /// </summary>
        public SendBattleEventMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // SendBattleEventMessage.
        }

        internal override void Decode()
        {
            this.EventType = (BattleEvent)this.Stream.ReadInt();
            this.Value1    = this.Stream.ReadInt();
            this.Value2    = this.Stream.ReadInt();
        }

        internal override void Handle()
        {
            if (this.Connection.GameMode.Battle != null)
            {
                switch (this.EventType)
                {
                    case BattleEvent.Taunt:
                    {
                        new BattleEventMessage(this.Connection.GameMode.Battle.GetEnemy(this.Connection.GameMode.Avatar).Connection)
                        {
                            EventType   = this.EventType,
                            PlayerIndex = this.Value1,
                            TauntIndex  = this.Value2
                        }.Send(); 
                        
                        break;
                    }
                    case BattleEvent.Aim:
                    {
                        new BattleEventMessage(this.Connection.GameMode.Battle.GetEnemy(this.Connection.GameMode.Avatar).Connection)
                        {
                            EventType  = this.EventType,
                            DirectionX = -this.Value1,
                            DirectionY = -this.Value2
                        }.Send();
                        
                        break;
                    }
                }
            }
        }
    }
}
