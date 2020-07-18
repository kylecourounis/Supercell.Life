namespace Supercell.Life.Server.Files.CsvLogic
{
    using Supercell.Life.Titan.Files.CsvReader;
    using Supercell.Life.Titan.Logic;

    using Supercell.Life.Server.Files.CsvHelpers;

    internal class LogicItemsData : LogicData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicItemsData"/> class.
        /// </summary>
        public LogicItemsData(Row row, LogicDataTable dataTable) : base(row, dataTable)
        {
            LogicData.Load(this, this.GetType(), row);
        }

        public string Name
        {
            get; set;
        }

        public string TID
        {
            get; set;
        }

        public string InfoTID
        {
            get; set;
        }

        public LogicArrayList<string> Cost
        {
            get; set;
        }

        public int CreateTime
        {
            get; set;
        }

        public string Action
        {
            get; set;
        }

        public int RequiredXp
        {
            get; set;
        }

        public int ActionCount
        {
            get; set;
        }

        public int UnavailableAfterUnequip
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

        public string UpgradeTID
        {
            get; set;
        }
    }
}
