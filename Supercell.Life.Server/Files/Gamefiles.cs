namespace Supercell.Life.Server.Files
{
    using System.Collections.Generic;

    using Supercell.Life.Titan.Files.CsvReader;

    using Supercell.Life.Server.Files.CsvHelpers;
    using Supercell.Life.Server.Logic.Enums;

    internal class Gamefiles
    {
        internal readonly Dictionary<int, LogicDataTable> DataTables;

        /// <summary>
        /// Initializes a new instance of the <see cref="Gamefiles"/> class.
        /// </summary>
        internal Gamefiles()
        {
            this.DataTables = new Dictionary<int, LogicDataTable>(CSV.Gamefiles.Count);
        }

        /// <summary>
        /// Gets the <see cref="LogicDataTable"/> at the specified index.
        /// </summary>
        internal LogicDataTable Get(int index)
        {
            return this.DataTables[index];
        }

        /// <summary>
        /// Gets the <see cref="LogicDataTable"/> at the specified index.
        /// </summary>
        internal LogicDataTable Get(Gamefile index)
        {
            return this.DataTables[(int)index];
        }

        /// <summary>
        /// Gets the data with the global identifier.
        /// </summary>
        internal LogicData GetWithGlobalID(int globalID)
        {
            int classId    = globalID / 1000000;
            int instanceId = globalID % 1000000;

            if (this.DataTables.TryGetValue(classId, out LogicDataTable table))
            {
                if (table.Datas.Size > instanceId)
                {
                    return table.Datas[instanceId];
                }
            }

            return null;
        }

        /// <summary>
        /// Initializes the specified table.
        /// </summary>
        internal void Initialize(Table table, Gamefile gamefile)
        {
            this.Initialize(table, (int)gamefile);
        }

        /// <summary>
        /// Initializes the specified table.
        /// </summary>
        internal void Initialize(Table table, int index)
        {
            this.DataTables.Add(index, new LogicDataTable(table, index));
        }
    }
}