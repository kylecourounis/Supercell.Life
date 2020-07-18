namespace Supercell.Life.Server.Logic.Alliance
{
    using Newtonsoft.Json;

    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Logic.Avatar;

    internal class AllianceMember
    {
        [JsonProperty] internal int HighID;
        [JsonProperty] internal int LowID;
        [JsonProperty] internal string Name;
        [JsonProperty] internal int Role;
        [JsonProperty] internal int Level;
        [JsonProperty] internal int Score;
        [JsonProperty] internal int League;

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        internal LogicLong Identifier => new LogicLong(this.HighID, this.LowID);

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceMember"/> class.
        /// </summary>
        internal AllianceMember(LogicClientAvatar avatar, Alliance.Role role)
        {
            this.HighID = avatar.Identifier.High;
            this.LowID  = avatar.Identifier.Low;
            this.Name   = avatar.Name;
            this.Role   = (int)role;
            this.Level  = avatar.ExpLevel;
            this.Score  = avatar.Score;
            this.League = avatar.League;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceMember"/> class.
        /// </summary>
        internal AllianceMember()
        {
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public void Encode(ByteStream stream)
        {
            stream.WriteLogicLong(this.Identifier);

            stream.WriteString(string.Empty);
            stream.WriteString(this.Name);

            stream.WriteInt(this.Role);
            stream.WriteInt(this.Level);

            stream.WriteInt(this.League);
            stream.WriteInt(this.Score);

            // === These might be related to this member's contributions to team goals ===
            stream.WriteInt(0);
            stream.WriteInt(0);
            stream.WriteInt(0);
            stream.WriteInt(0);
            // ============================================================================

            stream.WriteByte(0); // ? - In some other SC games, this a bool that determines whether a long after it is read, but it appears this game does not have that, so I'm writing this as a byte
        }
    }
}
