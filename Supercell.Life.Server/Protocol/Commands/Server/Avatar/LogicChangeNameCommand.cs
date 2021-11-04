namespace Supercell.Life.Server.Protocol.Commands.Server
{
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class LogicChangeNameCommand : LogicServerCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicChangeNameCommand"/> class.
        /// </summary>
        public LogicChangeNameCommand(Connection connection) : base(connection)
        {
            this.Type = Command.ChangeName;
        }

        internal override void Encode()
        {
            this.Stream.WriteString(this.Connection.GameMode.Avatar.Name);
            this.Stream.WriteBoolean(this.Connection.GameMode.Avatar.NameSetByUser);

            this.WriteHeader();
        }
    }
}