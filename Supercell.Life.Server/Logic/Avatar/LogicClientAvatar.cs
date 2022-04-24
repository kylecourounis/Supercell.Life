namespace Supercell.Life.Server.Logic.Avatar
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic;
    using Supercell.Life.Titan.Logic.Enums;
    using Supercell.Life.Titan.Logic.Json;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Alliance;
    using Supercell.Life.Server.Logic.Avatar.Slots;
    using Supercell.Life.Server.Logic.Avatar.Socials;
    using Supercell.Life.Server.Logic.Avatar.Timers;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Logic.Game;
    using Supercell.Life.Server.Logic.Game.Objects.Quests;
    using Supercell.Life.Server.Logic.Collections;
    using Supercell.Life.Server.Network;

    internal class LogicClientAvatar
    {
        internal Connection Connection;

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
        
        [JsonProperty] internal int TotalSessions;
        [JsonProperty] internal DateTime Update = DateTime.UtcNow;
        [JsonProperty] internal DateTime Created = DateTime.UtcNow;
        [JsonProperty] internal DateTime BanTime = DateTime.UtcNow;

        [JsonProperty] internal int ShipLevel;

        [JsonProperty] internal Facebook Facebook;

        [JsonProperty] internal LogicAchievementProgress AchievementProgress;
        [JsonProperty] internal LogicResources Resources;
        [JsonProperty] internal LogicVariables Variables;

        [JsonProperty] internal LogicHeroLevels HeroLevels;
        [JsonProperty] internal LogicDataSlots HeroUnlockSeens;
        [JsonProperty] internal LogicDataSlots HeroQuests;
        [JsonProperty] internal LogicDataSlots HeroMatches;
        [JsonProperty] internal LogicDataSlots HeroKills;
        [JsonProperty] internal LogicDataSlots HeroTired;

        [JsonProperty] internal LogicSpells Spells;
        [JsonProperty] internal LogicDataSlots SpellsReady;
        [JsonProperty] internal JArray PreviousSpells;

        [JsonProperty] internal LogicNpcProgress NpcProgress;
        [JsonProperty] internal LogicArrayList<int> Crowns;

        [JsonProperty] internal LogicDataSlots QuestUnlockSeens;
        [JsonProperty] internal LogicQuestMoves QuestMoves;

        [JsonProperty] internal LogicQuest OngoingQuestData;

        [JsonProperty] internal LogicDataSlots EnergyPackages;

        [JsonProperty] internal LogicDataSlots ItemInventories;
        [JsonProperty] internal LogicDataSlots ItemLevels;
        [JsonProperty] internal LogicDataSlots ItemAttachedTo;
        [JsonProperty] internal LogicDataSlots ItemUnavailable;

        [JsonProperty] internal LogicExtras Extras;

        [JsonProperty] internal LogicEnergyTimer EnergyTimer;
        [JsonProperty] internal LogicBoosterTimer Booster;
        [JsonProperty] internal LogicHeroUpgradeTimer HeroUpgrade;
        [JsonProperty] internal LogicShipUpgradeTimer ShipUpgrade;
        [JsonProperty] internal LogicSailingTimer Sailing;
        [JsonProperty] internal LogicSeasickTimer SeasickTimer; // I don't think this is correct, but I'm keeping it here for now
        [JsonProperty] internal LogicHeroTiredTimer HeroTiredTimer;
        [JsonProperty] internal LogicItemUnavailableTimer ItemUnavailableTimer;
        [JsonProperty] internal LogicSpellTimer SpellTimer;
        [JsonProperty] internal LogicTeamMailCooldownTimer TeamMailCooldownTimer;
        [JsonProperty] internal LogicMapChestTimer MapChestTimer;
        [JsonProperty] internal LogicBonusChestRespawnTimer BonusChestRespawnTimer;
        [JsonProperty] internal LogicDailyMultiplayerTimer DailyMultiplayerTimer;

        [JsonProperty] internal LogicArrayList<AvatarStreamEntry> StreamEntries;

        internal Dictionary<int, LogicQuest> Quests;

        internal LogicItems Items;

        internal readonly LogicTime Time;

        internal int TimeSinceLastSave;

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
        internal TimeSpan TimePlayed => DateTime.UtcNow.Subtract(this.Created);

        /// <summary>
        /// Gets the instance of <see cref="LogicGameMode"/> for this <see cref="LogicClientAvatar"/>.
        /// </summary>
        internal LogicGameMode GameMode => this.Connection.GameMode;

        /// <summary>
        /// Gets or sets the gold.
        /// </summary>
        internal int Gold
        {
            get => this.Resources.GetResourceCount(CommodityType.Gold);
            set => this.Resources.SetResourceCount(CommodityType.Gold, value);
        }

        /// <summary>
        /// Gets or sets the energy.
        /// </summary>
        internal int Energy
        {
            get
            {
                if (this.Resources.GetResourceCount(CommodityType.Energy) > this.MaxEnergy)
                {
                    this.Energy = this.MaxEnergy;
                }

                return this.Resources.GetResourceCount(CommodityType.Energy);
            }
            set => this.Resources.SetResourceCount(CommodityType.Energy, value);
        }

        /// <summary>
        /// Gets or sets Orb1.
        /// </summary>
        internal int Orb1
        {
            get => this.Resources.GetResourceCount(CommodityType.Orb1);
            set => this.Resources.SetResourceCount(CommodityType.Orb1, value);
        }

        /// <summary>
        /// Gets or sets Orb2.
        /// </summary>
        internal int Orb2
        {
            get => this.Resources.GetResourceCount(CommodityType.Orb2);
            set => this.Resources.SetResourceCount(CommodityType.Orb2, value);
        }

        /// <summary>
        /// Gets or sets Orb3.
        /// </summary>
        internal int Orb3
        {
            get => this.Resources.GetResourceCount(CommodityType.Orb3);
            set => this.Resources.SetResourceCount(CommodityType.Orb3, value);
        }

        /// <summary>
        /// Gets or sets Orb4.
        /// </summary>
        internal int Orb4
        {
            get => this.Resources.GetResourceCount(CommodityType.Orb4);
            set => this.Resources.SetResourceCount(CommodityType.Orb4, value);
        }
        
        /// <summary>
        /// Gets the maximum energy.
        /// </summary>
        internal int MaxEnergy
        {
            get
            {
                int count = Globals.InitialMaxEnergy;

                foreach (var slot in this.EnergyPackages.Values)
                {
                    LogicEnergyPackageData package = (LogicEnergyPackageData)slot.Data;

                    for (int i = 0; i < slot.Count; i++)
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
                       + this.FreeDiamonds
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
            this.Time                    = new LogicTime();
            this.Team                    = new JArray(Globals.StartingCharacter.GlobalID);

            this.AchievementProgress     = new LogicAchievementProgress(this);
            this.Resources               = new LogicResources(this);
            this.Variables               = new LogicVariables(this);

            this.Spells                  = new LogicSpells(this);
            this.SpellsReady             = new LogicDataSlots(this);
            this.PreviousSpells          = new JArray();

            this.HeroLevels              = new LogicHeroLevels(this);
            this.HeroQuests              = new LogicDataSlots(this);
            this.HeroMatches             = new LogicDataSlots(this);
            this.HeroKills               = new LogicDataSlots(this);
            this.HeroTired               = new LogicDataSlots(this);

            this.NpcProgress             = new LogicNpcProgress(this);
            this.Crowns                  = new LogicArrayList<int>();
            this.QuestUnlockSeens        = new LogicDataSlots(this);
            this.QuestMoves              = new LogicQuestMoves(this);

            this.EnergyPackages          = new LogicDataSlots(this, 2);
            this.HeroUnlockSeens         = new LogicDataSlots(this);

            this.ItemInventories         = new LogicDataSlots(this);
            this.ItemLevels              = new LogicDataSlots(this);
            this.ItemAttachedTo          = new LogicDataSlots(this);
            this.ItemUnavailable         = new LogicDataSlots(this);

            this.Extras                  = new LogicExtras(this);

            this.EnergyTimer             = new LogicEnergyTimer(this);
            this.Booster                 = new LogicBoosterTimer(this);
            this.HeroUpgrade             = new LogicHeroUpgradeTimer(this);
            this.ShipUpgrade             = new LogicShipUpgradeTimer(this);
            this.Sailing                 = new LogicSailingTimer(this);
            this.SeasickTimer            = new LogicSeasickTimer(this);
            this.HeroTiredTimer          = new LogicHeroTiredTimer(this);
            this.ItemUnavailableTimer    = new LogicItemUnavailableTimer(this);
            this.SpellTimer              = new LogicSpellTimer(this);
            this.MapChestTimer           = new LogicMapChestTimer(this);
            this.BonusChestRespawnTimer  = new LogicBonusChestRespawnTimer(this);
            this.TeamMailCooldownTimer   = new LogicTeamMailCooldownTimer(this);
            this.DailyMultiplayerTimer   = new LogicDailyMultiplayerTimer(this);

            this.Items                   = new LogicItems(this);

            this.Facebook                = new Facebook(this);
            
            this.Diamonds                = Globals.StartingDiamonds;
            this.FreeDiamonds            = Globals.StartingDiamonds;

            this.StreamEntries           = new LogicArrayList<AvatarStreamEntry>(50);

            this.Quests                  = new Dictionary<int, LogicQuest>(Levels.Quests);

            this.OngoingQuestData        = this.Quests[Globals.StartingQuest.GlobalID];
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
        /// Gets this <see cref="LogicClientAvatar"/>'s JSON.
        /// </summary>
        internal LogicJSONObject GetAvatarJSON()
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
            json.Put("mailC", new LogicJSONNumber(this.TeamMailCooldownTimer.Timer.RemainingSecs));
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

            this.MapChestTimer.Save(json);

            this.SeasickTimer.Save(json);
            
            this.HeroUpgrade.Save(json);

            json.Put("levels_played", new LogicJSONNumber(this.NpcProgress.Values.Select(slot => slot.Count).Count()));
            json.Put("levels_failed", new LogicJSONNumber()); // TODO

            json.Put("pvp_chest", new LogicJSONNumber(this.Variables.GetCount(LogicVariables.ChestProgress.GlobalID)));

            this.BonusChestRespawnTimer.Save(json);

            this.DailyMultiplayerTimer.Save(json);

            json.Put("tutorial_mask", new LogicJSONNumber(this.TutorialMask));

            return json;
        }

        /// <summary>
        /// Sets this <see cref="LogicClientAvatar"/>'s JSON.
        /// </summary>
        internal void SetAvatarJSON(ByteStream stream)
        {
            stream.WriteCompressedString(this.GetAvatarJSON().ToString());
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal void Encode(ByteStream stream)
        {
            stream.WriteInt(0); // LogicBase::encode

            stream.WriteLogicLong(this.Identifier);
            
            stream.WriteString(this.Name);
            stream.WriteString(this.Facebook.Identifier);

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
        /// Sets the commodity count.
        /// </summary>
        internal void SetCommodityCount(CommodityType commodity, int amount)
        {
            switch (commodity)
            {
                case CommodityType.Diamonds:
                {
                    this.Diamonds = amount;
                    break;
                }
                case CommodityType.FreeDiamonds:
                {
                    this.FreeDiamonds = amount;
                    break;
                }
                case CommodityType.Experience:
                {
                    if (this.ExpPoints >= 2500000)
                    {
                        return;
                    }

                    double finalValue = amount;

                    if (this.Booster.Started)
                    {
                        finalValue *= this.Booster.BoostPackage.Boost;
                    }

                    this.ExpPoints = (int)finalValue;

                    LogicExperienceLevelData experienceLevels = (LogicExperienceLevelData)CSV.Tables.Get(Gamefile.ExperienceLevels).GetDataWithID(this.ExpLevel - 1);

                    if (experienceLevels.ExpPoints <= this.ExpPoints)
                    {
                        this.ExpPoints -= experienceLevels.ExpPoints;
                        this.ExpLevel++;
                    }

                    break;
                }
                default:
                {
                    this.Resources.SetResourceCount(commodity, amount);
                    break;
                }
            }
        }

        /// <summary>
        /// Changes the specified commodity by the specified amount. 
        /// </summary>
        internal bool CommodityChangeCountHelper(CommodityType commodity, int amount)
        {
            switch (commodity)
            {
                case CommodityType.Diamonds:
                {
                    this.SetCommodityCount(CommodityType.Diamonds, this.Diamonds + amount);
                    break;
                }
                case CommodityType.FreeDiamonds:
                {
                    this.SetCommodityCount(CommodityType.FreeDiamonds, this.FreeDiamonds + amount);
                    this.SetCommodityCount(CommodityType.Diamonds, this.Diamonds + amount);
                    break;
                }
                case CommodityType.Experience:
                {
                    if (this.ExpPoints >= 2500000)
                    {
                        return false;
                    }

                    double finalValue = amount;

                    if (this.Booster.Started)
                    {
                        finalValue *= this.Booster.BoostPackage.Boost;
                    }

                    this.ExpPoints += (int)finalValue;

                    LogicExperienceLevelData experienceLevels = (LogicExperienceLevelData)CSV.Tables.Get(Gamefile.ExperienceLevels).GetDataWithID(this.ExpLevel - 1);

                    if (experienceLevels.ExpPoints <= this.ExpPoints)
                    {
                        this.ExpPoints -= experienceLevels.ExpPoints;
                        this.ExpLevel++;
                    }
                        
                    break;
                }
                default:
                {
                    if (amount < 0)
                    {
                        if (this.Resources.GetResourceCount(commodity) < LogicMath.Abs(amount))
                        {
                            return false;
                        }
                    }

                    this.Resources.AddItem((int)commodity, amount); 
                    break;
                }
            }

            return true;
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        internal void Save()
        {
            Avatars.Update(this);
            this.Alliance?.Save();
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
