namespace Supercell.Life.Server.Files.CsvLogic
{
    using Supercell.Life.Titan.Files.CsvReader;

    using Supercell.Life.Server.Files.CsvHelpers;

    internal class LogicResourcePackData : LogicData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicResourcePackData"/> class.
        /// </summary>
        public LogicResourcePackData(Row row, LogicDataTable dataTable) : base(row, dataTable)
        {
            this.CreateReferences(this, row);
        }

        public string TID
        {
            get; set;
        }

        public string Resource
        {
            get; set;
        }

        public int Amount
        {
            get; set;
        }

        public string IconSWF
        {
            get; set;
        }

        public string IconExportName
        {
            get; set;
        }
    }
}
