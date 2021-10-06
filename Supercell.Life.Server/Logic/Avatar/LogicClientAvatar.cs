namespace Supercell.Life.Server.Logic.Avatar
{
    using System;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Enums;
    using Supercell.Life.Titan.Logic.Json;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Alliance;
    using Supercell.Life.Server.Logic.Attack;
    using Supercell.Life.Server.Logic.Avatar.Items;
    using Supercell.Life.Server.Logic.Avatar.Slots;
    using Supercell.Life.Server.Logic.Avatar.Socials;
    using Supercell.Life.Server.Logic.Avatar.Timers;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Logic.Game;
    using Supercell.Life.Server.Logic.Game.Objects.Quests;
    using Supercell.Life.Server.Logic.Slots;
    using Supercell.Life.Server.Network;

    internal class LogicClientAvatar
    {
        internal Connection Connection;

        internal LogicGameMode GameMode;
        
        [JsonProperty] internal int HighID;
        [JsonProperty] internal int LowID;

        [JsonProperty] internal int ClanHighID;
        [JsonProperty] internal int ClanLowID;

        [JsonProperty] internal bool NameSetByUser;

        [JsonProperty] internal string Token;

        [JsonProperty] internal string Name = string.Empty;
        [JsonProperty] internal string Region;

        [JsonProperty] internal int ExpLevel = 1;
        [JsonProperty] internal int ExpPoints;
        [JsonProperty] internal int Diamonds;
        [JsonProperty] internal int FreeDiamonds;
        [JsonProperty] internal int Score;
        [JsonProperty] internal int League;

        [JsonProperty] internal int TutorialMask;

        [JsonProperty] internal JArray Team;

        [JsonProperty] internal Rank Rank;

        [JsonProperty] internal DateTime Update = DateTime.UtcNow;
        [JsonProperty] internal DateTime Created = DateTime.UtcNow;
        [JsonProperty] internal DateTime BanTime = DateTime.UtcNow;

        [JsonProperty] internal int ShipLevel;
        [JsonProperty] internal int Seasick;

        [JsonProperty] internal Facebook Facebook;

        [JsonProperty] internal LogicDataSlot AchievementProgress;
        [JsonProperty] internal LogicResources Resources;
        [JsonProperty] internal LogicVariables Variables;

        [JsonProperty] internal LogicHeroLevels HeroLevels;
        [JsonProperty] internal LogicDataSlot HeroUnlockSeens;
        [JsonProperty] internal LogicDataSlot HeroQuests;
        [JsonProperty] internal LogicDataSlot HeroMatches;
        [JsonProperty] internal LogicDataSlot HeroKills;
        [JsonProperty] internal LogicDataSlot HeroTired;

        [JsonProperty] internal LogicSpells Spells;
        [JsonProperty] internal LogicDataSlot SpellsReady;

        [JsonProperty] internal LogicNpcProgress NpcProgress;
        [JsonProperty] internal LogicDataSlot QuestUnlockSeens;
        [JsonProperty] internal LogicQuestMoves QuestMoves;

        [JsonProperty] internal LogicQuest OngoingQuestData;

        [JsonProperty] internal LogicDataSlot EnergyPackages;

        [JsonProperty] internal LogicDataSlot ItemInventories;
        [JsonProperty] internal LogicDataSlot ItemLevels;
        [JsonProperty] internal LogicDataSlot ItemAttachedTo;
        [JsonProperty] internal LogicDataSlot ItemUnavailable;

        [JsonProperty] internal LogicExtras Extras;

        [JsonProperty] internal LogicEnergyTimer EnergyTimer;
        [JsonProperty] internal LogicBoosterTimer Booster;
        [JsonProperty] internal LogicHeroUpgradeTimer HeroUpgrade;
        [JsonProperty] internal LogicSailingTimer Sailing;
        [JsonProperty] internal LogicShipUpgradeTimer ShipUpgrade;
        [JsonProperty] internal LogicItemUnavailableTimer ItemUnavailableTimer;
        [JsonProperty] internal LogicSpellTimer SpellTimer;

        internal LogicItems Items;

        internal readonly LogicTime Time;

        /// <summary>
        /// Gets the player identifier.
        /// </summary>
        internal LogicLong Identifier => new LogicLong(this.HighID, this.LowID);

        /// <summary>
        /// Gets the player's alliance.
        /// </summary>
        internal Alliance Alliance => Alliances.Get(new LogicLong(this.ClanHighID, this.ClanLowID));

        /// <summary>
        /// Gets a value indicating whether this <see cref="LogicClientAvatar"/> is in an alliance.
        /// </summary>
        internal bool IsInAlliance => this.Alliance != null;

        /// <summary>
        /// Gets a value indicating whether this <see cref="LogicClientAvatar"/> is banned.
        /// </summary>
        internal bool Banned => this.BanTime > DateTime.UtcNow;

        /// <summary>
        /// Gets the time played for this <see cref="LogicClientAvatar"/>.
        /// </summary>
        internal int TimePlayed => DateTime.UtcNow.Subtract(this.Created).Seconds;

        /// <summary>
        /// Gets or sets the gold.
        /// </summary>
        internal int Gold
        {
            get => this.Resources.Get(Resource.Gold);
            set => this.Resources.Set(Resource.Gold, value);
        }

        /// <summary>
        /// Gets or sets the energy.
        /// </summary>
        internal int Energy
        {
            get
            {
                if (this.Resources.Get(Resource.Energy) > this.MaxEnergy)
                {
                    this.Energy = this.MaxEnergy;
                }

                return this.Resources.Get(Resource.Energy);
            }
            set => this.Resources.Set(Resource.Energy, value);
        }

        /// <summary>
        /// Gets or sets Orb1.
        /// </summary>
        internal int Orb1
        {
            get => this.Resources.Get(Resource.Orb1);
            set => this.Resources.Set(Resource.Orb1, value);
        }

        /// <summary>
        /// Gets or sets Orb2.
        /// </summary>
        internal int Orb2
        {
            get => this.Resources.Get(Resource.Orb2);
            set => this.Resources.Set(Resource.Orb2, value);
        }

        /// <summary>
        /// Gets or sets Orb3.
        /// </summary>
        internal int Orb3
        {
            get => this.Resources.Get(Resource.Orb3);
            set => this.Resources.Set(Resource.Orb3, value);
        }

        /// <summary>
        /// Gets or sets Orb4.
        /// </summary>
        internal int Orb4
        {
            get => this.Resources.Get(Resource.Orb4);
            set => this.Resources.Set(Resource.Orb4, value);
        }
        
        /// <summary>
        /// Gets the maximum energy.
        /// </summary>
        internal int MaxEnergy
        {
            get
            {
                int count = 15;

                foreach (Item item in this.EnergyPackages.Values)
                {
                    LogicEnergyPackageData package = (LogicEnergyPackageData)item.Data;

                    for (int i = 0; i < item.Count; i++)
                    {
                        count += package.IncreaseInMaxEnergy[i];
                    }
                }

                return count;
            }
        }

        /// <summary>
        /// Gets the checksum.
        /// </summary>
        internal int Checksum
        {
            get
            {
                return this.ExpLevel
                       + this.ExpPoints
                       + this.Diamonds
                       + this.Diamonds
                       + this.Score
                       + this.Resources.Checksum
                       + this.AchievementProgress.Checksum
                       + this.Spells.Checksum
                       + this.HeroLevels.Checksum
                       + this.NpcProgress.Checksum;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicClientAvatar"/> class.
        /// </summary>
        internal LogicClientAvatar()
        {
            this.GameMode             = new LogicGameMode(this.Connection);

            this.Time                 = new LogicTime();
            this.Team                 = new JArray(Globals.StartingCharacter.GlobalID, 0, 0);

            this.AchievementProgress  = new LogicDataSlot(this);
            this.Resources            = new LogicResources(this);
            this.Variables            = new LogicVariables(this);

            this.Spells               = new LogicSpells(this);
            this.SpellsReady          = new LogicDataSlot(this);

            this.HeroLevels           = new LogicHeroLevels(this);
            this.HeroQuests           = new LogicDataSlot(this);
            this.HeroMatches          = new LogicDataSlot(this);
            this.HeroKills            = new LogicDataSlot(this);
            this.HeroTired            = new LogicDataSlot(this);

            this.NpcProgress          = new LogicNpcProgress(this);
            this.QuestUnlockSeens     = new LogicDataSlot(this);
            this.QuestMoves           = new LogicQuestMoves(this);

            this.EnergyPackages       = new LogicDataSlot(this, 2);
            this.HeroUnlockSeens      = new LogicDataSlot(this);

            this.ItemInventories      = new LogicDataSlot(this);
            this.ItemLevels           = new LogicDataSlot(this);
            this.ItemAttachedTo       = new LogicDataSlot(this);
            this.ItemUnavailable      = new LogicDataSlot(this);

            this.Extras               = new LogicExtras(this);

            this.EnergyTimer          = new LogicEnergyTimer(this);
            this.Booster              = new LogicBoosterTimer(this);
            this.HeroUpgrade          = new LogicHeroUpgradeTimer(this);
            this.Sailing              = new LogicSailingTimer(this);
            this.ShipUpgrade          = new LogicShipUpgradeTimer(this);
            this.ItemUnavailableTimer = new LogicItemUnavailableTimer(this);
            this.SpellTimer           = new LogicSpellTimer(this);
            
            this.Items                = new LogicItems(this);

            this.Facebook             = new Facebook(this);

            this.Diamonds             = Globals.StartingDiamonds;
            this.FreeDiamonds         = Globals.StartingDiamonds;

            this.OngoingQuestData        = LogicQuests.Quests[Globals.StartingQuest.GlobalID];
            this.OngoingQuestData.Avatar = this;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicClientAvatar"/> class.
        /// </summary>
        internal LogicClientAvatar(Connection connection, LogicLong id) : this()
        {
            this.Connection = connection;
            this.HighID     = id.High;
            this.LowID      = id.Low;
        }

        /// <summary>
        /// Sets this <see cref="LogicClientAvatar"/>'s JSON.
        /// </summary>
        internal void SetAvatarJSON(ByteStream stream)
        {
            LogicJSONObject json = new LogicJSONObject();

            this.Spells.Save(json);
            this.Items.Save(json);

            json.Put("questProgress_visit", this.NpcProgress.QuestProgressVisit);
            json.Put("questMoves_visit", this.QuestMoves.QuestMovesVisit);

            json.Put("troop_req_msg", new LogicJSONString());

            json.Put("team", this.HeroLevels.CurrentLoadout);

            json.Put("flicks_out_char", new LogicJSONNumber());
            json.Put("fast_flicks", new LogicJSONNumber());

            this.NpcProgress.Save(json);

            json.Put("shareC", new LogicJSONNumber());
            json.Put("mailC", new LogicJSONNumber());
            json.Put("challengeC", new LogicJSONNumber());

            json.Put("troop_req_t", new LogicJSONNumber());

            this.Booster.Save(json); // XP Booster

            json.Put("ship_lvl", new LogicJSONNumber(this.ShipLevel));
            this.ShipUpgrade.Save(json);

            json.Put("launched_event", new LogicJSONNumber());

            this.Sailing.Save(json);

            json.Put("league", new LogicJSONNumber(this.League));
            json.Put("help_opened", new LogicJSONBoolean(true));
            
            this.EnergyTimer.Save(json);

            json.Put("map_chest", new LogicJSONNumber());
            json.Put("mapDailyBonusChestRespawnTimer", new LogicJSONNumber(86400));

            // Json.Put("freePvp", new LogicJSONNumber());
            // Json.Put("pvp_chest", new LogicJSONNumber());

            json.Put("seasick", new LogicJSONNumber(this.Seasick * 3600));

            this.HeroUpgrade.Save(json);

            json.Put("tutorial_mask", new LogicJSONNumber(this.TutorialMask));
            
            stream.WriteCompressedString(json.ToString());
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal void Encode(ByteStream stream)
        {
            stream.WriteInt(0); // 'sub_19AAC4' - weird int written before the ID

            stream.WriteLogicLong(this.Identifier);
            
            stream.WriteString(this.Name);
            stream.WriteString(this.Facebook.Identifier); // Facebook ID

            stream.WriteInt(this.Diamonds);
            stream.WriteInt(this.FreeDiamonds);
            stream.WriteInt(this.ExpLevel);
            stream.WriteInt(this.ExpPoints);
            stream.WriteInt(this.Score);
            stream.WriteInt(0);

            if (this.IsInAlliance)
            {
                stream.WriteBoolean(this.IsInAlliance);

                stream.WriteLogicLong(this.Alliance.Identifier);
                stream.WriteString(this.Alliance.Name);
                stream.WriteDataReference(this.Alliance.BadgeData);
                stream.WriteInt(this.Alliance.Members.Find(member => member.Name.Equals(this.Name)).Role);

                stream.WriteBoolean(this.NameSetByUser);
            }
            else
            {
                stream.WriteBools(this.IsInAlliance, this.NameSetByUser);
            }

            stream.WriteInt(0);

            this.Resources.Encode(stream);
            
            this.AchievementProgress.Encode(stream); // Achievement Reward Claimed Data
            this.AchievementProgress.Encode(stream); // Achievement XP Reward Data
            this.AchievementProgress.Encode(stream); // Achievement Progress Data

            this.ItemInventories.Encode(stream);

            this.NpcProgress.Encode(stream);
            this.QuestUnlockSeens.Encode(stream);
            this.QuestUnlockSeens.Encode(stream);

            this.HeroLevels.Encode(stream);

            this.HeroKills.Encode(stream);
            this.HeroQuests.Encode(stream);
            this.HeroMatches.Encode(stream);

            this.HeroUnlockSeens.Encode(stream);

            this.Sailing.Heroes.Encode(stream);
            this.Sailing.HeroLevels.Encode(stream);

            this.Extras.Encode(stream);
            this.EnergyPackages.Encode(stream);

            stream.WriteInt(0); // EventState

            this.Variables.Encode(stream);

            stream.WriteInt(0); // Borrowed Out Hero
            stream.WriteInt(0); // Borrowed In Hero
            
            this.QuestMoves.Encode(stream);

            this.Spells.Encode(stream);
            this.SpellsReady.Encode(stream);

            this.HeroTired.Encode(stream); // HeroTired

            this.ItemLevels.Encode(stream);
            this.ItemAttachedTo.Encode(stream);
            this.ItemUnavailable.Encode(stream);

            stream.WriteInt(0); // ?

            this.SetAvatarJSON(stream);
        }

        /// <summary>
        /// Encodes this player's ranking entry.
        /// </summary>
        internal void EncodeRankingEntry(ByteStream stream)
        {
            stream.WriteLogicLong(this.Identifier);
            stream.WriteString(this.Name);

            stream.WriteInt(0);
            stream.WriteInt(this.Score);
            stream.WriteInt(0);
            stream.WriteInt(this.ExpLevel);

            stream.WriteInt(0);
            stream.WriteInt(0);
            stream.WriteString(null);

            stream.WriteBoolean(this.IsInAlliance);

            if (this.IsInAlliance)
            {
                stream.WriteLogicLong(this.Alliance.Identifier);
                stream.WriteString(this.Alliance.Name);
                stream.WriteDataReference(this.Alliance.BadgeData);
            }

            stream.WriteInt(this.League);
        }
        
        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return this.Identifier.ToString();
        }
    }
}
