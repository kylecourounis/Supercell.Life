namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class MaintenanceInboundMessage : PiranhaMessage
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
        /// Initializes a new instance of the <see cref="MaintenanceInboundMessage"/> class.
        /// </summary>
        public MaintenanceInboundMessage(Connection connection) : base(connection)
        {
            this.Type = Message.MaintenanceInbound;
        }
    }
}