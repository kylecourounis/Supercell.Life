namespace Supercell.Life.Server.Files.CsvLogic
{
    using Supercell.Life.Titan.Files.CsvReader;

    using Supercell.Life.Server.Files.CsvHelpers;

    internal class LogicCollectableData : LogicData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicCollectableData"/> class.
        /// </summary>
        public LogicCollectableData(Row row, LogicDataTable dataTable) : base(row, dataTable)
        {
            LogicData.Load(this, this.GetType(), row);
        }

        public string TID
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

        public string IconSWF
        {
            get; set;
        }

        public string IconExportName
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

        public string AppearEffect
        {
            get; set;
        }

        public string BouncingStopsEffect
        {
            get; set;
        }

        public int ColorMulR
        {
            get; set;
        }

        public int ColorMulG
        {
            get; set;
        }

        public int ColorMulB
        {
            get; set;
        }

        public int ColorAddR
        {
            get; set;
        }

        public int ColorAddG
        {
            get; set;
        }

        public int ColorAddB
        {
            get; set;
        }

        public string RewardResource
        {
            get; set;
        }

        public int RewardAmount
        {
            get; set;
        }

        public int DisappearTurns
        {
            get; set;
        }
    }
}
