namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Titan.Logic.Enums;

    using Supercell.Life.Server.Logic.Battle;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class ReconnectToSectorStateMessage : PiranhaMessage
    {
        internal LogicBattle Battle;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReconnectToSectorStateMessage"/> class.
        /// </summary>
        public ReconnectToSectorStateMessage(Connection connection) : base(connection)
        {
            this.Type             = Message.ReconnectToSectorState;
            this.Connection.State = State.Battle;
        }

        internal override void Encode()
        {
            this.Stream.WriteInt(0);

            this.Battle.GameModes.ForEach(gamemode =>
            {
                gamemode.Avatar.Encode(this.Stream);
            });

            this.Stream.WriteInt(0);

            this.Stream.WriteDataReference(this.Battle.PvPTier);
            this.Stream.WriteDataReference(this.Battle.Event);

            this.Stream.WriteString(this.Battle.JSON.ToString());
        }
    }
}