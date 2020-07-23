namespace Supercell.Life.Server.Files.CsvLogic
{
    using Supercell.Life.Titan.Files.CsvReader;

    using Supercell.Life.Server.Files.CsvHelpers;

    internal class LogicBillingPackageData : LogicData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="LogicBillingPackageData"/> class.
        /// </summary>
        public LogicBillingPackageData(Row row, LogicDataTable dataTable) : base(row, dataTable)
        {
            LogicData.Load(this, this.GetType(), row);
        }

        public string TID
        {
            get; set;
        }

        public bool Disabled
        {
            get; set;
        }

        public bool ExistsApple
        {
            get; set;
        }

        public bool ExistsAndroid
        {
            get; set;
        }

        public int Diamonds
        {
            get; set;
        }

        public int USD
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

        public int Order
        {
            get; set;
        }
    }
}
