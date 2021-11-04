namespace Supercell.Life.Server.Protocol.Commands.Server
{
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Alliance;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class LogicJoinAllianceCommand : LogicServerCommand
    {
        internal Alliance Alliance;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicJoinAllianceCommand"/> class.
        /// </summary>
        public LogicJoinAllianceCommand(Connection connection) : base(connection)
        {
            this.Type     = Command.JoinAlliance;
            this.Alliance = this.Connection.GameMode.Avatar.Alliance;
        }

        internal override void Encode()
        {
            this.Stream.WriteLogicLong(this.Alliance.Identifier);
            this.Stream.WriteString(this.Alliance.Name);

            this.Stream.WriteDataReference(this.Alliance.BadgeData);

            this.Stream.WriteBoolean(this.Alliance.Members.Size < 0);
        }
    }
}
