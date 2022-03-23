namespace Supercell.Life.Titan.Files.CsvReader
{
    using System.Linq;

    using Supercell.Life.Titan.Logic;

    public class Column
    {
        public readonly LogicArrayList<string> Values;

        /// <summary>
        /// Initializes a new instance of the <see cref="Column"/> class.
        /// </summary>
        public Column()
        {
            this.Values = new LogicArrayList<string>();
        }

        /// <summary>
        /// Gets the size of the array.
        /// </summary>
        public static int GetArraySize(int offset, int nOffset)
        {
            return nOffset - offset;
        }

        /// <summary>
        /// Adds the specified value.
        /// </summary>
        public void Add(string value)
        {
            if (value == null)
            {
                this.Values.Add(this.Values.Size > 0 ? this.Values.Last() : string.Empty);
            }
            else
            {
                this.Values.Add(value);
            }
        }

        /// <summary>
        /// Gets the specified row.
        /// </summary>
        public string Get(int row)
        {
            return (this.Values.Size > row) ? this.Values[row] : null;
        }

        /// <summary>
        /// Gets the size.
        /// </summary>
        public int GetSize()
        {
            return this.Values.Size;
        }
    }
}