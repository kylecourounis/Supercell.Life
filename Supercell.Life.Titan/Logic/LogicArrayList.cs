namespace Supercell.Life.Titan.Logic
{
    using System.Collections.Generic;

    public class LogicArrayList<T> : List<T>
    {
        /// <summary>
        /// Gets the size of this list.
        /// </summary>
        public int Size
        {
            get
            {
                return this.Count;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicArrayList{T}"/> class.
        /// </summary>
        public LogicArrayList()
        {
            // LogicArrayList.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicArrayList{T}"/> class.
        /// </summary>
        public LogicArrayList(int capacity) : base(capacity)
        {
            // LogicArrayList.
        }
    }
}