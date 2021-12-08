namespace Supercell.Life.Server.Helpers.Json
{
    using System;

    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using Supercell.Life.Server.Database.Models;
    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Logic.Avatar.Entries;

    internal class AvatarStreamEntryConverter : JsonConverter
    {
        /// <summary>
        /// Determines whether this instance can convert the specified type.
        /// </summary>
        public override bool CanConvert(Type type)
        {
            return type == typeof(AvatarStreamEntry);
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
                var type = (AvatarStreamEntry.AvatarStreamType)((int)json["StreamType"]);

                switch (type)
                {
                    case AvatarStreamEntry.AvatarStreamType.AllianceInvite:
                    {
                        return JsonConvert.DeserializeObject<AllianceInvitationAvatarStreamEntry>(json.ToString(), AvatarDb.JsonSettings);
                    }
                    case AvatarStreamEntry.AvatarStreamType.AllianceKick:
                    {
                        return JsonConvert.DeserializeObject<AllianceKickOutStreamEntry>(json.ToString(), AvatarDb.JsonSettings);
                    }
                    case AvatarStreamEntry.AvatarStreamType.AllianceMail:
                    {
                        return JsonConvert.DeserializeObject<AllianceMailAvatarStreamEntry>(json.ToString(), AvatarDb.JsonSettings);
                    }
                    default:
                    {
                        return JsonConvert.DeserializeObject<AvatarStreamEntry>(json.ToString(), AvatarDb.JsonSettings);
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