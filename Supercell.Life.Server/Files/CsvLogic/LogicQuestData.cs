namespace Supercell.Life.Server.Files.CsvLogic
{
    using Supercell.Life.Titan.Files.CsvReader;

    using Supercell.Life.Server.Files.CsvHelpers;

    internal class LogicQuestData : LogicData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="LogicQuestData"/> class.
        /// </summary>
        public LogicQuestData(Row row, LogicDataTable dataTable) : base(row, dataTable)
        {
            LogicData.Load(this, this.GetType(), row);
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

        public string IconUiExportName
        {
            get; set;
        }

        public string World
        {
            get; set;
        }

        public int RequiredXpLevel
        {
            get; set;
        }

        public string RequiredQuest
        {
            get; set;
        }

        public string QuestType
        {
            get; set;
        }

        public string LevelFile
        {
            get; set;
        }

        public int GoalMoveCount
        {
            get; set;
        }

        public int MinEnemies
        {
            get; set;
        }

        public int MinObstacles
        {
            get; set;
        }

        public int Energy
        {
            get; set;
        }

        public int GoldRewardOverride
        {
            get; set;
        }

        public int XpRewardOverride
        {
            get; set;
        }

        public string AIBehaviour
        {
            get; set;
        }

        public int AIInaccuracy
        {
            get; set;
        }

        public bool Hidden
        {
            get; set;
        }

        public int MinGoldDrop
        {
            get; set;
        }

        public int MaxGoldDrop
        {
            get; set;
        }

        public string League
        {
            get; set;
        }

        public int SortOrder
        {
            get; set;
        }
    }
}
