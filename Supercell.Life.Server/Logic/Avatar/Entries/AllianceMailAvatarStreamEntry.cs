namespace Supercell.Life.Server.Logic.Avatar.Entries
{
    using Newtonsoft.Json;

    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Math;

    internal class AllianceMailAvatarStreamEntry : AvatarStreamEntry
    {
        [JsonProperty] internal int AllianceHighID;
        [JsonProperty] internal int AllianceLowID;
        [JsonProperty] internal string AllianceName;
        [JsonProperty] internal int AllianceBadge;

        [JsonProperty] internal string Title;
        [JsonProperty] internal string Message;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceMailAvatarStreamEntry"/> class.
        /// </summary>
        internal AllianceMailAvatarStreamEntry()
        {
            // AllianceMailAvatarStreamEntry.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceMailAvatarStreamEntry"/> class.
        /// </summary>
        internal AllianceMailAvatarStreamEntry(LogicClientAvatar avatar, LogicClientAvatar sender) : base(avatar, sender)
        {
            this.StreamType = AvatarStreamType.AllianceMail;
        }

        internal override void Encode(ChecksumEncoder encoder)
        {
            base.Encode(encoder);

            encoder.WriteString(this.Title);
            encoder.WriteString(this.Message);

            encoder.WriteLogicLong(new LogicLong(this.AllianceHighID, this.AllianceLowID));
            encoder.WriteString(this.AllianceName);
            encoder.WriteInt(this.AllianceBadge);
        }
    }
}
