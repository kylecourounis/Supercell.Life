namespace Supercell.Life.Server.Files.CsvLogic
{ 
    using Supercell.Life.Titan.Files.CsvReader;

    using Supercell.Life.Server.Files.CsvHelpers;

    internal class LogicAchievementData : LogicData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicAchievementData"/> class.
        /// </summary>
        public LogicAchievementData(Row row, LogicDataTable dataTable) : base(row, dataTable)
        {
            this.CreateReferences(this, row);
        }

        public int Level
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

        public string Action
        {
            get; set;
        }

        public int ActionCount
        {
            get; set;
        }

        public int ExpReward
        {
            get; set;
        }

        public int DiamondReward
        {
            get; set;
        }

        public string CompletedTID
        {
            get; set;
        }

        public bool ShowOnlyInBattle
        {
            get; set;
        }

        public string Hero
        {
            get; set;
        }

        public bool ShowOnlyInHomescreen
        {
            get; set;
        }

        public int SortIndex
        {
            get; set;
        }

        public bool Hidden
        {
            get; set;
        }
    }
}
