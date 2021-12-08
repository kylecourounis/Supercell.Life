namespace Supercell.Life.Server.Logic.Avatar
{
    using System;

    using Newtonsoft.Json;

    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Logic.Collections;

    internal class AvatarStreamEntry
    {
        internal LogicClientAvatar Avatar;
        internal LogicClientAvatar Sender;
        
        [JsonProperty] internal int HighID;
        [JsonProperty] internal int LowID;

        [JsonProperty] internal int SenderHighID;
        [JsonProperty] internal int SenderLowID;

        [JsonProperty] internal string SenderName;
        [JsonProperty] internal int SenderLevel;
        [JsonProperty] internal int SenderLeague;

        [JsonProperty] internal DateTime Created = DateTime.UtcNow;

        [JsonProperty] internal bool IsRemoved;
        [JsonProperty] internal bool IsNew;

        [JsonProperty] internal AvatarStreamType StreamType;

        /// <summary>
        /// Gets the age of this <see cref="AvatarStreamEntry"/>.
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
        /// Initializes a new instance of the <see cref="AvatarStreamEntry"/> class.
        /// </summary>
        internal AvatarStreamEntry()
        {
            // AvatarStreamEntry.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarStreamEntry"/> class.
        /// </summary>
        internal AvatarStreamEntry(LogicClientAvatar avatar)
        {
            this.Avatar = avatar;
            this.LowID  = this.Avatar.StreamEntries.Count + 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarStreamEntry"/> class.
        /// </summary>
        internal AvatarStreamEntry(LogicClientAvatar avatar, LogicClientAvatar sender) : this(avatar)
        {
            this.SetSender(sender);
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal virtual void Encode(ChecksumEncoder encoder)
        {
            encoder.WriteLogicLong(new LogicLong(this.HighID, this.LowID));

            encoder.WriteLogicLong(new LogicLong(this.SenderHighID, this.SenderLowID));
            encoder.WriteString(this.SenderName);

            encoder.WriteInt(0);
            encoder.WriteInt(this.SenderLeague);
            encoder.WriteInt(this.Age);

            encoder.WriteBoolean(this.IsRemoved);
            encoder.WriteBoolean(this.IsNew);
        }

        /// <summary>
        /// Sets the sender of this <see cref="AvatarStreamEntry"/>.
        /// </summary>
        internal void SetSender(LogicClientAvatar avatar)
        {
            if (avatar == null)
            {
                this.Sender = Avatars.Get(new LogicLong(this.SenderHighID, this.SenderLowID));
            }
            else
            {
                this.Sender = avatar;
            }

            this.SenderHighID = avatar.HighID;
            this.SenderLowID  = avatar.LowID;
            this.SenderName   = avatar.Name;
            this.SenderLevel  = avatar.ExpLevel;
            this.SenderLeague = avatar.League;
        }

        internal enum AvatarStreamType
        {
            BattleLog,
            AllianceJoin   = 3,
            AllianceInvite = 4,
            AllianceKick   = 5,
            AllianceMail   = 6,
            Unknown        = 7
        }
    }
}
