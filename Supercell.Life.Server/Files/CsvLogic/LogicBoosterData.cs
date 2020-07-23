namespace Supercell.Life.Server.Files.CsvLogic
{
    using Supercell.Life.Titan.Files.CsvReader;

    using Supercell.Life.Server.Files.CsvHelpers;

    internal class LogicBoosterData : LogicData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="LogicBoosterData"/> class.
        /// </summary>
        public LogicBoosterData(Row row, LogicDataTable dataTable) : base(row, dataTable)
        {
            LogicData.Load(this, this.GetType(), row);
        }

        public string TID
        {
            get; set;
        }

        public string InfoTID
        {
            get; set;
        }

        public int TimeDays
        {
            get; set;
        }

        public int BoostPercentage
        {
            get; set;
        }

        public int Diamonds
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

        internal double Boost
        {
            get
            {
                switch (this.BoostPercentage)
                {
                    case 100:
                    {
                        return 2;
                    }
                    case 150:
                    {
                        return 2.5;
                    }
                    case 200:
                    {
                        return 3;
                    }
                    default:
                    {
                        return 1;
                    }
                }
            }
        }
    }
}
