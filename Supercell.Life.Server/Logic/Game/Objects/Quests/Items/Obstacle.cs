﻿namespace Supercell.Life.Server.Logic.Game.Objects.Quests.Items
{
    using Supercell.Life.Titan.Logic.Json;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic.Enums;

    internal class Obstacle
    {
        internal int Hits;

        /// <summary>
        /// Gets the data.
        /// </summary>
        internal LogicObstacleData Data
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
        /// Initializes a new instance of the <see cref="Obstacle"/> class.
        /// </summary>
        internal Obstacle(LogicJSONObject json)
        {
            this.Data = (LogicObstacleData)CSV.Tables.Get(Gamefile.Obstacles).GetDataWithID(json.GetJsonNumber("data").GetIntValue());
            this.X    = json.GetJsonNumber("x").GetIntValue();
            this.Y    = json.GetJsonNumber("y").GetIntValue();
            this.Hits = 0;
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return $"Data: {this.Data.GlobalID}, X-Y: ({this.X},{this.Y})";
        }
    }
}