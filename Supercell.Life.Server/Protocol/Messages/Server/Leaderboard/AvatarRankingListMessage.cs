namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Logic.Slots;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class AvatarRankingListMessage : PiranhaMessage
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
        /// Initializes a new instance of the <see cref="AvatarRankingListMessage"/> class.
        /// </summary>
        internal AvatarRankingListMessage(Connection connection) : base(connection)
        {
            this.Type = Message.AvatarRankingList;
        }
        
        internal override void Encode()
        {
            var avatarList = Avatars.OrderByDescending();

            this.Stream.WriteInt(avatarList.Count);

            foreach (LogicClientAvatar avatar in avatarList)
            {
                avatar.EncodeRankingEntry(this.Stream);
            }
        }
    }
}
