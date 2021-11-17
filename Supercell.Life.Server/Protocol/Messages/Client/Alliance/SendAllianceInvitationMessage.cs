namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Logic.Alliance;
    using Supercell.Life.Server.Logic.Avatar.Entries;
    using Supercell.Life.Server.Logic.Collections;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;
    using Supercell.Life.Server.Protocol.Commands.Server;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class SendAllianceInvitationMessage : PiranhaMessage
    {
        internal LogicLong AvatarID;

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
        /// Initializes a new instance of the <see cref="SendAllianceInvitationMessage"/> class.
        /// </summary>
        public SendAllianceInvitationMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // SendAllianceInvitationMessage.
        }

        internal override void Decode()
        {
            this.AvatarID = this.Stream.ReadLogicLong();
        }

        internal override void Handle()
        {
            var avatar = Avatars.Get(this.AvatarID);

            if (avatar != null)
            {
                var invite = new AllianceInvitationAvatarStreamEntry(avatar, this.Connection.GameMode.Avatar)
                {
                    AllianceHighID = this.Connection.GameMode.Avatar.Alliance.HighID,
                    AllianceLowID  = this.Connection.GameMode.Avatar.Alliance.LowID,
                    AllianceName   = this.Connection.GameMode.Avatar.Alliance.Name,
                    AllianceBadge  = this.Connection.GameMode.Avatar.Alliance.Badge,
                    IsNew          = true
                };

                avatar.StreamEntries.Add(invite);

                if (avatar.Connection != null)
                {
                    new AvatarStreamEntryMessage(avatar.Connection, invite).Send();
                }
            }
        }
    }
}