namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Server.Logic.Alliance;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class AllianceStreamEntryMessage : PiranhaMessage
    {
        private readonly AllianceStreamEntry StreamEntry;

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
        /// Initializes a new instance of the <see cref="AllianceStreamEntryMessage"/> class.
        /// </summary>
        internal AllianceStreamEntryMessage(Connection connection, AllianceStreamEntry streamEntry) : base(connection)
        {
            this.Type        = Message.AllianceStreamEntry;
            this.StreamEntry = streamEntry;
        }

        internal override void Encode()
        {
            this.Stream.WriteInt((int)this.StreamEntry.StreamType);
            this.StreamEntry.Encode(this.Stream);
        }
    }
}
