namespace Supercell.Life.Server.Protocol.Commands.Server
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Logic.Alliance;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class LogicChangeAllianceRoleCommand : LogicServerCommand
    {
        internal LogicLong AllianceID;

        internal Alliance.Role Role;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicChangeAllianceRoleCommand"/> class.
        /// </summary>
        public LogicChangeAllianceRoleCommand(Connection connection) : base(connection)
        {
            this.Type = Command.ChangeAllianceRole;
        }

        internal override void Decode(ByteStream stream)
        {
            this.AllianceID = stream.ReadLogicLong();
            this.Role       = (Alliance.Role)stream.ReadInt();

            base.Decode(stream);
        }

        internal override void Encode(ChecksumEncoder encoder)
        {
            encoder.WriteLogicLong(this.Connection.GameMode.Avatar.Alliance.Identifier);
            encoder.WriteInt((int)this.Role);

            base.Encode(encoder);
        }
    }
}
