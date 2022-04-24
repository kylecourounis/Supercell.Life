namespace Supercell.Life.Server.Logic.Game.Objects.Quests.Items
{
    using Supercell.Life.Titan.Logic.Json;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Titan.Logic.Utils;

    internal class Enemy
    {
        internal int Hitpoints;
        internal int Damage;
        internal int FirstAttackOnTurn;
        internal int AttackTurnSeq;

        /// <summary>
        /// Gets the data.
        /// </summary>
        internal LogicEnemyCharacterData Data
        {
            get;
        }

        /// <summary>
        /// Gets the x.
        /// </summary>
        internal int X
        {
            get;
        }

        /// <summary>
        /// Gets the y.
        /// </summary>
        internal int Y
        {
            get;
        }

        /// <summary>
        /// Gets the team.
        /// </summary>
        internal int Team
        {
            get;
        }

        /// <summary>
        /// Gets the level.
        /// </summary>
        internal int Level
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Enemy"/> class.
        /// </summary>
        internal Enemy(int globalID, int x, int y)
        {
            this.Data  = (LogicEnemyCharacterData)CSV.Tables.Get(Gamefile.EnemyCharacters).GetDataWithID(globalID);
            this.X     = x;
            this.Y     = y;
            this.Team  = 1;
            this.Level = LogicStringUtil.ConvertToInt(this.Data.ExportName.Split("_")[2].Replace("lvl", string.Empty));

            this.Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Enemy"/> class.
        /// </summary>
        internal Enemy(LogicJSONObject json)
        {
            this.Data  = (LogicEnemyCharacterData)CSV.Tables.Get(Gamefile.EnemyCharacters).GetDataWithID(json.GetJsonNumber("data").GetIntValue());
            this.X     = json.GetJsonNumber("x").GetIntValue();
            this.Y     = json.GetJsonNumber("y").GetIntValue();
            this.Team  = json.GetJsonNumber("team").GetIntValue();
            this.Level = json.GetJsonNumber("lvl").GetIntValue();

            this.Initialize();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            this.Hitpoints         = this.Data.Hitpoints[this.Level];
            this.Damage            = this.Data.Damage[this.Level];
            this.FirstAttackOnTurn = this.Data.FirstAttackOnTurn;
            this.AttackTurnSeq     = this.Data.AttackTurnSeq;
        }
        
        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return $"Data: {this.Data.GlobalID}, X-Y: ({this.X},{this.Y}), Team: {this.Team}, Level: {this.Level}";
        }
    }
}