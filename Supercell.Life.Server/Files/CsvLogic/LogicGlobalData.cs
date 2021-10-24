namespace Supercell.Life.Server.Files.CsvLogic
{
    using Supercell.Life.Titan.Files.CsvReader;
    using Supercell.Life.Titan.Logic;

    using Supercell.Life.Server.Files.CsvHelpers;

    internal class LogicGlobalData : LogicData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicGlobalData"/> class.
        /// </summary>
        public LogicGlobalData(Row row, LogicDataTable dataTable) : base(row, dataTable)
        {
            this.CreateReferences(this, row);
        }

        public int NumberValue
        {
            get; set;
        }

        public bool BooleanValue
        {
            get; set;
        }

        public string TextValue
        {
            get; set;
        }

        public LogicArrayList<int> NumberArray
        {
            get; set;
        }

        public LogicArrayList<string> StringArray
        {
            get; set;
        }
    }
}
