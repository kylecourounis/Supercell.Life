namespace Supercell.Life.Server.Protocol.Commands.Server
{
    using Supercell.Life.Titan.DataStream;

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

        internal override void Encode(ByteStream stream)
        {
            stream.WriteLogicLong(this.Alliance.Identifier);
            stream.WriteString(this.Alliance.Name);

            stream.WriteDataReference(this.Alliance.BadgeData);

            stream.WriteBoolean(this.Alliance.Members.Size < 0);
        }
    }
}
