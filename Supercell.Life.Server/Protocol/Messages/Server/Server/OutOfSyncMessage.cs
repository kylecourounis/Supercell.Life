namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class OutOfSyncMessage : PiranhaMessage
    {
        /// <summary>
        /// The service node for this message.
        /// </summary>
        internal override ServiceNode Node
        {
            get
            {
                return ServiceNode.Home;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OutOfSyncMessage"/> class.
        /// </summary>
        public OutOfSyncMessage(Connection connection) : base(connection)
        {
            this.Type = Message.OutOfSync;
        }
    }
}