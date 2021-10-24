namespace Supercell.Life.Server.Files.CsvLogic
{
    using Supercell.Life.Titan.Files.CsvReader;

    using Supercell.Life.Server.Files.CsvHelpers;

    internal class LogicLeagueData : LogicData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicLeagueData"/> class.
        /// </summary>
        public LogicLeagueData(Row row, LogicDataTable dataTable) : base(row, dataTable)
        {
            this.CreateReferences(this, row);
        }

        public string TID
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

        public string IconExportNameSmall
        {
            get; set;
        }

        public string MultiplayerIconSWF
        {
            get; set;
        }

        public string MultiplayerIconExportName
        {
            get; set;
        }

        public string IconText
        {
            get; set;
        }

        public int DemoteLimit
        {
            get; set;
        }

        public int PromoteLimit
        {
            get; set;
        }

        public int PlacementLimitLow
        {
            get; set;
        }

        public int PlacementLimitHigh
        {
            get; set;
        }

        public bool DemoteEnabled
        {
            get; set;
        }

        public bool PromoteEnabled
        {
            get; set;
        }

        public int PVPGoldReward
        {
            get; set;
        }

        public int PVPXpReward
        {
            get; set;
        }

        public string QuestItem
        {
            get; set;
        }
    }
}
