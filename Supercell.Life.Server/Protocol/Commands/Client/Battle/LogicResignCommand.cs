namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Network;

    internal class LogicResignCommand : LogicCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicResignCommand"/> class.
        /// </summary>
        public LogicResignCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicResignCommand.
        }

        internal override void Decode()
        {
            this.ReadHeader();
        }

        internal override void Execute()
        {
            this.Connection.Avatar.RecentlyResigned = true;
            this.Connection.Avatar.Save();
        }
    }
}
