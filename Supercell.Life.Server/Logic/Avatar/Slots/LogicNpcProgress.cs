namespace Supercell.Life.Server.Logic.Avatar.Slots
{
    using Supercell.Life.Titan.Logic;
    using Supercell.Life.Titan.Logic.Json;

    using Newtonsoft.Json;

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
            json.Put("ongoing_quest_data", new LogicJSONNumber(this.Avatar.OngoingQuestData));
            json.Put("ongoing_level_idx", new LogicJSONNumber(this.Avatar.OngoingQuestData));
            json.Put("ongoing_event_data", new LogicJSONNumber());

            if (this.Avatar.Variables.Get(LogicVariables.IgnoreOngoingQuest.GlobalID) != null)
            {
                json.Put("IgnoreOngoingQuest", new LogicJSONNumber(this.Avatar.Variables.Get(LogicVariables.IgnoreOngoingQuest.GlobalID).Id));
            }

            // Json.Put("ongoing_level", new LogicJSONObject());
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