namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Server.Logic.Battle;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class BattleResultMessage : PiranhaMessage
    {
        internal LogicBattleResult BattleResult;

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleResultMessage"/> class.
        /// </summary>
        public BattleResultMessage(Connection connection) : base(connection)
        {
            this.Type = Message.BattleResult;
        }

        internal override void Encode()
        {
            this.Stream.WriteInt(0);
            this.Stream.WriteInt(0);
            this.Stream.WriteInt(0);
            this.Stream.WriteInt(0);
            this.Stream.WriteInt(0);
            this.Stream.WriteInt(0);

            this.Connection.GameMode.Avatar.Encode(this.Stream);
        }
    }
}