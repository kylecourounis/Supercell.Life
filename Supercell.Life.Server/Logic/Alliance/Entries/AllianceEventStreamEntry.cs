namespace Supercell.Life.Server.Logic.Alliance.Entries
{
    using Newtonsoft.Json;

    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Math;

    internal class AllianceEventStreamEntry : AllianceStreamEntry
    {
        [JsonProperty] internal AllianceStreamEvent Event;

        [JsonProperty] internal int ExecutorHighID;
        [JsonProperty] internal int ExecutorLowID;

        [JsonProperty] internal string ExecutorName;

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceEventStreamEntry"/> class.
        /// </summary>
        internal AllianceEventStreamEntry()
        {
            // AllianceEventStreamEntry.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceEventStreamEntry"/> class.
        /// </summary>
        internal AllianceEventStreamEntry(AllianceMember member, AllianceMember executor, AllianceStreamEvent streamEvent) : base(member, AllianceStreamType.Event)
        {
            this.ExecutorHighID = executor.HighID;
            this.ExecutorLowID = executor.LowID;
            this.ExecutorName = executor.Name;
            this.Event        = streamEvent;
        }

        internal override void Encode(ChecksumEncoder encoder)
        {
            base.Encode(encoder);

            encoder.WriteInt((int)this.Event);
            encoder.WriteLogicLong(new LogicLong(this.ExecutorHighID, this.ExecutorLowID));
            encoder.WriteString(this.ExecutorName);
        }
    }
}