namespace Supercell.Life.Server.Files.CsvLogic
{
    using Supercell.Life.Titan.Files.CsvReader;

    using Supercell.Life.Server.Files.CsvHelpers;

    internal class LogicWorldData : LogicData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicWorldData"/> class.
        /// </summary>
        public LogicWorldData(Row row, LogicDataTable dataTable) : base(row, dataTable)
        {
            LogicData.Load(this, this.GetType(), row);
        }

        public string TID
        {
            get; set;
        }

        public string InfoUIExportName
        {
            get; set;
        }

        public string LevelBackgroundSWF
        {
            get; set;
        }

        public string LevelBackgroundExportNames
        {
            get; set;
        }

        public string LevelEndBackgroundExportNames
        {
            get; set;
        }

        public string LevelForegroundExportNames
        {
            get; set;
        }

        public string LevelCloudSWF
        {
            get; set;
        }

        public string LevelCloudExportNames
        {
            get; set;
        }

        public int ShadowColorMulR
        {
            get; set;
        }

        public int ShadowColorMulG
        {
            get; set;
        }

        public int ShadowColorMulB
        {
            get; set;
        }

        public int ShadowAlpha
        {
            get; set;
        }

        public string ProfileBackgroundExportName
        {
            get; set;
        }

        public string BarrelEnemies
        {
            get; set;
        }

        public int BarrelEnemyChance
        {
            get; set;
        }
    }
}
