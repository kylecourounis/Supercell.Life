namespace Supercell.Life.Server.Logic.Avatar.Slots
{
    using Supercell.Life.Server.Logic.Game;
    using Supercell.Life.Titan.Logic.Json;

    internal class LogicNpcProgress : LogicDataSlots
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicNpcProgress"/> class.
        /// </summary>
        internal LogicNpcProgress(LogicClientAvatar avatar) : base(avatar, 512)
        {
            // LogicNpcProgress
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal override void Initialize()
        {
            if (!this.ContainsKey(Globals.StartingQuest.GlobalID))
            {
                this.AddItem(Globals.StartingQuest.GlobalID, 0);
            }
        }

        /// <summary>
        /// Saves the ongoing quest data to the specified <see cref="LogicJSONObject"/>.
        /// </summary>
        internal void Save(LogicJSONObject json)
        {
            json.Put("ongoing_quest_data", new LogicJSONNumber(this.Avatar.OngoingQuestData.GlobalID));
            json.Put("ongoing_level_idx", new LogicJSONNumber(this.Avatar.OngoingQuestData.Level));
            json.Put("ongoing_event_data", new LogicJSONNumber());

            if (this.Avatar.Variables.Get(LogicVariables.IgnoreOngoingQuest.GlobalID) != null)
            {
                json.Put("IgnoreOngoingQuest", new LogicJSONNumber(this.Avatar.Variables.Get(LogicVariables.IgnoreOngoingQuest.GlobalID).Id));
            }
            
            // json.Put("ongoing_level", this.OngoingLevel);
        }

        /// <summary>
        /// Gets the ongoing level data as a <see cref="LogicJSONObject"/>.
        /// </summary>
        internal LogicJSONObject OngoingLevel
        {
            get
            {
                LogicJSONObject retVal = new LogicJSONObject();

                LogicJSONArray array = new LogicJSONArray();

                int enemyKillcount = 0;

                foreach (var battle in this.Avatar.Quests[this.Avatar.OngoingQuestData.GlobalID].Levels[this.Avatar.OngoingQuestData.Level].Battles)
                {
                    enemyKillcount += battle.EnemiesKilled;
                    array.Add(battle.JSON);
                }

                retVal.Put("battles", array);

                retVal.Put("move_count", new LogicJSONNumber(this.Avatar.OngoingQuestData.Moves));
                retVal.Put("sublevel_move_count", new LogicJSONNumber(this.Avatar.OngoingQuestData.SublevelMoveCount));
                retVal.Put("enemy_killcount", new LogicJSONNumber(enemyKillcount));
                retVal.Put("current_battle", new LogicJSONNumber(this.Avatar.Quests[this.Avatar.OngoingQuestData.GlobalID].Levels[this.Avatar.OngoingQuestData.Level].CurrentBattle));
                retVal.Put("level_index", new LogicJSONNumber(this.Avatar.OngoingQuestData.Level));
                retVal.Put("drops", new LogicJSONArray());
                retVal.Put("ver", new LogicJSONNumber(1));

                return retVal;
            }
        }

        /// <summary>
        /// Gets the visited quest progress as a <see cref="LogicJSONArray"/>.
        /// </summary>
        internal LogicJSONArray QuestProgressVisit
        {
            get
            {
                LogicJSONArray array = new LogicJSONArray();

                foreach (var quest in this.Values)
                {
                    array.Add(quest.SaveToJSON());
                }

                return array;
            }
        }
    }
}