namespace Supercell.Life.Server.Files.CsvLogic
{
    using Supercell.Life.Titan.Files.CsvReader;

    using Supercell.Life.Server.Files.CsvHelpers;

    internal class LogicVariableData : LogicData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicVariableData"/> class.
        /// </summary>
        public LogicVariableData(Row row, LogicDataTable dataTable) : base(row, dataTable)
        {
            this.CreateReferences(this, row);
        }

        public int Value
        {
            get; set;
        }
    }
}
