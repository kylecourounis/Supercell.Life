namespace Supercell.Life.Server.Files.CsvLogic
{
    using Supercell.Life.Titan.Files.CsvReader;
    using Supercell.Life.Titan.Logic;

    using Supercell.Life.Server.Files.CsvHelpers;

    internal class LogicEnergyPackageData : LogicData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="LogicEnergyPackageData"/> class.
        /// </summary>
        public LogicEnergyPackageData(Row row, LogicDataTable dataTable) : base(row, dataTable)
        {
            LogicData.Load(this, this.GetType(), row);
        }

        public string TID
        {
            get; set;
        }

        public string InfoTID
        {
            get; set;
        }

        public LogicArrayList<int> IncreaseInMaxEnergy
        {
            get; set;
        }

        public LogicArrayList<int> Diamonds
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

        public bool HideWhenOnStartingEnergy
        {
            get; set;
        }
    }
}
