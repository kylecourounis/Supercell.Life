namespace Supercell.Life.Server.Logic.Alliance.Entries
{
    using Newtonsoft.Json;

    using Supercell.Life.Titan.DataStream;

    internal class AllianceChatStreamEntry : AllianceStreamEntry
    {
        [JsonProperty] internal string Message;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceChatStreamEntry"/> class.
        /// </summary>
        internal AllianceChatStreamEntry()
        {
            // AllianceChatStreamEntry.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceChatStreamEntry"/> class.
        /// </summary>
        internal AllianceChatStreamEntry(AllianceMember member, string message) : base(member, AllianceStreamType.Chat)
        {
            this.Message = message;
        }

        internal override void Encode(ChecksumEncoder encoder)
        {
            base.Encode(encoder);

            encoder.WriteString(this.Message);
        }
    }
}
