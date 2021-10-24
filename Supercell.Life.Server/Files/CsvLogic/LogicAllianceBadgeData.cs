namespace Supercell.Life.Server.Files.CsvLogic
{ 
    using Supercell.Life.Titan.Files.CsvReader;
    
    using Supercell.Life.Server.Files.CsvHelpers;

    internal class LogicAllianceBadgeData : LogicData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicAllianceBadgeData"/> class.
        /// </summary>
        public LogicAllianceBadgeData(Row row, LogicDataTable dataTable) : base(row, dataTable)
        {
            this.CreateReferences(this, row);
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
