namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Server.Logic.Battle;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Commands;
    using Supercell.Life.Server.Protocol.Enums;

    internal class SectorHeartbeatMessage : PiranhaMessage
    {
        internal LogicBattle Battle;

        /// <summary>
        /// Initializes a new instance of the <see cref="SectorHeartbeatMessage"/> class.
        /// </summary>
        public SectorHeartbeatMessage(Connection connection) : base(connection)
        {
            this.Type = Message.SectorHeartbeat;
        }

        internal override void Encode()
        {
            this.Stream.WriteInt(this.Connection.GameMode.Avatar.Time.ClientSubTick / 10);
            this.Stream.WriteInt(this.Connection.GameMode.Avatar.Checksum);

            this.Stream.WriteInt(this.Battle.CommandQueues[this.Connection.GameMode].Count);

            for (int i = 0; i < this.Battle.CommandQueues[this.Connection.GameMode].Count; i++)
            {
                if (this.Battle.CommandQueues[this.Connection.GameMode].TryDequeue(out LogicCommand command))
                {
                    this.Stream.WriteInt((int)command.Type);
                    command.Encode(this.Stream);
                }
            }
        }
    }
}
