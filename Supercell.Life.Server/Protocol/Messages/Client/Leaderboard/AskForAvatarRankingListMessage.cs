namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Titan.DataStream;
    
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class AskForAvatarRankingListMessage : PiranhaMessage
    {
        /// <summary>
        /// The service node for this message.
        /// </summary>
        internal override ServiceNode Node
        {
            get
            {
                return ServiceNode.Ranking;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForAvatarRankingListMessage"/> class.
        /// </summary>
        public AskForAvatarRankingListMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // AskForAvatarRankingListMessage.
        }

        internal override void Handle()
        {
            new AvatarRankingListMessage(this.Connection).Send();
        }
    }
}
