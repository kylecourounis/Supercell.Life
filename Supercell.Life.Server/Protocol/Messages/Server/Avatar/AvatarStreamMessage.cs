namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class AvatarStreamMessage : PiranhaMessage
    {
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
        /// Initializes a new instance of the <see cref="AvatarStreamMessage"/> class.
        /// </summary>
        public AvatarStreamMessage(Connection connection) : base(connection)
        {
            this.Type = Message.AvatarStream;
        }
        
        internal override void Encode()
        {
            this.Stream.WriteInt(this.Connection.GameMode.Avatar.StreamEntries.Count);

            this.Connection.GameMode.Avatar.StreamEntries.ForEach(entry =>
            {
                entry.Encode(this.Stream);
            });
        }
    }
}
