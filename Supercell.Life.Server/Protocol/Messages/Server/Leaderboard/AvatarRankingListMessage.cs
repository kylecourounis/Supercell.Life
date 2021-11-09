namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Logic.Collections;
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
                this.EncodeRankingEntry(avatar);
            }
        }

        /// <summary>
        /// Encodes the specified <see cref="LogicClientAvatar"/>'s ranking entry.
        /// </summary>
        private void EncodeRankingEntry(LogicClientAvatar avatar)
        {
            this.Stream.WriteLogicLong(avatar.Identifier);
            this.Stream.WriteString(avatar.Name);

            this.Stream.WriteInt(0);
            this.Stream.WriteInt(avatar.Score);
            this.Stream.WriteInt(0);
            this.Stream.WriteInt(avatar.ExpLevel);

            this.Stream.WriteInt(0);
            this.Stream.WriteInt(0);
            this.Stream.WriteString(null);

            this.Stream.WriteBoolean(avatar.IsInAlliance);

            if (avatar.IsInAlliance)
            {
                this.Stream.WriteLogicLong(avatar.Alliance.Identifier);
                this.Stream.WriteString(avatar.Alliance.Name);
                this.Stream.WriteDataReference(avatar.Alliance.BadgeData);
            }

            this.Stream.WriteInt(avatar.League);
        }
    }
}
