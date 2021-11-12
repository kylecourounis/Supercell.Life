namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class GamecenterAccountAlreadyBoundMessage : PiranhaMessage
    {
        private readonly LogicClientAvatar Avatar;

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
        /// Initializes a new instance of the <see cref="GamecenterAccountAlreadyBoundMessage"/> class.
        /// </summary>
        public GamecenterAccountAlreadyBoundMessage(Connection connection, LogicClientAvatar avatar) : base(connection)
        {
            this.Type   = Message.GamecenterAlreadyBound;
            this.Avatar = avatar;
        }
        
        internal override void Encode()
        {
            this.Stream.WriteString(null);

            if (this.Avatar.Identifier != 0)
            {
                this.Stream.WriteByte(1);
                this.Stream.WriteLogicLong(this.Avatar.Identifier);
            }
            else this.Stream.WriteByte(0);

            this.Stream.WriteString(this.Avatar.Token);

            this.Avatar.Encode(this.Stream);
        }
    }
}