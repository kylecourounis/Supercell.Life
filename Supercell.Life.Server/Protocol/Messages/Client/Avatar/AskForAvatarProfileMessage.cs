namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class AskForAvatarProfileMessage : PiranhaMessage
    {
        private long VisitID;

        private long Unknown;

        /// <summary>
        /// The service node for this message.
        /// </summary>
        internal override ServiceNode Node
        {
            get
            {
                return ServiceNode.Avatar;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForAvatarProfileMessage"/> class.
        /// </summary>
        public AskForAvatarProfileMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // AskForAvatarProfileMessage.
        }

        internal override void Decode()
        {
            this.VisitID = this.Stream.ReadLogicLong();

            if (this.Stream.ReadBoolean())
            {
                this.Unknown = this.Stream.ReadLong();
            }
        }

        internal override void Handle()
        {
            if (this.VisitID > 0)
            {
                new AvatarProfileMessage(this.Connection, this.VisitID).Send();
            }
        }
    }
}
