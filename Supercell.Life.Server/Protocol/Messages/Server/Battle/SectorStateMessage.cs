namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Titan.Logic.Enums;

    using Supercell.Life.Server.Logic.Battle;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class SectorStateMessage : PiranhaMessage
    {
        internal LogicBattle Battle;

        internal int Side;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SectorStateMessage"/> class.
        /// </summary>
        public SectorStateMessage(Connection connection, int side) : base(connection)
        {
            this.Type             = Message.SectorState;
            this.Connection.State = State.Battle;
            this.Side             = side;
        }

        internal override void Encode()
        {
            this.Battle.Encode(this.Stream, this.Side);
        }
    }
}
