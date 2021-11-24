namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Battle;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class SectorEndTurnTimeoutMessage : PiranhaMessage
    {
        internal LogicBattle Battle;

        internal int Seed;

        /// <summary>
        /// Initializes a new instance of the <see cref="SectorEndTurnTimeoutMessage"/> class.
        /// </summary>
        public SectorEndTurnTimeoutMessage(Connection connection) : base(connection)
        {
            this.Type = Message.SectorEndTurnTimeout;
        }

        internal override void Encode()
        {
            this.Stream.WriteInt(0);
            this.Stream.WriteInt(0);
            this.Stream.WriteInt(this.Seed);
            this.Stream.WriteString("");
            this.Stream.WriteInt(0);
            this.Stream.WriteDataReference(null);
        }
    }
}
