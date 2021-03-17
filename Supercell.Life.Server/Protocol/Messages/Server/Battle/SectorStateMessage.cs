namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Titan.Logic.Enums;

    using Supercell.Life.Server.Logic.Attack;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class SectorStateMessage : PiranhaMessage
    {
        internal LogicBattle Battle;

        internal int Index;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SectorStateMessage"/> class.
        /// </summary>
        public SectorStateMessage(Connection connection, int idx) : base(connection)
        {
            this.Type             = Message.SectorState;
            this.Connection.State = State.Battle;
            this.Index            = idx;
        }

        internal override void Encode()
        {
            if (this.Index == 0)
            {
                this.Battle.Avatars[0].Encode(this.Stream);
                this.Battle.Avatars[1].Encode(this.Stream);
            }
            else if (this.Index == 1)
            {
                this.Battle.Avatars[1].Encode(this.Stream);
                this.Battle.Avatars[0].Encode(this.Stream);
            }

            this.Battle.Encode(this.Stream);
        }
    }
}
