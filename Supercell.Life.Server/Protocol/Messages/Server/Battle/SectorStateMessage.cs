namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Titan.Logic.Enums;

    using Supercell.Life.Server.Logic.Battle;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class SectorStateMessage : PiranhaMessage
    {
        internal LogicBattle Battle;

        /// <summary>
        /// Initializes a new instance of the <see cref="SectorStateMessage"/> class.
        /// </summary>
        public SectorStateMessage(Connection connection) : base(connection)
        {
            this.Type             = Message.SectorState;
            this.Connection.State = State.Battle;
        }

        internal override void Encode()
        {
            this.Battle.Encode(this.Stream, this.Connection.GameMode);
        }
    }
}
