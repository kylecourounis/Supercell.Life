namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class FacebookAccountBoundMessage : PiranhaMessage
    {
        private readonly int ResultCode;

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
        /// Initializes a new instance of the <see cref="FacebookAccountBoundMessage"/> class.
        /// </summary>
        public FacebookAccountBoundMessage(Connection connection, int resultCode) : base(connection)
        {
            this.Type = Message.FacebookBound;
            this.ResultCode = resultCode;
        }
        
        internal override void Encode()
        {
            this.Stream.WriteInt(this.ResultCode);
        }
    }
}