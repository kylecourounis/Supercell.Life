namespace Supercell.Life.Server.Logic.Avatar.Slots
{
    using Newtonsoft.Json;

    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Json;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvHelpers;

    internal class LogicDataSlot
    {
        [JsonProperty("id")]  internal int Id;
        [JsonProperty("cnt")] internal int Count;

        /// <summary>
        /// Gets an instance of <see cref="LogicData"/> using <see cref="LogicDataSlot.Id"/>.
        /// </summary>
        internal LogicData Data
        {
            get
            {
                return CSV.Tables.GetWithGlobalID(this.Id);
            }
        }

        /// <summary>
        /// Gets the checksum for this <see cref="LogicDataSlot"/>.
        /// </summary>
        internal int Checksum
        {
            get
            {
                return this.Id + this.Count;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicDataSlot"/> class.
        /// </summary>
        internal LogicDataSlot()
        {
            // LogicDataSlot.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicDataSlot"/> class.
        /// </summary>
        internal LogicDataSlot(int data, int count)
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

        /// <summary>
        /// Saves this <see cref="LogicDataSlot"/> to the returned <see cref="LogicJSONObject"/>.
        /// </summary>
        internal LogicJSONObject SaveToJSON()
        {
            LogicJSONObject json = new LogicJSONObject();

            json.Put("id", new LogicJSONNumber(this.Id));
            json.Put("cnt", new LogicJSONNumber(this.Count));

            return json;
        }

        /// <summary>
        /// Reads this <see cref="LogicDataSlot"/> from the specified <see cref="LogicJSONObject"/>.
        /// </summary>
        internal void ReadFromJSON(LogicJSONObject json)
        {
            this.Id    = json.GetJsonNumber("id").GetIntValue();
            this.Count = json.GetJsonNumber("cnt").GetIntValue();
        }
    }
}