namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class GamecenterAccountBoundMessage : PiranhaMessage
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
        /// Initializes a new instance of the <see cref="GamecenterAccountBoundMessage"/> class.
        /// </summary>
        public GamecenterAccountBoundMessage(Connection connection, int resultCode) : base(connection)
        {
            this.Type = Message.GamecenterBound;
            this.ResultCode = resultCode;
        }
        
        internal override void Encode()
        {
            this.Stream.WriteInt(this.ResultCode);
        }
    }
}