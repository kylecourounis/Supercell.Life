namespace Supercell.Life.Server.Logic.Avatar.Slots
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Logic.Avatar.Items;
    using Supercell.Life.Titan.Logic.Json;

    internal class LogicQuestMoves : LogicDataSlot
    {
        internal LogicQuestMoves(LogicClientAvatar avatar) : base(avatar)
        {
            // LogicQuestMoves.
        }

        internal override void Initialize()
        {
        }

        internal override void Encode(ByteStream stream)
        {
            stream.WriteInt(this.Count);

            foreach (Item item in this.Values)
            {
                stream.WriteInt(item.Id);
                stream.WriteInt(1);
                stream.WriteInt(item.Count);
            }
        }

        internal LogicJSONArray QuestMovesVisit
        {
            get
            {
                LogicJSONArray array = new LogicJSONArray();

                foreach (Item quest in this.Values)
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
