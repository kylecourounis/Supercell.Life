﻿namespace Supercell.Life.Server.Logic.Avatar.Slots
{
    using Newtonsoft.Json;

    using Supercell.Life.Server.Logic.Game;
    using Supercell.Life.Titan.Logic;
    using Supercell.Life.Titan.Logic.Json;

    using Supercell.Life.Server.Logic.Game.Objects.Quests;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class LogicNpcProgress : LogicDataSlot
    {
        [JsonProperty] internal LogicArrayList<int> Crowns;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicNpcProgress"/> class.
        /// </summary>
        internal LogicNpcProgress(LogicClientAvatar avatar) : base(avatar, 512)
        {
            this.Crowns = new LogicArrayList<int>();
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

            /* foreach (QuestData Data in CSV.Tables.Get(Gamefile.Quests).Datas.Cast<QuestData>().Where(Data => Data.QuestType == "Unlock"))
            {
                this.AddItem(new Item(Data.GlobalID, 1));
            } */
        }

        /// <summary>
        /// Saves the ongoing quest data to the specified <see cref="LogicJSONObject"/>.
        /// </summary>
        internal void Save(LogicJSONObject json)
        {
            json.Put("ongoing_quest_data", new LogicJSONNumber(this.Avatar.OngoingQuestData.GlobalID));
            json.Put("ongoing_level_idx", new LogicJSONNumber(this.Avatar.OngoingQuestData.GlobalID));
            json.Put("ongoing_event_data", new LogicJSONNumber());

            if (this.Avatar.Variables.Get(LogicVariables.IgnoreOngoingQuest.GlobalID) != null)
            {
                json.Put("IgnoreOngoingQuest", new LogicJSONNumber(this.Avatar.Variables.Get(LogicVariables.IgnoreOngoingQuest.GlobalID).Id));
            }

            var ongoingLvl = this.OngoingLevel;

            Debugger.Debug(ongoingLvl);

            json.Put("ongoing_level", ongoingLvl);
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

                foreach (var quest in LogicQuests.Quests[this.Avatar.OngoingQuestData.GlobalID].Levels)
                {
                    foreach (var battle in quest.Battles.ToObject<LogicArrayList<LogicQuest.LogicLevel.Battle>>())
                    {
                        array.Add(new LogicJSONString(JsonConvert.SerializeObject(battle).Replace("\\", string.Empty)));
                    }
                }

                retVal.Put("battles", array);

                retVal.Put("move_count", new LogicJSONNumber(this.Avatar.QuestMoves.GetCount(this.Avatar.OngoingQuestData.GlobalID)));
                retVal.Put("sublevel_move_count", new LogicJSONNumber(this.Avatar.OngoingQuestData.Moves));
                retVal.Put("enemy_killcount", new LogicJSONNumber());
                retVal.Put("current_battle", new LogicJSONNumber());
                retVal.Put("level_index", new LogicJSONNumber(this.Avatar.OngoingQuestData.Level));
                retVal.Put("drops", new LogicJSONNumber());
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
                    LogicJSONObject jsonObj = new LogicJSONObject();

                    jsonObj.Put("id", new LogicJSONNumber(quest.Id));
                    jsonObj.Put("cnt", new LogicJSONNumber(quest.Count));

                    array.Add(jsonObj);
                }

                return array;
            }
        }
    }
}