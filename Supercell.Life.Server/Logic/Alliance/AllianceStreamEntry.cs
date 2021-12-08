namespace Supercell.Life.Server.Logic.Alliance
{
    using System;

    using Newtonsoft.Json;

    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Helpers;

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

        [JsonProperty] internal AllianceStreamType StreamType;

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
        internal AllianceStreamEntry(AllianceMember member, AllianceStreamType type)
        {
            this.SenderHighID = member.HighID;
            this.SenderLowID  = member.LowID;

            this.SenderName   = member.Name;
            this.SenderLeague = member.League;

            this.SenderRole   = (Alliance.Role)member.Role;

            this.StreamType   = type;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal virtual void Encode(ChecksumEncoder encoder)
        {
            encoder.WriteLogicLong(new LogicLong(this.HighID, this.LowID));

            encoder.WriteLogicLong(new LogicLong(this.SenderHighID, this.SenderLowID));
            encoder.WriteString(this.SenderName);

            encoder.WriteInt(this.SenderLevel);
            encoder.WriteInt(this.SenderLeague);
            encoder.WriteInt((int)this.SenderRole);

            encoder.WriteInt(this.Age);

            encoder.WriteBoolean(false); // ?

            switch (this.StreamType)
            {
                case AllianceStreamType.Unknown_1:
                {
                    encoder.WriteDataReference(null);
                    encoder.WriteInt(0);

                    encoder.WriteBoolean(true);
                    encoder.WriteString("");

                    break;
                }
                case AllianceStreamType.Unknown_5:
                {
                    encoder.WriteInt(0);
                    encoder.WriteLogicLong(0);

                    encoder.WriteString("");
                    encoder.WriteString("");
                    encoder.WriteString("");

                    encoder.WriteInt(0);
                    encoder.WriteInt(0);
                    encoder.WriteInt(0);

                    break;
                }
                case AllianceStreamType.Unknown_7:
                {
                    encoder.WriteDataReference(null);
                    break;
                }
                case AllianceStreamType.Unknown_10:
                {
                    encoder.WriteString("");

                    encoder.WriteByte(0);
                    encoder.WriteBoolean(true);

                    encoder.WriteString("");

                    break;
                }
                case AllianceStreamType.Unknown_11:
                {
                    encoder.WriteString("");

                    encoder.WriteInt(0);
                    encoder.WriteInt(0);
                    encoder.WriteInt(0);

                    encoder.WriteBoolean(true);
                        
                    encoder.WriteInt(0);
                    encoder.WriteLogicLong(0);

                    break;
                }
            }
        }
        
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
            Unknown_1  = 1,
            Chat       = 2,
            Rejected   = 3,
            Event      = 4,
            Unknown_5  = 5,
            Unknown_7  = 7,
            Unknown_10 = 10,
            Unknown_11 = 11
        }
    }
}
