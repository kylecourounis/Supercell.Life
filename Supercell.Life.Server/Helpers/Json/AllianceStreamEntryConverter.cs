namespace Supercell.Life.Server.Helpers.Json
{
    using System;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using Supercell.Life.Server.Database.Models;
    using Supercell.Life.Server.Logic.Alliance;
    using Supercell.Life.Server.Logic.Alliance.Entries;

    internal class AllianceStreamEntryConverter : JsonConverter
    {
        /// <summary>
        /// Determines whether this instance can convert the specified type.
        /// </summary>
        public override bool CanConvert(Type type)
        {
            return type == typeof(AllianceStreamEntry);
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="JsonConverter" /> can read JSON.
        /// </summary>
        public override bool CanRead
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="JsonConverter" /> can write JSON.
        /// </summary>
        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Reads the json.
        /// </summary>
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject json = JObject.Load(reader);

            try
            {
                var type = (AllianceStreamEntry.AllianceStreamType)((int)json["StreamType"]);

                switch (type)
                {
                    case AllianceStreamEntry.AllianceStreamType.Chat:
                    {
                        return JsonConvert.DeserializeObject<AllianceChatStreamEntry>(json.ToString(), AllianceDb.JsonSettings);
                    }
                    case AllianceStreamEntry.AllianceStreamType.Rejected:
                    {
                        return JsonConvert.DeserializeObject<AllianceRejectedStreamEntry>(json.ToString(), AllianceDb.JsonSettings);
                    }
                    case AllianceStreamEntry.AllianceStreamType.Event:
                    {
                        return JsonConvert.DeserializeObject<AllianceEventStreamEntry>(json.ToString(), AllianceDb.JsonSettings);
                    }
                    default:
                    {
                        return JsonConvert.DeserializeObject<AllianceStreamEntry>(json.ToString(), AllianceDb.JsonSettings);
                    }
                }
            }
            catch
            {
                return existingValue;
            }
        }

        /// <summary>
        /// Writes the json.
        /// </summary>
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value != null)
            {
                serializer.Serialize(writer, value);
            }
        }
    }
}