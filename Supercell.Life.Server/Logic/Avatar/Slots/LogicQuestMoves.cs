namespace Supercell.Life.Server.Logic.Avatar.Slots
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Json;

    internal class LogicQuestMoves : LogicDataSlots
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicQuestMoves"/> class.
        /// </summary>
        internal LogicQuestMoves(LogicClientAvatar avatar) : base(avatar)
        {
            // LogicQuestMoves.
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal override void Initialize()
        {
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal override void Encode(ByteStream stream)
        {
            stream.WriteInt(this.Count);

            foreach (var slot in this.Values)
            {
                stream.WriteInt(slot.Id);
                stream.WriteInt(1);
                stream.WriteInt(slot.Count);
            }
        }

        /// <summary>
        /// Gets the quest moves for the visited player as a <see cref="LogicJSONArray"/>.
        /// </summary>
        internal LogicJSONArray QuestMovesVisit
        {
            get
            {
                LogicJSONArray array = new LogicJSONArray();

                foreach (var quest in this.Values)
                {
                    LogicJSONObject jsonObj = new LogicJSONObject();

                    jsonObj.Put("id", new LogicJSONNumber(quest.Id));
                    jsonObj.Put("lv", new LogicJSONNumber(1));
                    jsonObj.Put("cnt", new LogicJSONNumber(quest.Count));

                    array.Add(jsonObj);
                }

                return array;
            }
        }
    }
}
