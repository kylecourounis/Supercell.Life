namespace Supercell.Life.Server.Protocol.Commands.Server
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Logic.Alliance;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class LogicJoinAllianceCommand : LogicServerCommand
    {
        internal Alliance Alliance;

        internal LogicLong AllianceID;

        internal string AllianceName;

        internal int AllianceBadge;

        internal bool JustCreated;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicJoinAllianceCommand"/> class.
        /// </summary>
        public LogicJoinAllianceCommand(Connection connection) : base(connection)
        {
            this.Type = Command.JoinAlliance;
        }

        internal override void Decode(ByteStream stream)
        {
            this.AllianceID    = stream.ReadLogicLong();
            this.AllianceName  = stream.ReadString();
            this.AllianceBadge = stream.ReadInt();
            this.JustCreated   = stream.ReadBoolean();

            base.Decode(stream);
        }

        internal override void Encode(ChecksumEncoder encoder)
        {
            encoder.WriteLogicLong(this.Alliance.Identifier);
            encoder.WriteString(this.Alliance.Name);
            encoder.WriteInt(this.Alliance.Badge);
            encoder.WriteBoolean(this.JustCreated);

            base.Encode(encoder);
        }
    }
}
