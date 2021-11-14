namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class BattleEventMessage : PiranhaMessage
    {
        internal int EventType;

        internal int X;
        internal int Y;

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleEventMessage"/> class.
        /// </summary>
        public BattleEventMessage(Connection connection) : base(connection)
        {
            this.Type = Message.BattleEvent;
        }

        internal override void Encode()
        {
            this.Stream.WriteInt(this.EventType);

            this.Stream.WriteInt(this.X);
            this.Stream.WriteInt(this.Y);
        }
    }
}