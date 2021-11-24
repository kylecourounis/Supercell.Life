namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Server.Logic.Battle;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class BattleEventMessage : PiranhaMessage
    {
        internal LogicBattleEvent BattleEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleEventMessage"/> class.
        /// </summary>
        public BattleEventMessage(Connection connection) : base(connection)
        {
            this.Type = Message.BattleEvent;
        }

        internal override void Encode()
        {
            this.BattleEvent.Encode(this.Stream);
        }
    }
}