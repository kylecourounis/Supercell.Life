namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class BattleEventMessage : PiranhaMessage
    {
        internal BattleEvent EventType;

        internal int DirectionX, DirectionY;
        internal int PlayerIndex, TauntIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleEventMessage"/> class.
        /// </summary>
        public BattleEventMessage(Connection connection) : base(connection)
        {
            this.Type = Message.BattleEvent;
        }

        internal override void Encode()
        {
            this.Stream.WriteInt((int)this.EventType);

            switch (this.EventType) 
            {
                case BattleEvent.Taunt:
                {
                    this.Stream.WriteInt(this.PlayerIndex == 0 ? 1 : 0); // Invert the Player Index so that the taunt bubble will actually show
                    this.Stream.WriteInt(this.TauntIndex);

                    break;
                }
                case BattleEvent.Aim:
                {
                    this.Stream.WriteInt(this.DirectionX);
                    this.Stream.WriteInt(this.DirectionY);

                    break;
                }
            }
        }
    }
}