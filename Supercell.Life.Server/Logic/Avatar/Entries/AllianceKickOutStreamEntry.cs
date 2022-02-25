namespace Supercell.Life.Server.Logic.Avatar.Entries
{
    using Newtonsoft.Json;

    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Math;

    internal class AllianceKickOutStreamEntry : AvatarStreamEntry
    {
        [JsonProperty] internal int AllianceHighID;
        [JsonProperty] internal int AllianceLowID;
        [JsonProperty] internal string AllianceName;
        [JsonProperty] internal int AllianceBadge;

        [JsonProperty] internal string Reason;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceKickOutStreamEntry"/> class.
        /// </summary>
        internal AllianceKickOutStreamEntry()
        {
            // AllianceKickOutStreamEntry.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceKickOutStreamEntry"/> class.
        /// </summary>
        internal AllianceKickOutStreamEntry(LogicClientAvatar avatar, LogicClientAvatar sender) : base(avatar, sender)
        {
            this.StreamType = AvatarStreamType.AllianceKick;
        }

        internal override void Encode(ChecksumEncoder encoder)
        {
            base.Encode(encoder);

            //encoder.WriteString(this.Reason);

            //encoder.WriteLogicLong(new LogicLong(this.AllianceHighID, this.AllianceLowID));
            //encoder.WriteString(this.AllianceName);
            //encoder.WriteInt(this.AllianceBadge);
            
            //encoder.WriteByte(0);

            //encoder.WriteLogicLong(new LogicLong(this.SenderHighID, this.SenderLowID)); // Not entirely sure what this is - client only reads it if the boolean above is true
        }
    }
}
