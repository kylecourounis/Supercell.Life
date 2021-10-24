namespace Supercell.Life.Server.Files.CsvLogic
{
    using Supercell.Life.Titan.Files.CsvReader;

    using Supercell.Life.Server.Files.CsvHelpers;

    internal class LogicResourceData : LogicData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicResourceData"/> class.
        /// </summary>
        public LogicResourceData(Row row, LogicDataTable dataTable) : base(row, dataTable)
        {
            this.CreateReferences(this, row);
        }

        public string TID
        {
            get; set;
        }

        public string SWF
        {
            get; set;
        }

        public string CollectEffect
        {
            get; set;
        }

        public string ResourceIconExportName
        {
            get; set;
        }

        public string ResourceIconButtonExportName
        {
            get; set;
        }

        public string HudInstanceName
        {
            get; set;
        }

        public int TextRed
        {
            get; set;
        }

        public int TextGreen
        {
            get; set;
        }

        public int TextBlue
        {
            get; set;
        }
    }
}
