namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using System.Collections.Concurrent;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Commands;
    using Supercell.Life.Server.Protocol.Enums;

    internal class SectorHeartbeatMessage : PiranhaMessage
    {
        internal int Turn;

        internal ConcurrentQueue<LogicCommand> Commands;

        /// <summary>
        /// Initializes a new instance of the <see cref="SectorHeartbeatMessage"/> class.
        /// </summary>
        public SectorHeartbeatMessage(Connection connection) : base(connection)
        {
            this.Type     = Message.SectorHeartbeat;
            this.Commands = new ConcurrentQueue<LogicCommand>();
        }

        internal override void Encode()
        {
            this.Stream.WriteInt(this.Turn);
            this.Stream.WriteInt(0);

            this.Stream.WriteInt(this.Commands.Count);

            for (int i = 0; i < this.Commands.Count; i++)
            {
                var removed = this.Commands.TryDequeue(out LogicCommand command);

                if (removed)
                {
                    command.Encode();

                    this.Stream.WriteInt((int)command.Type);
                    this.Stream.Write(command.Stream.ToArray());
                }
            }
        }
    }
}
