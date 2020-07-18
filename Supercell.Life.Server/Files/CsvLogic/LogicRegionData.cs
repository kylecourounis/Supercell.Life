namespace Supercell.Life.Server.Files.CsvLogic
{
	using Supercell.Life.Titan.Files.CsvReader;

	using Supercell.Life.Server.Files.CsvHelpers;

    internal class LogicRegionData : LogicData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="LogicRegionData"/> class.
        /// </summary>
        public LogicRegionData(Row row, LogicDataTable dataTable) : base(row, dataTable)
        {
            LogicData.Load(this, this.GetType(), row);
        }

        public string TID
        {
            get; set;
        }

        public string DisplayName
        {
            get; set;
        }

        public bool IsCountry
        {
            get; set;
        }
    }
}
