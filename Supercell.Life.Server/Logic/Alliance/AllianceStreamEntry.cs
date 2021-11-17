namespace Supercell.Life.Server.Logic.Alliance
{
    using System;

    using Newtonsoft.Json;

    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Math;
    
    internal class AllianceStreamEntry
    {
        [JsonProperty] internal int HighID;
        [JsonProperty] internal int LowID;

        [JsonProperty] internal int SenderHighID;
        [JsonProperty] internal int SenderLowID;
        [JsonProperty] internal int SenderLevel;
        [JsonProperty] internal int SenderLeague;

        [JsonProperty] internal string SenderName;

        [JsonProperty] internal Alliance.Role SenderRole;

        [JsonProperty] internal DateTime Created = DateTime.UtcNow;

        [JsonProperty]
        internal AllianceStreamType StreamType
        {
            get
            {
                return this.Message != null ? AllianceStreamType.Chat : AllianceStreamType.Event;
            }
        }

        [JsonProperty] internal int ExecutorHighID;
        [JsonProperty] internal int ExecutorLowID;

        [JsonProperty] internal AllianceStreamEvent Event;
        [JsonProperty] internal string ExecutorName;

        [JsonProperty] internal string Message;

        /// <summary>
        /// Gets the age of this <see cref="AllianceStreamEntry"/>.
        /// </summary>
        [JsonIgnore]
        internal int Age
        {
            get
            {
                return (int)DateTime.UtcNow.Subtract(this.Created).TotalSeconds;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceStreamEntry"/> class.
        /// </summary>
        internal AllianceStreamEntry()
        {
            // AllianceStreamEntry.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceStreamEntry"/> class.
        /// </summary>
        internal AllianceStreamEntry(AllianceMember member)
        {
            this.SenderHighID = member.HighID;
            this.SenderLowID  = member.LowID;

            this.SenderName   = member.Name;
            this.SenderLeague = member.League;

            this.SenderRole   = (Alliance.Role)member.Role;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceStreamEntry"/> class.
        /// </summary>
        internal AllianceStreamEntry(AllianceMember member, string message) : this(member)
        {
            this.Message = message;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceStreamEntry"/> class.
        /// </summary>
        internal AllianceStreamEntry(AllianceMember member, AllianceMember executor, AllianceStreamEvent streamEvent) : this(member)
        {
            this.ExecutorHighID = executor.HighID;
            this.ExecutorLowID  = executor.LowID;
            this.ExecutorName   = executor.Name;
            this.Event          = streamEvent;
        }

        internal void Encode(ChecksumEncoder encoder)
        {
            if (this.StreamType == AllianceStreamType.Chat)
            {
                this.EncodeHeader(encoder);

                encoder.WriteString(this.Message);
            }
            else
            {
                this.EncodeHeader(encoder);

                encoder.WriteInt((int)this.Event);
                encoder.WriteLogicLong(new LogicLong(this.ExecutorHighID, this.ExecutorLowID));
                encoder.WriteString(this.ExecutorName);
            }
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal void EncodeHeader(ChecksumEncoder encoder)
        {
            encoder.WriteLogicLong(new LogicLong(this.HighID, this.LowID));
            encoder.WriteLogicLong(new LogicLong(this.HighID, this.LowID));

            encoder.WriteString(this.SenderName);

            encoder.WriteInt(this.SenderLevel);
            encoder.WriteInt(this.SenderLeague);
            encoder.WriteInt((int)this.SenderRole);

            encoder.WriteInt(this.Age);

            encoder.WriteBoolean(false); // ?
        }

        #region ShouldSerialize
        public bool ShouldSerializeMessage()
        {
            return this.StreamType == AllianceStreamType.Chat;
        }

        public bool ShouldSerializeEvent()
        {
            return this.StreamType == AllianceStreamType.Event;
        }

        public bool ShouldSerializeExecutorHighID()
        {
            return this.StreamType == AllianceStreamType.Event;
        }

        public bool ShouldSerializeExecutorLowID()
        {
            return this.StreamType == AllianceStreamType.Event;
        }

        public bool ShouldSerializeExecutorName()
        {
            return this.StreamType == AllianceStreamType.Event;
        }
        #endregion

        internal enum AllianceStreamEvent
        {
            Kick = 1,
            Accepted,
            Joined,
            Left,
            Promoted,
            Demoted
        }

        internal enum AllianceStreamType
        {
            Chat  = 2,
            Event = 4
        }
    }
}
