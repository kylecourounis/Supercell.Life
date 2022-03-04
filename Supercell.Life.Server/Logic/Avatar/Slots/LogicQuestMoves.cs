namespace Supercell.Life.Server.Logic.Avatar.Slots
{
    using System.Collections.Generic;
    using System.Linq;
    
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Json;
    
    internal class LogicQuestMoves : List<(int GlobalID, int Level, int Count)>
    {
        private readonly LogicClientAvatar Avatar;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicQuestMoves"/> class.
        /// </summary>
        internal LogicQuestMoves(LogicClientAvatar avatar)
        {
            this.Avatar = avatar;
        }
        
        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal void Encode(ByteStream stream)
        {
            stream.WriteInt(this.Count);

            foreach (var (id, lvl, count) in this)
            {
                stream.WriteInt(id);
                stream.WriteInt(lvl);
                stream.WriteInt(count);
            }
        }

        /// <summary>
        /// Adds the level and move count for the quest with the specified GlobalID.
        /// </summary>
        internal void AddItem(int id, int lvl, int count)
        {
            this.Add((id, lvl, count));
        }

        /// <summary>
        /// Sets the level and move count for the quest with the specified GlobalID.
        /// </summary>
        internal void Set(int id, int lvl, int count)
        {
            int idx = this.FindIndex(value => id == value.GlobalID);

            var moves = this[idx];

            moves.GlobalID = id;
            moves.Level = lvl;
            moves.Count = count;

            this[idx] = moves;
        }

        /// <summary>
        /// Gets all of the move data for the quest with the specified GlobalID.
        /// </summary>
        internal IEnumerable<(int, int, int)> Get(int id)
        {
            return this.FindAll(value => id == value.GlobalID);
        }

        /// <summary>
        /// Gets the number of moves for the level within the quest with the specified GlobalID.
        /// </summary>
        internal int GetMovesForLevel(int id, int lvl)
        {
            return this[this.FindIndex(value => id == value.GlobalID && lvl == value.Level)].Count;
        }

        /// <summary>
        /// Gets the total number of moves for the quest with the specified GlobalID.
        /// </summary>
        internal int GetCount(int id)
        {
            return this.Get(id).Sum(value => value.Item3);
        }
        
        /// <summary>
        /// Gets the quest moves for the visited player as a <see cref="LogicJSONArray"/>.
        /// </summary>
        internal LogicJSONArray QuestMovesVisit
        {
            get
            {
                LogicJSONArray array = new LogicJSONArray();

                foreach (var (id, lvl, count) in this)
                {
                    LogicJSONObject jsonObj = new LogicJSONObject();

                    jsonObj.Put("id", new LogicJSONNumber(id));
                    jsonObj.Put("lv", new LogicJSONNumber(lvl));
                    jsonObj.Put("cnt", new LogicJSONNumber(count));

                    array.Add(jsonObj);
                }

                return array;
            }
        }
    }
}
