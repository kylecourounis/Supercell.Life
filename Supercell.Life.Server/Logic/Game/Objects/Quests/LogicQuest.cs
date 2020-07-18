namespace Supercell.Life.Server.Logic.Game.Objects.Quests
{
    using System.Linq;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using Supercell.Life.Titan.Logic;
    using Supercell.Life.Titan.Logic.Enums;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Avatar;

    internal class LogicQuest
    {
        internal LogicClientAvatar Avatar;

        internal LogicQuestData Data;
        
        internal string Name;
        internal LogicArrayList<LogicLevel> Levels;

        internal int Moves;

        /// <summary>
        /// Gets the global identifier for the <see cref="LogicQuestData"/> instance in this class.
        /// </summary>
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
        /// Starts this instance.
        /// </summary>
        internal void Start()
        {
            if (this.Avatar.ExpLevel >= this.Data.RequiredXpLevel && this.Avatar.Energy >= this.Data.Energy)
            {
                this.Avatar.CurrentQuest     = this;
                this.Avatar.OngoingQuestData = this.GlobalID;
                this.Avatar.Connection.State = State.Battle;
                this.Avatar.Energy          -= this.Data.Energy;

                if (!this.Avatar.NpcProgress.ContainsKey(this.GlobalID))
                {
                    this.SaveCurrentArea(this.Name, 0);
                }
                else if (this.Avatar.NpcProgress.ContainsKey(this.GlobalID) && !this.Avatar.NpcProgress.Crowns.Contains(this.GlobalID))
                {
                    this.SaveCurrentArea(this.Name, 0);
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

                    this.SaveCurrentArea(this.Name, this.Avatar.CurrentArea["Level"].ToObject<int>() + 1);

                    break;
                }
                case "PvP":
                {
                    this.Avatar.WinBattle(59);
                    break;
                }
                default:
                {
                    this.Avatar.QuestMoves.Set(this.GlobalID, this.Moves);

                    if (this.Avatar.NpcProgress.GetCount(this.GlobalID) < this.Levels.Size)
                    {
                        this.Avatar.NpcProgress.AddItem(this.GlobalID, 1);
                    }

                    if (this.Moves <= this.Data.GoalMoveCount)
                    {
                        this.Avatar.NpcProgress.Crowns.Add(this.GlobalID);
                    }

                    if (this.Avatar.ItemAttachedTo.Values.Any(item => item.Id.Equals(this.Avatar.ItemLevels.Get(37000000).Id) && this.Avatar.Team.Any(hero => hero.ToObject<int>().Equals(item.Count))))
                    {
                        this.Avatar.Energy += 1;
                    }

                    this.SaveCurrentArea(this.Name, this.Avatar.CurrentArea["Level"].ToObject<int>() + 1);
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
            if (this.Avatar.CurrentArea["Level"].ToObject<int>() == this.Levels.Count && (this.Data.QuestType != "Unlock" || this.Data.QuestType != "PvP"))
            {
                Debugger.Debug("Create a chest.");

                var areaNames      = LogicQuests.Quests;
                int currentAreaIdx = areaNames.Values.ToList().FindIndex(value => value.Name.Equals(this.Name));
                LogicQuest area    = areaNames[currentAreaIdx + 1];

                this.Avatar.OngoingQuestData = area.GlobalID;
                this.SaveCurrentArea(area.Name, 0);
            }
            else
            {
                Debugger.Debug("No chest yet.");
            }
        }

        /// <summary>
        /// Saves the current area.
        /// </summary>
        internal void SaveCurrentArea(string name, int level)
        {
            this.Avatar.CurrentArea["Name"]  = name;
            this.Avatar.CurrentArea["Level"] = level;
        }

        internal class LogicLevel
        {
            [JsonProperty("battles")] internal JArray Battles;
            [JsonProperty("ver")]     internal int Version;

            /// <summary>
            /// Initializes a new instance of the <see cref="LogicLevel"/> class.
            /// </summary>
            internal LogicLevel()
            {
                this.Battles = new JArray();

                foreach (var battle in this.Battles)
                {
                    this.Battles.Add(JsonConvert.DeserializeObject<Battle>(battle.ToString()));
                }
            }

            internal class Battle
            {
                [JsonProperty("enemies")]   internal JArray Enemies;
                [JsonProperty("obstacles")] internal JArray Obstacles;

                [JsonProperty("bg_index")]  internal int BGIndex;
                [JsonProperty("fg_index")]  internal int FGIndex;
            }
        }
    }
}