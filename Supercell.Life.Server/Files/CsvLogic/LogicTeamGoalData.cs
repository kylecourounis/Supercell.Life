namespace Supercell.Life.Server.Files.CsvLogic
{
    using Supercell.Life.Titan.Files.CsvReader;

    using Supercell.Life.Server.Files.CsvHelpers;

    internal class LogicTeamGoalData : LogicData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicTeamGoalData"/> class.
        /// </summary>
        public LogicTeamGoalData(Row row, LogicDataTable dataTable) : base(row, dataTable)
        {
            this.CreateReferences(this, row);
        }

        public int Type
        {
            get; set;
        }

        public int Level
        {
            get; set;
        }

        public int SimilarityValue
        {
            get; set;
        }

        public bool CanBeInNewTeam
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

        public int DiamondReward
        {
            get; set;
        }

        public string CompletedTID
        {
            get; set;
        }

        public string Hero
        {
            get; set;
        }

        public string NPC
        {
            get; set;
        }

        public string Obstacle
        {
            get; set;
        }

        public int SortIndex
        {
            get; set;
        }

        public bool Enabled
        {
            get; set;
        }
    }
}
