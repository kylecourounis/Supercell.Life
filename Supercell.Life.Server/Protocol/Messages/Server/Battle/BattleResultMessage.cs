namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Server.Core;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class BattleResultMessage : PiranhaMessage
    {
        internal int TrophyReward;

        /// <summary>
        /// Initializes a new instance of the <see cref="BattleResultMessage"/> class.
        /// </summary>
        public BattleResultMessage(Connection connection) : base(connection)
        {
            this.Type = Message.BattleResult;
            this.TrophyReward = Loader.Random.Rand(20, 30);
        }

        internal override void Encode()
        {
            this.Stream.WriteInt(1);
            this.Stream.WriteInt(this.TrophyReward);
            this.Stream.WriteInt(0);
            this.Stream.WriteInt(0);
            this.Stream.WriteInt(0);
            this.Stream.WriteInt(0);

            this.Connection.GameMode.Avatar.Encode(this.Stream);
        }
    }
}