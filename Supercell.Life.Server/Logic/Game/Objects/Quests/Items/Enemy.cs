namespace Supercell.Life.Server.Logic.Game.Objects.Quests.Items
{
    using Supercell.Life.Titan.Logic.Json;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic.Enums;

    internal class Enemy
    {
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
        internal Enemy(LogicJSONObject json)
        {
            this.Data  = (LogicEnemyCharacterData)CSV.Tables.Get(Gamefile.EnemyCharacters).GetDataWithID(json.GetJsonNumber("data").GetIntValue());
            this.X     = json.GetJsonNumber("x").GetIntValue();
            this.Y     = json.GetJsonNumber("y").GetIntValue();
            this.Team  = json.GetJsonNumber("team").GetIntValue();
            this.Level = json.GetJsonNumber("lvl").GetIntValue();
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