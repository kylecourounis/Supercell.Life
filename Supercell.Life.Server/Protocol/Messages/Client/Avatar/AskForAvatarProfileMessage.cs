namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class AskForAvatarProfileMessage : PiranhaMessage
    {
        private LogicLong VisitID;

        private bool Unknown_1;

        private LogicLong Unknown_2;

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
            this.VisitID = this.Stream.ReadLong();

            if (this.Stream.BytesLeft > 0)
            {
                this.Unknown_1 = this.Stream.ReadBoolean();

                if (this.Unknown_1)
                {
                    this.Unknown_2 = this.Stream.ReadLong();
                }
            }
        }

        internal override void Handle()
        {
            if (this.Unknown_2.Low == 0)
            {
                // new AvatarProfileMessage(this.Connection, this.Connection.Avatar.Identifier).Send();
            }
            else
            {
                new AvatarProfileMessage(this.Connection, this.VisitID).Send();
            }
        }
    }
}
