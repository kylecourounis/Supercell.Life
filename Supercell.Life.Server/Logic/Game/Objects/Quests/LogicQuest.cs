namespace Supercell.Life.Server.Logic.Game.Objects.Quests
{
    using System;

    using Newtonsoft.Json;

    using Supercell.Life.Titan.Logic;
    using Supercell.Life.Titan.Logic.Enums;
    using Supercell.Life.Titan.Logic.Utils;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Titan.Logic.Math;

    [JsonObject(MemberSerialization.OptIn)]
    internal class LogicQuest
    {
        internal LogicClientAvatar Avatar;

        internal LogicQuestData Data;
        internal LogicArrayList<LogicLevel> Levels;

        internal int SublevelMoveCount;

        internal int GoldReward;
        internal int XPReward;

        internal int ReplayLevel;
        internal int ReplayGoldReward;
        internal int ReplayXPReward;

        internal bool IsReplaying;

        internal LogicArrayList<LogicCharacter> Characters;
        internal int CharacterIndex = 1;

        /// <summary>
        /// Gets the global identifier for the <see cref="LogicQuestData"/> instance in this class.
        /// </summary>
        [JsonProperty]
        internal int GlobalID
        {
            get
            {
                return this.Data.GlobalID;
            }
        }

        /// <summary>
        /// Gets the player's current sublevel in this quest.
        /// </summary>
        [JsonProperty]
        internal int Level
        {
            get
            {
                return this.Avatar.NpcProgress.GetCount(this.GlobalID);
            }
        }

        /// <summary>
        /// Gets the number of moves in this <see cref="LogicQuest"/>.
        /// </summary>
        internal int Moves
        {
            get
            {
                return this.Avatar.QuestMoves.GetCount(this.GlobalID);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicQuest"/> class.
        /// </summary>
        internal LogicQuest()
        {
            this.Levels     = new LogicArrayList<LogicLevel>();
            this.Characters = new LogicArrayList<LogicCharacter>();
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        internal void Start(LogicClientAvatar avatar, LogicQuestData data)
        {
            this.Avatar = avatar;
            this.Data   = data;

            this.Avatar.Connection.State = State.Battle;

            int requiredQuest = 6000000; // This default value is the GlobalID of the first quest of the game

            if (!this.Data.RequiredQuest.IsNullOrEmptyOrWhitespace())
            {
                requiredQuest = ((LogicQuestData)CSV.Tables.Get(Gamefile.Quests).GetDataByName(this.Data.RequiredQuest)).GlobalID;
            }

            if (this.Avatar.ExpLevel >= this.Data.RequiredXpLevel && this.Avatar.Energy >= this.Data.Energy)
            {
                this.Avatar.PreviousSpells.Clear();

                if (this.Avatar.NpcProgress.ContainsKey(this.GlobalID) && this.Level == this.Levels.Size)
                {
                    this.IsReplaying = true;
                    this.ReplayLevel = this.Avatar.GameMode.Random.Rand(this.Levels.Size);
                }
                
                this.Avatar.OngoingQuestData = this;
                this.Avatar.CommodityChangeCountHelper(CommodityType.Energy, -this.Data.Energy);

                this.Characters.Clear();
                
                foreach (var hero in this.Avatar.Team.ToObject<int[]>())
                {
                    this.Avatar.HeroQuests.AddItem(hero, 1);
                    this.Characters.Add(new LogicCharacter(this.Avatar, hero));
                }

                this.SetInitialCharacterPositions();
            }
        }

        /// <summary>
        /// Sets the initial character positions.
        /// </summary>
        internal void SetInitialCharacterPositions()
        {
            for (int i = 0; i < this.Characters.Size; i++)
            {
                this.Characters[i].Position = i == 0 ? new LogicVector2(50, 245) : new LogicVector2(this.Characters[i - 1].Position.X + 75, i == 1 ? 250 : 245);
            }
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        internal void Save()
        {
            switch (this.Data.QuestType)
            {
                case "Unlock":
                {
                    this.Avatar.NpcProgress.AddItem(this.GlobalID, 1); 
                    break;
                }
                case "PvP":
                {
                    this.Avatar.NpcProgress.AddItem(this.GlobalID, 1);

                    this.Avatar.AddTrophyScoreHelper(59);
                    this.Avatar.WinBattle();

                    break;
                }
                default:
                {
                    if (this.SublevelMoveCount > 0)
                    {
                        if (this.IsReplaying)
                        {
                            this.GoldReward = this.ReplayGoldReward;
                            this.XPReward   = this.ReplayXPReward;
                            
                            if (this.SublevelMoveCount < this.Avatar.QuestMoves.GetMovesForLevel(this.GlobalID, this.ReplayLevel))
                            { 
                                this.Avatar.QuestMoves.Set(this.GlobalID, this.ReplayLevel, this.SublevelMoveCount);
                            }

                            if (this.Avatar.BonusChestRespawnTimer.ReplayQuest == this.GlobalID)
                            {
                                this.Avatar.BonusChestRespawnTimer.ReplayChestTimes++;

                                if (this.Avatar.BonusChestRespawnTimer.ReplayChestTimes == 5)
                                {
                                    // Create Chest
                                }
                            }
                        }
                        else
                        {
                            if (this.Data.GoldRewardOverride > 0 && this.Data.XpRewardOverride > 0)
                            {
                                this.GoldReward = this.Data.GoldRewardOverride;
                                this.XPReward   = this.Data.XpRewardOverride;
                            }
                            else
                            {
                                int level = Files.CsvHelpers.GlobalID.GetID(this.GlobalID) - 1;

                                if (level < 1)
                                {
                                    level = 1;
                                }

                                LogicExperienceLevelData expLevelData = (LogicExperienceLevelData)CSV.Tables.Get(Gamefile.ExperienceLevels).GetDataByName(level >= 35 ? "35" : LogicStringUtil.IntToString(level));

                                this.GoldReward = expLevelData.DefaultQuestRewardGoldPerEnergy * this.Data.Energy;
                                this.XPReward   = expLevelData.DefaultQuestRewardXpPerEnergy * this.Data.Energy;
                            }

                            if (this.Avatar.NpcProgress.GetCount(this.GlobalID) < this.Levels.Size)
                            {
                                this.Avatar.NpcProgress.AddItem(this.GlobalID, 1);
                                this.Avatar.QuestMoves.AddItem(this.GlobalID, this.Level - 1, this.SublevelMoveCount);

                                this.SublevelMoveCount = 0;
                            }

                            if (this.Level == this.Levels.Size)
                            {
                                if (this.Avatar.QuestMoves.GetCount(this.GlobalID) <= this.Data.GoalMoveCount)
                                {
                                    if (!this.Avatar.Crowns.Contains(this.GlobalID))
                                    {
                                        this.Avatar.Crowns.Add(this.GlobalID);
                                    }
                                }
                            }
                        }

                        int goldDrop = this.Avatar.GameMode.Random.Rand(this.Data.MinGoldDrop, this.Data.MaxGoldDrop); // Best I can do until object collision works

                        this.Avatar.CommodityChangeCountHelper(CommodityType.Gold, this.GoldReward + goldDrop);

                        if (this.Avatar.Items.EnergyRecycler.IsAttached)
                        {
                            this.Avatar.CommodityChangeCountHelper(CommodityType.Energy, 1);
                        }

                        if (this.Avatar.Items.PlunderThunder.IsAttached)
                        {
                            this.Avatar.CommodityChangeCountHelper(CommodityType.Experience, (int)Math.Round(this.XPReward * this.Avatar.Items.PlunderThunder.PercentageMultiplier(1.02, 0.02)));
                        }
                        else
                        {
                            this.Avatar.CommodityChangeCountHelper(CommodityType.Experience, this.XPReward);
                        }

                        this.CreateChest();
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Creates a chest.
        /// </summary>
        internal void CreateChest()
        {
            if (!this.IsReplaying)
            {
                if (this.Level == this.Levels.Size && (this.Data.QuestType != "Unlock" || this.Data.QuestType != "PvP"))
                {
                    Debugger.Debug("Create a chest.");

                    LogicChest chest = new LogicChest(this.Avatar);
                    chest.CreateChest(LogicChest.ChestType.Medium);
                }
                else
                {
                    Debugger.Debug("No chest yet.");
                }
            }
        }
    }
}