namespace Supercell.Life.Server.Files.CsvLogic
{
    using Supercell.Life.Titan.Files.CsvReader;

    using Supercell.Life.Server.Files.CsvHelpers;

    internal class LogicSpellData : LogicData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicSpellData"/> class.
        /// </summary>
        public LogicSpellData(Row row, LogicDataTable dataTable) : base(row, dataTable)
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

        public int UnlockCost
        {
            get; set;
        }

        public int CreateCost
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

        public int ActionValue
        {
            get; set;
        }

        public int TriggerRadius
        {
            get; set;
        }

        public string PickUpEffect
        {
            get; set;
        }

        public string UseEffect
        {
            get; set;
        }

        public string BouncingStopsEffect
        {
            get; set;
        }

        public string SWF
        {
            get; set;
        }

        public string ExportName
        {
            get; set;
        }

        public string ExportNameFly
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

        public string IconExportCard
        {
            get; set;
        }
    }
}