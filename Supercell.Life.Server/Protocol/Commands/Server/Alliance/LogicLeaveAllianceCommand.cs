namespace Supercell.Life.Server.Protocol.Commands.Server
{
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class LogicLeaveAllianceCommand : LogicServerCommand
    {
        internal LogicLong AllianceID;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicLeaveAllianceCommand"/> class.
        /// </summary>
        public LogicLeaveAllianceCommand(Connection connection, LogicLong allianceId) : base(connection)
        {
            this.Type       = Command.LeaveAlliance;
            this.AllianceID = allianceId;
        }

        internal override void Encode()
        {
            this.Stream.WriteLogicLong(this.AllianceID);
        }
    }
}
