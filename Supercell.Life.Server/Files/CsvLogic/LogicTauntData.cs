namespace Supercell.Life.Server.Files.CsvLogic
{
    using Supercell.Life.Titan.Files.CsvReader;

    using Supercell.Life.Server.Files.CsvHelpers;

    internal class LogicTauntData : LogicData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicTauntData"/> class.
        /// </summary>
        public LogicTauntData(Row row, LogicDataTable dataTable) : base(row, dataTable)
        {
            this.CreateReferences(this, row);
        }

        public int Cost
        {
            get; set;
        }

        public string TauntText
        {
            get; set;
        }

        public string CharacterUnlock
        {
            get; set;
        }

        public string Resource
        {
            get; set;
        }

        public int UnlockLevel
        {
            get; set;
        }

        public bool UnlockedInBeginning
        {
            get; set;
        }

        public string ParamType
        {
            get; set;
        }
    }
}
