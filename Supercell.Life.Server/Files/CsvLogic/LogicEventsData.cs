namespace Supercell.Life.Server.Files.CsvLogic
{
    using Supercell.Life.Server.Files.CsvHelpers;
    using Supercell.Life.Titan.Files.CsvReader;

    internal class LogicEventsData : LogicData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicEnergyPackageData"/> class.
        /// </summary>
        public LogicEventsData(Row row, LogicDataTable dataTable) : base(row, dataTable)
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

        public string NameTID
        {
            get; set;
        }

        public string InfoTID
        {
            get; set;
        }

        public string NotificationTID
        {
            get; set;
        }

        public string LaunchDate
        {
            get; set;
        }

        public string EndDate
        {
            get; set;
        }

        public bool Enabled
        {
            get; set;
        }

        public int IntervalDays
        {
            get; set;
        }

        public string Type
        {
            get; set;
        }

        public int DurationSeconds
        {
            get; set;
        }

        public int OffsetAfterLaunch
        {
            get; set;
        }

        public string ChestOrbReward
        {
            get; set;
        }

        public int ExtraChestGoldReward
        {
            get; set;
        }

        public int ExtraChestXPReward
        {
            get; set;
        }

        public int ExtraGoldReward
        {
            get; set;
        }

        public int ExtraXPReward
        {
            get; set;
        }

        public string EventQuestData
        {
            get; set;
        }

        public int EventQuestLeagueMin
        {
            get; set;
        }

        public int OrbRewardChances
        {
            get; set;
        }

        public string StartSound
        {
            get; set;
        }

        public string ItemSWF
        {
            get; set;
        }

        public string ItemExportName
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

        public string StartEffect
        {
            get; set;
        }
    }
}
