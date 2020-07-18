namespace Supercell.Life.Server.Files.CsvLogic
{
	using Supercell.Life.Titan.Files.CsvReader;

	using Supercell.Life.Server.Files.CsvHelpers;

    internal class LogicDialogData : LogicData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="LogicDialogData"/> class.
        /// </summary>
        public LogicDialogData(Row row, LogicDataTable dataTable) : base(row, dataTable)
        {
            LogicData.Load(this, this.GetType(), row);
        }

        public string Quest
        {
            get; set;
        }

        public string DialogType
        {
            get; set;
        }

        public string DialogTID
        {
            get; set;
        }

        public string Sound
        {
            get; set;
        }

        public bool OwnText
        {
            get; set;
        }

        public string Boss
        {
            get; set;
        }

        public string TakeDmgTaunt
        {
            get; set;
        }

        public string DealDmgTaunt
        {
            get; set;
        }

        public string HitObstacleTaunt
        {
            get; set;
        }

        public string DieTaunt
        {
            get; set;
        }

        public string KillTaunt
        {
            get; set;
        }

        public string SpecialTaunt
        {
            get; set;
        }
    }
}
