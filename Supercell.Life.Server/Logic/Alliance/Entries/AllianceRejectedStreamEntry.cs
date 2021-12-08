namespace Supercell.Life.Server.Logic.Alliance.Entries
{
    using Newtonsoft.Json;

    using Supercell.Life.Titan.DataStream;

    internal class AllianceRejectedStreamEntry : AllianceStreamEntry
    {
        [JsonProperty] internal string ExecutorName;
        [JsonProperty] internal string RejectedName;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceRejectedStreamEntry"/> class.
        /// </summary>
        internal AllianceRejectedStreamEntry()
        {
            // AllianceRejectedStreamEntry.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceRejectedStreamEntry"/> class.
        /// </summary>
        internal AllianceRejectedStreamEntry(AllianceMember member, string rejectedName) : base(member, AllianceStreamType.Rejected)
        {
            this.ExecutorName = member.Name;
            this.RejectedName = rejectedName;
        }

        internal override void Encode(ChecksumEncoder encoder)
        {
            base.Encode(encoder);

            encoder.WriteString(this.ExecutorName);
            encoder.WriteString(this.RejectedName);

            encoder.WriteInt(0);
        }
    }
}