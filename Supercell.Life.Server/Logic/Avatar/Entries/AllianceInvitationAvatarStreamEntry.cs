namespace Supercell.Life.Server.Logic.Avatar.Entries
{
    using Newtonsoft.Json;

    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Math;

    internal class AllianceInvitationAvatarStreamEntry : AvatarStreamEntry
    {
        [JsonProperty] internal int AllianceHighID;
        [JsonProperty] internal int AllianceLowID;
        [JsonProperty] internal string AllianceName;
        [JsonProperty] internal int AllianceBadge;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceInvitationAvatarStreamEntry"/> class.
        /// </summary>
        internal AllianceInvitationAvatarStreamEntry()
        {
            // AllianceInvitationAvatarStreamEntry.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceInvitationAvatarStreamEntry"/> class.
        /// </summary>
        internal AllianceInvitationAvatarStreamEntry(LogicClientAvatar avatar, LogicClientAvatar sender) : base(avatar, sender)
        {
            this.StreamType = AvatarStreamType.AllianceInvite;
        }

        internal override void Encode(ChecksumEncoder encoder)
        {
            base.Encode(encoder);

            encoder.WriteLogicLong(new LogicLong(this.AllianceHighID, this.AllianceLowID));
            encoder.WriteString(this.AllianceName);
            encoder.WriteInt(this.AllianceBadge);
        }
    }
}
