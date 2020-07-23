namespace Supercell.Life.Server.Files.CsvLogic
{
    using Supercell.Life.Titan.Files.CsvReader;

    using Supercell.Life.Server.Files.CsvHelpers;

    internal class LogicBossData : LogicData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicBossData"/> class.
        /// </summary>
        public LogicBossData(Row row, LogicDataTable dataTable) : base(row, dataTable)
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

        public string SWF
        {
            get; set;
        }

        public string ExportName
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

        public int BountyMin
        {
            get; set;
        }

        public int BountyMax
        {
            get; set;
        }

        public int Scale
        {
            get; set;
        }

        public int Hitpoints
        {
            get; set;
        }

        public int Radius
        {
            get; set;
        }

        public bool NoCollisionWhenDies
        {
            get; set;
        }

        public int FirstAttackOnTurn
        {
            get; set;
        }

        public int AttackTurnSeq
        {
            get; set;
        }

        public string TakeDamageEffect
        {
            get; set;
        }

        public string TakeDamageEffect2
        {
            get; set;
        }

        public string DieEffect
        {
            get; set;
        }

        public int ShadowHeight
        {
            get; set;
        }

        public int ColorMulR
        {
            get; set;
        }

        public int ColorMulG
        {
            get; set;
        }

        public int ColorMulB
        {
            get; set;
        }

        public int ColorAddR
        {
            get; set;
        }

        public int ColorAddG
        {
            get; set;
        }

        public int ColorAddB
        {
            get; set;
        }

        public string SpawnEnemyWhenDie1
        {
            get; set;
        }

        public string SpawnEnemyWhenDie2
        {
            get; set;
        }

        public int VelocityModifierOnCollision
        {
            get; set;
        }

        public string AttacksChosenInSequence
        {
            get; set;
        }

        public int AttackDelay
        {
            get; set;
        }

        public int AttackOddsA
        {
            get; set;
        }

        public string AttackEffect
        {
            get; set;
        }

        public string AttackEffect2
        {
            get; set;
        }

        public string AttackTypeA
        {
            get; set;
        }

        public string ProjectileA
        {
            get; set;
        }

        public string SpawnedCharacterWhenAttackA
        {
            get; set;
        }

        public int AttackRadiusA
        {
            get; set;
        }

        public int Damage
        {
            get; set;
        }

        public int AttackOddsB
        {
            get; set;
        }

        public string AttackEffectB
        {
            get; set;
        }

        public string AttackEffect2B
        {
            get; set;
        }

        public string AttackTypeB
        {
            get; set;
        }

        public string ProjectileB
        {
            get; set;
        }

        public string SpawnedCharacterWhenAttackB
        {
            get; set;
        }

        public int AttackRadiusB
        {
            get; set;
        }

        public int DamageB
        {
            get; set;
        }

        public string SpawnedObstacle
        {
            get; set;
        }
    }
}
