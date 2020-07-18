namespace Supercell.Life.Titan.Files.CsvReader
{
    using System.Linq;

    using Supercell.Life.Titan.Logic;

    internal class Column
    {
        internal readonly LogicArrayList<string> Values;

        /// <summary>
        /// Initializes a new instance of the <see cref="Column"/> class.
        /// </summary>
        internal Column()
        {
            this.Values = new LogicArrayList<string>();
        }

        /// <summary>
        /// Gets the size of the array.
        /// </summary>
        internal static int GetArraySize(int offset, int nOffset)
        {
            return nOffset - offset;
        }

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        internal void Add(string value)
        {
            if (value == null)
            {
                this.Values.Add(this.Values.Count > 0 ? this.Values.Last() : string.Empty);
            }
            else
            {
                this.Values.Add(value);
            }
        }

        /// <summary>
        /// Gets the specified row.
        /// </summary>
        internal string Get(int row)
        {
            return (this.Values.Count > row) ? this.Values[row] : null;
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        internal int GetSize()
        {
            return this.Values.Count;
        }
    }
}