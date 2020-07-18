namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Titan.Logic;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Commands.Server;
    using Supercell.Life.Server.Protocol.Enums;

    internal class SectorHeartbeatMessage : PiranhaMessage
    {
        internal int Turn;

        internal int Seed;

        internal LogicArrayList<LogicServerCommand> Commands;

        /// <summary>
        /// Initializes a new instance of the <see cref="SectorHeartbeatMessage"/> class.
        /// </summary>
        public SectorHeartbeatMessage(Connection connection, int seed) : base(connection)
        {
            this.Type     = Message.SectorHeartbeat;
            this.Seed     = seed;
            this.Commands = new LogicArrayList<LogicServerCommand>();
        }

        internal override void Encode()
        {
            this.Stream.WriteInt(this.Turn);
            this.Stream.WriteInt(this.Seed);

            this.Stream.WriteInt(this.Commands.Count);

            foreach (LogicServerCommand command in this.Commands)
            {
                command.Encode();

                this.Stream.WriteInt((int)command.Type);
                this.Stream.Write(command.Stream.ToArray());
            }
        }
    }
}
