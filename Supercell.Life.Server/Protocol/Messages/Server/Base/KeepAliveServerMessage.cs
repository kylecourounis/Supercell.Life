namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class KeepAliveServerMessage : PiranhaMessage
    {
        /// <summary>
        /// The service node for this message.
        /// </summary>
        internal override ServiceNode Node
        {
            get
            {
                return ServiceNode.Account;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="KeepAliveServerMessage"/> class.
        /// </summary>
        public KeepAliveServerMessage(Connection connection) : base (connection)
        {
            this.Type = Message.KeepAliveServer;
        }
    }
}
