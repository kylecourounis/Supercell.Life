namespace Supercell.Life.Server.Logic.Game.Objects.Quests.Items
{
    using Newtonsoft.Json;

    internal class Enemy
    {
        [JsonProperty("data")]
        internal int Data
        {
            get;
            set;
        }

        [JsonProperty("x")]
        internal int X
        {
            get;
            set;
        }

        [JsonProperty("y")]
        internal int Y
        {
            get;
            set;
        }

        [JsonProperty("team")]
        internal int Team
        {
            get;
            set;
        }

        [JsonProperty("level")]
        internal int Level
        {
            get;
            set;
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return $"Data: {this.Data}, X-Y: ({this.X},{this.Y}), Team: {this.Team}, Level: {this.Level}";
        }
    }
}