namespace Supercell.Life.Server.Files.CsvLogic
{
	using Supercell.Life.Titan.Files.CsvReader;

	using Supercell.Life.Server.Files.CsvHelpers;

    internal class LogicExperienceLevelData : LogicData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="LogicExperienceLevelData"/> class.
        /// </summary>
        public LogicExperienceLevelData(Row row, LogicDataTable dataTable) : base(row, dataTable)
        {
            LogicData.Load(this, this.GetType(), row);
        }

        public int ExpPoints
        {
            get; set;
        }

        public int PvpBonusXp
        {
            get; set;
        }

        public int MapChestMinGold
        {
            get; set;
        }

        public int MapChestMaxGold
        {
            get; set;
        }

        public int MapChestMinXP
        {
            get; set;
        }

        public int MapChestMaxXP
        {
            get; set;
        }

        public int DefaultQuestRewardGoldPerEnergy
        {
            get; set;
        }

        public int DefaultQuestRewardXpPerEnergy
        {
            get; set;
        }
    }
}
