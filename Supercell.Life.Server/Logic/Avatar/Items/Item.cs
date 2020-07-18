namespace Supercell.Life.Server.Logic.Avatar.Items
{
    using Newtonsoft.Json;

    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvHelpers;

    internal class Item
    {
        [JsonProperty("id")]  internal int Id;
        [JsonProperty("cnt")] internal int Count;

        /// <summary>
        /// Gets an instance of <see cref="LogicData"/> using <see cref="Item.Id"/>.
        /// </summary>
        internal LogicData Data
        {
            get
            {
                return CSV.Tables.GetWithGlobalID(this.Id);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Item"/> class.
        /// </summary>
        internal Item()
        {
            // Item.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Item"/> class.
        /// </summary>
        internal Item(int data, int count)
        {
            this.Id    = data;
            this.Count = count;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal void Decode(ByteStream stream)
        {
            this.Id    = stream.ReadInt();
            this.Count = stream.ReadInt();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal void Encode(ByteStream stream)
        {
            stream.WriteInt(this.Id);
            stream.WriteInt(this.Count);
        }
    }
}