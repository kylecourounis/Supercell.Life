namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Logic.Alliance;
    using Supercell.Life.Server.Logic.Slots;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class AskForAllianceDataMessage : PiranhaMessage
    {
        internal LogicLong AllianceId;

        /// <summary>
        /// The service node for this message.
        /// </summary>
        internal override ServiceNode Node
        {
            get
            {
                return ServiceNode.Alliance;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForAllianceDataMessage"/> class.
        /// </summary>
        public AskForAllianceDataMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // AskForAllianceDataMessage.
        }

        internal override void Decode()
        {
            this.AllianceId = this.Stream.ReadLogicLong();
        }

        internal override void Handle()
        {
            Alliance alliance = Alliances.Get(this.AllianceId);

            if (alliance != null)
            {
                new AllianceDataMessage(this.Connection, alliance).Send();
            }
        }
    }
}