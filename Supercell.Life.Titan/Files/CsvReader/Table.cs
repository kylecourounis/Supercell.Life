namespace Supercell.Life.Titan.Files.CsvReader
{
    using NotVisualBasic.FileIO;

    using Supercell.Life.Titan.Logic;

    public class Table
    {
        public readonly LogicArrayList<Column> Columns;
        public readonly LogicArrayList<string> Headers;
        public readonly LogicArrayList<Row> Rows;
        public readonly LogicArrayList<string> Types;

        /// <summary>
        /// Initializes a new instance of the <see cref="Table"/> class.
        /// </summary>
        public Table(string path)
        {
            this.Rows = new LogicArrayList<Row>();
            this.Headers = new LogicArrayList<string>();
            this.Types = new LogicArrayList<string>();
            this.Columns = new LogicArrayList<Column>();
            
            using (CsvTextFieldParser reader = new CsvTextFieldParser(path))
            {
                reader.SetDelimiter(',');

                string[] columns = reader.ReadFields();

                foreach (string column in columns)
                {
                    this.Headers.Add(column);
                    this.Columns.Add(new Column());
                }

                string[] types = reader.ReadFields();

                foreach (string type in types)
                {
                    this.Types.Add(type);
                }

                while (!reader.EndOfData)
                {
                    string[] values = reader.ReadFields();

                    if (!string.IsNullOrEmpty(values[0]))
                    {
                        this.AddRow(new Row(this));
                    }

                    for (int i = 0; i < this.Headers.Count; i++)
                    {
                        this.Columns[i].Add(values[i]);
                    }
                }
            }
        }

        /// <summary>
        /// Gets the row at the specified index.
        /// </summary>
        public Row GetRowAt(int idx)
        {
            return this.Rows[idx];
        }

        /// <summary>
        /// Gets the row count.
        /// </summary>
        public int GetRowCount()
        {
            return this.Rows.Count;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public string GetValue(string name, int level)
        {
            return this.GetValueAt(this.Headers.IndexOf(name), level);
        }

        /// <summary>
        /// Gets the value at the specified column and row.
        /// </summary>
        public string GetValueAt(int column, int row)
        {
            return (column > -1 && row > -1) ? this.Columns[column].Get(row) : null;
        }

        /// <summary>
        /// Adds the row.
        /// </summary>
        public void AddRow(Row row)
        {
            this.Rows.Add(row);
        }

        /// <summary>
        /// Gets the array size at the specified row and column index.
        /// </summary>
        public int GetArraySizeAt(Row row, int columnIdx)
        {
            int idx = this.Rows.IndexOf(row);
            if (idx == -1)
            {
                return 0;
            }

            Column column = this.Columns[columnIdx];
            int nextOffset = 0;
            if (idx + 1 >= this.Rows.Count)
            {
                nextOffset = column.GetSize();
            }
            else
            {
                Row nextRow = this.Rows[idx + 1];
                nextOffset = nextRow.Offset;
            }

            int offset = row.Offset;
            return Column.GetArraySize(offset, nextOffset);
        }

        /// <summary>
        /// Gets the index of the column by its name.
        /// </summary>
        public int GetColumnIndexByName(string name)
        {
            return this.Headers.IndexOf(name);
        }

        /// <summary>
        /// Gets the name of the column at the specified index.
        /// </summary>
        public string GetColumnName(int idx)
        {
            return this.Headers[idx];
        }

        /// <summary>
        /// Gets the column row count.
        /// </summary>
        public int GetColumnRowCount()
        {
            return this.Columns.Count > 0 ? this.Columns[0].GetSize() : 0;
        }
    }
}