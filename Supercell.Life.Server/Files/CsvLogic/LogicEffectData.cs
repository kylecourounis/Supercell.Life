namespace Supercell.Life.Server.Files.CsvLogic
{
    using Supercell.Life.Titan.Files.CsvReader;

    using Supercell.Life.Server.Files.CsvHelpers;

    internal class LogicEffectData : LogicData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicEffectData"/> class.
        /// </summary>
        public LogicEffectData(Row row, LogicDataTable dataTable) : base(row, dataTable)
        {
            this.CreateReferences(this, row);
        }

        public string SWF
        {
            get; set;
        }

        public string ExportName
        {
            get; set;
        }

        public string ParticleEmitter
        {
            get; set;
        }

        public int EmitterProbability
        {
            get; set;
        }

        public int EmitterDelayMs
        {
            get; set;
        }

        public int EmitterOffsetX
        {
            get; set;
        }

        public int EmitterOffsetY
        {
            get; set;
        }

        public int CameraShake
        {
            get; set;
        }

        public bool AttachToParent
        {
            get; set;
        }

        public bool Looping
        {
            get; set;
        }

        public string IsoLayer
        {
            get; set;
        }

        public bool Targeted
        {
            get; set;
        }

        public int MaxCount
        {
            get; set;
        }

        public bool RemoveWhenSourceDies
        {
            get; set;
        }

        public string Sound
        {
            get; set;
        }

        public bool EnableOnlyWhenInView
        {
            get; set;
        }

        public bool DisableFromLowGFX
        {
            get; set;
        }
    }
}
