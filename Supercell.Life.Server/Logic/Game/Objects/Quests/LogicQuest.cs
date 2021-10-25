namespace Supercell.Life.Server.Logic.Game.Objects.Quests
{
    using System.Linq;

    using Newtonsoft.Json;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Titan.Logic;
    using Supercell.Life.Titan.Logic.Enums;
    using Supercell.Life.Titan.Logic.Json;
    using Supercell.Life.Titan.Logic.Utils;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Logic.Avatar.Slots;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Logic.Game.Objects.Quests.Items;

    [JsonObject(MemberSerialization.OptIn)]
    internal class LogicQuest
    {
        internal LogicClientAvatar Avatar;

        internal LogicQuestData Data;
        internal LogicArrayList<LogicLevel> Levels;

        internal int SublevelMoveCount;

        internal int GoldReward;
        internal int XPReward;

        internal int ReplayGoldReward;
        internal int ReplayXPReward;

        [JsonProperty] internal string Name;

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
            this.Levels = new LogicArrayList<LogicLevel>();
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        internal void Start(LogicClientAvatar avatar, LogicQuestData data)
        {
            this.Avatar = avatar;
            this.Data   = data;

            int requiredQuest = 6000000; // This default value is the GlobalID of the first quest of the game

            if (!this.Data.RequiredQuest.IsNullOrEmptyOrWhitespace())
            {
                requiredQuest = ((LogicQuestData)CSV.Tables.Get(Gamefile.Quests).GetDataByName(this.Data.RequiredQuest)).GlobalID;
            }

            if (this.Avatar.NpcProgress.ContainsKey(requiredQuest) && this.Avatar.ExpLevel >= this.Data.RequiredXpLevel && this.Avatar.Energy >= this.Data.Energy)
            {
                this.Avatar.OngoingQuestData = this;
                this.Avatar.Connection.State = State.Battle;
                this.Avatar.Energy          -= this.Data.Energy;
                
                foreach (var hero in this.Avatar.Team.ToObject<int[]>())
                {
                    this.Avatar.HeroQuests.AddItem(hero, 1);
                }
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
                    this.Avatar.WinBattle(59);

                    break;
                }
                default:
                {
                    if (!this.Avatar.NpcProgress.Crowns.Contains(this.GlobalID))
                    {
                        this.Avatar.QuestMoves.AddItem(this.GlobalID, this.SublevelMoveCount);
                    }
                    else
                    {
                        this.Avatar.AddGold(this.ReplayGoldReward);
                        this.Avatar.AddXP(this.ReplayXPReward);
                    }
                    
                    if (this.SublevelMoveCount > 0)
                    {
                        if (this.Avatar.NpcProgress.GetCount(this.GlobalID) < this.Levels.Size)
                        {
                            this.Avatar.NpcProgress.AddItem(this.GlobalID, 1);
                            this.SublevelMoveCount = 0;
                        }
                        
                        this.Avatar.AddGold(this.Data.GoldRewardOverride);

                        if (this.Avatar.Items.IsAttached(LogicItems.EnergyRecycler))
                        {
                            this.Avatar.Energy += 1;
                        }

                        if (this.Avatar.Items.IsAttached(LogicItems.PlunderThunder))
                        {
                            this.Avatar.AddXP(this.Data.XpRewardOverride * this.Avatar.Items.PlunderThunderPercentage);
                        }
                        else
                        {
                            this.Avatar.AddXP(this.Data.XpRewardOverride);
                        }
                    }

                    if (this.Level == this.Levels.Size)
                    {
                        if (this.Moves <= this.Data.GoalMoveCount)
                        {
                            this.Avatar.NpcProgress.Crowns.Add(this.GlobalID);
                        }
                    }

                    this.CalculateChestLoot();

                    break;
                }
            }
        }

        /// <summary>
        /// Calculates the chest loot.
        /// Very much WIP.
        /// </summary>
        internal void CalculateChestLoot()
        {
            if (this.Level == this.Levels.Size && (this.Data.QuestType != "Unlock" || this.Data.QuestType != "PvP"))
            {
                Debugger.Debug("Create a chest.");
            }
            else
            {
                Debugger.Debug("No chest yet.");
            }
        }

        internal class LogicLevel
        {
            private readonly LogicJSONArray BattlesJson;

            internal LogicQuest Quest;

            internal LogicArrayList<Battle> Battles;
            internal int Version;

            internal int CurrentBattle;

            /// <summary>
            /// Initializes a new instance of the <see cref="LogicLevel"/> class.
            /// </summary>
            internal LogicLevel(LogicQuest quest, LogicJSONObject json)
            {
                this.Quest   = quest;
                this.Battles = new LogicArrayList<Battle>();

                this.BattlesJson = json.GetJsonArray("battles");
                this.Version     = json.GetJsonNumber("ver").GetIntValue();

                for (int i = 0; i < this.BattlesJson.Size; i++)
                {
                    this.Battles.Add(new Battle(this, this.BattlesJson.GetJsonObject(i)));
                }
            }

            internal class Battle
            {
                internal LogicLevel Level;

                internal LogicArrayList<Enemy> Enemies;
                internal LogicArrayList<Obstacle> Obstacles;

                internal int BGIndex;
                internal int FGIndex;

                internal int EnemiesKilled;

                /// <summary>
                /// Gets this instance of <see cref="Battle"/> as a <see cref="LogicJSONObject"/>.
                /// </summary>
                internal LogicJSONObject JSON
                {
                    get;
                }

                /// <summary>
                /// Gets a value indicating whether this <see cref="Battle"/> is current battle.
                /// </summary>
                internal bool IsCurrentBattle
                {
                    get
                    {
                        return this.Level.CurrentBattle == this.Level.Battles.IndexOf(this);
                    }
                }

                /// <summary>
                /// Gets a value indicating whether this <see cref="Battle"/> is complete.
                /// </summary>
                internal bool IsCompleted
                {
                    get
                    {
                        return this.Enemies.Size == 0;
                    }
                }

                /// <summary>
                /// Initializes a new instance of the <see cref="Battle"/> class.
                /// </summary>
                internal Battle(LogicLevel level, LogicJSONObject battle)
                {
                    this.Level     = level;
                    this.JSON      = battle;

                    this.InitializeLists();

                    this.BGIndex   = this.JSON.GetJsonNumber("bg_index").GetIntValue();
                    this.FGIndex   = this.JSON.GetJsonNumber("fg_index").GetIntValue();
                }

                /// <summary>
                /// Initializes the lists of enemies and obstacles in the battle.
                /// </summary>
                private void InitializeLists()
                {
                    this.Enemies   = new LogicArrayList<Enemy>();
                    this.Obstacles = new LogicArrayList<Obstacle>();

                    LogicJSONArray enemies = this.JSON.GetJsonArray("enemies");

                    if (enemies != null)
                    {
                        for (int i = 0; i < enemies.Size; i++)
                        {
                            this.Enemies.Add(new Enemy(enemies.GetJsonObject(i)));
                        }
                    }

                    LogicJSONArray obstacles = this.JSON.GetJsonArray("obstacles");

                    if (obstacles != null)
                    {
                        for (int i = 0; i < obstacles.Size; i++)
                        {
                            this.Obstacles.Add(new Obstacle(obstacles.GetJsonObject(i)));
                        }
                    }
                }

                /// <summary>
                /// Checks whether the character at the specified X and Y coordinates collided with an enemy in the battle.
                /// </summary>
                internal void CheckCollision(int characterX, int characterY)
                {
                    if (this.Enemies.Size > 0)
                    {
                        for (int i = 0; i < this.Enemies.Size; i++)
                        {
                            int enemyX = this.Enemies[i].X;
                            int enemyY = this.Enemies[i].Y;

                            Debugger.Debug($"{characterX} ?= {enemyX} && {characterY} ?= {enemyY}");

                            if (characterX == enemyX && characterY == enemyY)
                            {
                                Debugger.Debug(this.Enemies[i].Data);
                                
                                this.Enemies.RemoveAt(i);
                                this.EnemiesKilled++;
                            }
                        }
                    }

                    Debugger.Debug(this.Level.CurrentBattle);

                    if (this.IsCompleted)
                    {
                        this.Level.CurrentBattle++;
                    }
                }
            }
        }
    }
}