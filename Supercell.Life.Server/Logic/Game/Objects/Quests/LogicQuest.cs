namespace Supercell.Life.Server.Logic.Game.Objects.Quests
{
    using System.Linq;

    using Newtonsoft.Json;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Titan.Logic;
    using Supercell.Life.Titan.Logic.Enums;
    using Supercell.Life.Titan.Logic.Json;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Logic.Enums;

    [JsonObject(MemberSerialization.OptIn)]
    internal class LogicQuest
    {
        internal LogicClientAvatar Avatar;

        internal LogicQuestData Data;
        internal LogicArrayList<LogicLevel> Levels;

        internal int Moves;

        [JsonProperty] internal string Name;
        [JsonProperty] internal int Level;

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
        /// Initializes a new instance of the <see cref="LogicQuest"/> class.
        /// </summary>
        internal LogicQuest()
        {
            this.Levels = new LogicArrayList<LogicLevel>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicQuest"/> class.
        /// </summary>
        internal LogicQuest(LogicClientAvatar avatar, LogicQuestData data)
        {
            this.Avatar = avatar;
            this.Data   = data;
            this.Levels = new LogicArrayList<LogicLevel>();
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        internal void Start()
        {
            var requiredQuest = ((LogicQuestData)CSV.Tables.Get(Gamefile.Quests).GetDataByName(this.Data.RequiredQuest)).GlobalID;

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
            this.Avatar.AddGold(this.Data.GoldRewardOverride);
            this.Avatar.AddXP(this.Data.XpRewardOverride);

            switch (this.Data.QuestType)
            {
                case "Unlock":
                {
                    if (this.Avatar.NpcProgress.GetCount(this.GlobalID) < this.Levels.Size)
                    {
                        this.Avatar.NpcProgress.AddItem(this.GlobalID, 1);
                    }

                    break;
                }
                case "PvP":
                {
                    this.Avatar.GameMode.WinBattle(59);
                    break;
                }
                default:
                {
                    this.Avatar.QuestMoves.Set(this.GlobalID, this.Moves);

                    if (this.Moves > 0)
                    {
                        if (this.Avatar.NpcProgress.GetCount(this.GlobalID) < this.Levels.Size)
                        {
                            this.Avatar.NpcProgress.AddItem(this.GlobalID, 1);
                            this.Level = this.Avatar.NpcProgress.GetCount(this.GlobalID);
                        }
                        
                        if (this.Moves <= this.Data.GoalMoveCount)
                        {
                            this.Avatar.NpcProgress.Crowns.Add(this.GlobalID);
                        }

                        if (this.Avatar.ItemAttachedTo.Values.Any(item => item.Id.Equals(this.Avatar.ItemLevels.Get(37000000).Id) && this.Avatar.Team.Any(hero => hero.ToObject<int>().Equals(item.Count))))
                        {
                            this.Avatar.Energy += 1;
                        }
                    }

                    this.Moves = 0;

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
            if (this.Level == this.Levels.Count && (this.Data.QuestType != "Unlock" || this.Data.QuestType != "PvP"))
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
            internal LogicArrayList<Battle> Battles;
            internal LogicJSONArray BattlesJson;
            internal int Version;

            /// <summary>
            /// Initializes a new instance of the <see cref="LogicLevel"/> class.
            /// </summary>
            internal LogicLevel(LogicJSONObject json)
            {
                this.Battles = new LogicArrayList<Battle>();

                this.BattlesJson = json.GetJsonArray("battles");
                this.Version     = json.GetJsonNumber("ver").GetIntValue();
                
                for (int i = 0; i < this.BattlesJson.Size; i++)
                {
                    this.Battles.Add(new Battle(this.BattlesJson.GetJsonObject(i)));
                }
            }

            internal class Battle
            {
                internal LogicJSONArray Enemies;
                internal LogicJSONArray Obstacles;

                internal int BGIndex;
                internal int FGIndex;

                /// <summary>
                /// Gets this instance of <see cref="Battle"/> as a <see cref="LogicJSONObject"/>.
                /// </summary>
                internal LogicJSONObject JSON
                {
                    get;
                }

                /// <summary>
                /// Initializes a new instance of the <see cref="Battle"/> class.
                /// </summary>
                internal Battle(LogicJSONObject battle)
                {
                    this.JSON      = battle;
                    this.Enemies   = this.JSON.GetJsonArray("enemies");
                    this.Obstacles = this.JSON.GetJsonArray("obstacles");
                    this.BGIndex   = this.JSON.GetJsonNumber("bg_index").GetIntValue();
                    this.FGIndex   = this.JSON.GetJsonNumber("fg_index").GetIntValue();
                }
            }
        }
    }
}