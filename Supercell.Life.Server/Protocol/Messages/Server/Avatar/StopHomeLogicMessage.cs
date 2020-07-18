namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class StopHomeLogicMessage : PiranhaMessage
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
        /// Initializes a new instance of the <see cref="StopHomeLogicMessage"/> class.
        /// </summary>
        public StopHomeLogicMessage(Connection connection) : base(connection)
        {
            this.Type = Message.StopHomeLogic;
        }
    }
}