namespace Supercell.Life.Server.Protocol.Commands.Server
{
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Alliance;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class LogicAllianceSettingsChangedCommand : LogicServerCommand
    {
        internal Alliance Alliance;
        internal LogicAllianceBadgeData Badge;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicAllianceSettingsChangedCommand"/> class.
        /// </summary>
        public LogicAllianceSettingsChangedCommand(Connection connection) : base(connection)
        {
            this.Type     = Command.AllianceSettingsChanged;
            this.Alliance = this.Connection.GameMode.Avatar.Alliance;
        }

        internal override void Encode()
        {
            this.Stream.WriteLogicLong(this.Alliance.Identifier);
            this.Stream.WriteDataReference(this.Badge);
        }
    }
}