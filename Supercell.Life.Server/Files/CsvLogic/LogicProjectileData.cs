namespace Supercell.Life.Server.Files.CsvLogic
{
    using Supercell.Life.Titan.Files.CsvReader;

    using Supercell.Life.Server.Files.CsvHelpers;

    internal class LogicProjectileData : LogicData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicProjectileData"/> class.
        /// </summary>
        public LogicProjectileData(Row row, LogicDataTable dataTable) : base(row, dataTable)
        {
            LogicData.Load(this, this.GetType(), row);
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

        public int Scale
        {
            get; set;
        }

        public int Speed
        {
            get; set;
        }

        public int StartHeight
        {
            get; set;
        }

        public int StartOffset
        {
            get; set;
        }

        public int PushBackSpeed
        {
            get; set;
        }

        public int HitRadius
        {
            get; set;
        }

        public bool IsBallistic
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

        public bool ShadowScaling
        {
            get; set;
        }

        public bool ShadowAlpha
        {
            get; set;
        }

        public bool UseTopLayer
        {
            get; set;
        }

        public bool BounceFromWalls
        {
            get; set;
        }

        public int BounceCount
        {
            get; set;
        }

        public int SlowdownToSpeedRatio
        {
            get; set;
        }

        public int SlowdownModifier
        {
            get; set;
        }

        public string HitTargetEffect
        {
            get; set;
        }

        public string DieEffect
        {
            get; set;
        }

        public int AreaDamageRadius
        {
            get; set;
        }

        public int AreaDamagePushback
        {
            get; set;
        }

        public bool DisableCollisionDamage
        {
            get; set;
        }

        public string SpawnObstacle
        {
            get; set;
        }

        public bool Ballistic
        {
            get; set;
        }

        public bool DoNotDestroyObstaclesOnArea
        {
            get; set;
        }

        public string SpawnProjectile
        {
            get; set;
        }

        public int SpawnProjectileCount
        {
            get; set;
        }
    }
}
