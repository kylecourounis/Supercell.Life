namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Server.Logic.Battle;
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
                new BattleEventMessage(this.Connection.GameMode.Battle.GetEnemy(this.Connection.GameMode.Avatar).Connection)
                {
                    BattleEvent = new LogicBattleEvent(this.Connection.GameMode.Battle, this.EventType, this.Value1, this.Value2)
                }.Send();
            }
        }
    }
}
