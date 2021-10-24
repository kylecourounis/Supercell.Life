namespace Supercell.Life.Server.Files.CsvLogic
{
    using Supercell.Life.Titan.Files.CsvReader;

    using Supercell.Life.Server.Files.CsvHelpers;

    internal class LogicDecoData : LogicData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicDecoData"/> class.
        /// </summary>
        public LogicDecoData(Row row, LogicDataTable dataTable) : base(row, dataTable)
        {
            this.CreateReferences(this, row);
        }

        public string TID
        {
            get; set;
        }

        public string DescriptionTID
        {
            get; set;
        }

        public int Cost
        {
            get; set;
        }

        public string Resource
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

        public string ObjectSWF
        {
            get; set;
        }

        public string ObjectExportName
        {
            get; set;
        }

        public string ShadowSWF
        {
            get; set;
        }

        public string ShadowExportName
        {
            get; set;
        }

        public string IconBGSWF
        {
            get; set;
        }

        public string IconBGExportName
        {
            get; set;
        }

        public string VisualType
        {
            get; set;
        }

        public int UnlockLevel
        {
            get; set;
        }

        public string CharacterUnlock
        {
            get; set;
        }

        public int CostumeIndex
        {
            get; set;
        }

        public bool Hidden
        {
            get; set;
        }

        public int SortIndex
        {
            get; set;
        }
    }
}
