namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class CancelFriendlyChallengeMessage : PiranhaMessage
    {
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
        /// Initializes a new instance of the <see cref="CancelFriendlyChallengeMessage"/> class.
        /// </summary>
        public CancelFriendlyChallengeMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // CancelFriendlyChallengeMessage.
        }
        
        internal override void Handle()
        {
            new OwnAvatarDataMessage(this.Connection).Send();
        }
    }
}