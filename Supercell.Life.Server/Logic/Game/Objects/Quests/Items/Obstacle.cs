namespace Supercell.Life.Server.Logic.Game.Objects.Quests.Items
{
    using Supercell.Life.Titan.Logic.Json;

    internal class Obstacle
    {
        /// <summary>
        /// Gets the data.
        /// </summary>
        internal int Data
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
            this.Data = json.GetJsonNumber("data").GetIntValue();
            this.X    = json.GetJsonNumber("x").GetIntValue();
            this.Y    = json.GetJsonNumber("y").GetIntValue();
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return $"Data: {this.Data}, X-Y: ({this.X},{this.Y})";
        }
    }
}