namespace Supercell.Life.Server.Logic.Alliance
{
    using System.Linq;

    using Newtonsoft.Json;

    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic;
    using Supercell.Life.Titan.Logic.Enums;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Logic.Alliance;
    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Logic.Avatar.Timers;
    using Supercell.Life.Server.Logic.Collections;
    using Supercell.Life.Server.Protocol.Messages;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class Alliance
    {
        [JsonProperty] internal int HighID;
        [JsonProperty] internal int LowID;

        [JsonProperty] internal int RequiredTrophies;

        [JsonProperty] internal int Origin;

        [JsonProperty] internal int Badge;
        internal LogicAllianceBadgeData BadgeData => (LogicAllianceBadgeData)CSV.Tables.Get(Gamefile.AllianceBadges).GetDataWithID(this.Badge);
        
        [JsonProperty] internal string Name;
        [JsonProperty] internal string Description;

        [JsonProperty] internal Hiring Type = Hiring.Open;
        
        [JsonProperty] internal LogicArrayList<AllianceMember> Members;
        [JsonProperty] internal LogicArrayList<AllianceStreamEntry> Entries;

        internal LogicTeamGoalData TeamGoal => (LogicTeamGoalData)CSV.Tables.Get(Gamefile.TeamGoals).GetDataByName("clear_bats0");

        [JsonProperty] internal int TotalStarsCollected;
        [JsonProperty] internal int StarsLastSeason;

        [JsonProperty] internal LogicTeamGoalTimer TeamGoalTimer;

        /// <summary>
        /// Gets the seed.
        /// </summary>
        internal int Seed
        {
            get
            {
                return this.Entries.Count + 1;
            }
        }

        /// <summary>
        /// Gets the clan identifier.
        /// </summary>
        internal long Identifier
        {
            get
            {
                return (long) this.HighID << 32 | (uint) this.LowID;
            }
        }

        /// <summary>
        /// Gets the score.
        /// </summary>
        internal int Score
        {
            get
            {
                return this.Members.Sum(member => member.Score) / 2;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Alliance"/> class.
        /// </summary>
        internal Alliance()
        {
            this.Members = new LogicArrayList<AllianceMember>(50);
            this.Entries = new LogicArrayList<AllianceStreamEntry>(40);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Alliance"/> class.
        /// </summary>
        internal Alliance(int highId, int lowId) : this()
        {
            this.HighID        = highId;
            this.LowID         = lowId;
            this.TeamGoalTimer = new LogicTeamGoalTimer(this);
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal void Encode(ByteStream stream)
        {
            this.EncodeHeader(stream);

            stream.WriteString(this.Description);
        }

        /// <summary>
        /// Encodes the header.
        /// </summary>
        internal void EncodeHeader(ByteStream stream)
        {
            stream.WriteLogicLong(this.Identifier);
            stream.WriteString(this.Name);

            stream.WriteDataReference(this.BadgeData);

            stream.WriteInt((int)this.Type);
            stream.WriteInt(this.Members.Size);

            stream.WriteInt(this.Score);
            stream.WriteInt(this.RequiredTrophies);

            stream.WriteDataReference(null);
            stream.WriteDataReference(null);

            stream.WriteInt(this.TotalStarsCollected); 
            stream.WriteInt(this.StarsLastSeason); 
        }

        /// <summary>
        /// Encodes the ranking entry.
        /// </summary>
        internal void EncodeRankingEntry(ByteStream stream)
        {
            stream.WriteLogicLong(this.Identifier);
            stream.WriteString(this.Name);

            stream.WriteInt(0);
            stream.WriteInt(this.Score);
            stream.WriteInt(0);
            stream.WriteInt(0);

            stream.WriteDataReference(this.BadgeData);

            stream.WriteInt(this.Members.Size);
        }

        /// <summary>
        /// Adds the specified <see cref="AllianceStreamEntry"/>.
        /// </summary>
        internal void AddEntry(AllianceStreamEntry entry)
        {
            entry.LowID = this.Seed;

            if (this.Entries.Count >= 40)
            {
                this.Entries.RemoveAt(0);
            }

            this.Entries.Add(entry);

            foreach (LogicClientAvatar avatar in this.Members.Select(member => Avatars.Get(member.Identifier)).Where(avatar => avatar.Connection != null && avatar.Connection.IsConnected))
            {
                new AllianceStreamEntryMessage(avatar.Connection, entry).Send();
            }
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return $"{this.HighID}-{this.LowID}";
        }

        internal enum Role
        {
            Member   = 1,
            Leader   = 2,
            Admin    = 3, // Apparently like the equivalent to elder, but seems to be unused?
            CoLeader = 4
        }
    }
}