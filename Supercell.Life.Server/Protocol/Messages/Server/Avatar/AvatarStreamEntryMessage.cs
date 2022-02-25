namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class AvatarStreamEntryMessage : PiranhaMessage
    {
        internal AvatarStreamEntry StreamEntry;

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
        /// Initializes a new instance of the <see cref="AvatarStreamEntryMessage"/> class.
        /// </summary>
        public AvatarStreamEntryMessage(Connection connection, AvatarStreamEntry entry) : base(connection)
        {
            this.Type        = Message.AvatarStreamEntry;
            this.StreamEntry = entry;
        }

        internal override void Encode()
        {
            this.StreamEntry.Encode(this.Stream);
        }
    }
}
