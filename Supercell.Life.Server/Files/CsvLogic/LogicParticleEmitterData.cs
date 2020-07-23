namespace Supercell.Life.Server.Files.CsvLogic
{
    using Supercell.Life.Titan.Files.CsvReader;

    using Supercell.Life.Server.Files.CsvHelpers;

    internal class LogicParticleEmitterData : LogicData
    {
		/// <summary>
        /// Initializes a new instance of the <see cref="LogicParticleEmitterData"/> class.
        /// </summary>
        public LogicParticleEmitterData(Row row, LogicDataTable dataTable) : base(row, dataTable)
        {
            LogicData.Load(this, this.GetType(), row);
        }

        public string ParticleSwf
        {
            get; set;
        }

        public string ParticleExportName
        {
            get; set;
        }

        public int ParticleCount
        {
            get; set;
        }

        public int EmissionTime
        {
            get; set;
        }

        public int MinLife
        {
            get; set;
        }

        public int MaxLife
        {
            get; set;
        }

        public int StartScale
        {
            get; set;
        }

        public int EndScale
        {
            get; set;
        }

        public bool RandomStartScale
        {
            get; set;
        }

        public int MinHorizAngle
        {
            get; set;
        }

        public int MaxHorizAngle
        {
            get; set;
        }

        public int MinVertAngle
        {
            get; set;
        }

        public int MaxVertAngle
        {
            get; set;
        }

        public int MinSpeed
        {
            get; set;
        }

        public int MaxSpeed
        {
            get; set;
        }

        public int StartZ
        {
            get; set;
        }

        public int Gravity
        {
            get; set;
        }

        public int Inertia
        {
            get; set;
        }

        public int MinRotate
        {
            get; set;
        }

        public int MaxRotate
        {
            get; set;
        }

        public bool DisableRotation
        {
            get; set;
        }

        public int FadeOutTime
        {
            get; set;
        }

        public int StartRadius
        {
            get; set;
        }

        public int StartRadiusMax
        {
            get; set;
        }

        public bool ScaleTimeline
        {
            get; set;
        }

        public bool OrientToMovement
        {
            get; set;
        }

        public bool OrientToParent
        {
            get; set;
        }

        public bool OrientToImpact
        {
            get; set;
        }

        public bool BounceFromGround
        {
            get; set;
        }

        public bool StopAnimationAfterBounce
        {
            get; set;
        }

        public bool AdditiveBlend
        {
            get; set;
        }

        public bool UseMovingEmitter
        {
            get; set;
        }

        public int EmitterMinHorizAngle
        {
            get; set;
        }

        public int EmitterMaxHorizAngle
        {
            get; set;
        }

        public int EmitterMinVertAngle
        {
            get; set;
        }

        public int EmitterMaxVertAngle
        {
            get; set;
        }

        public int EmitterMinSpeed
        {
            get; set;
        }

        public int EmitterMaxSpeed
        {
            get; set;
        }

        public int EmitterGravity
        {
            get; set;
        }

        public int EmitterInertia
        {
            get; set;
        }

        public string EmitterIndicatorSwf
        {
            get; set;
        }

        public string EmitterIndicatorExportName
        {
            get; set;
        }
    }
}
