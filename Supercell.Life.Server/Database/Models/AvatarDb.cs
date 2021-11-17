namespace Supercell.Life.Server.Database.Models
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using MongoDB.Driver;

    using Newtonsoft.Json;

    using Supercell.Life.Server.Helpers.Json;
    using Supercell.Life.Server.Logic.Avatar;

    internal class AvatarDb
    {
        /// <summary>
        /// The settings for the <see cref="JsonConvert"/> class.
        /// </summary>
        [BsonIgnore]
        internal static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling            = TypeNameHandling.Auto,            MissingMemberHandling   = MissingMemberHandling.Ignore,
            DefaultValueHandling        = DefaultValueHandling.Include,     NullValueHandling       = NullValueHandling.Ignore,
            ReferenceLoopHandling       = ReferenceLoopHandling.Ignore,     Converters              = new List<JsonConverter> { new TimerConverter() },
            Formatting                  = Formatting.Indented
        };

        [BsonId] internal BsonObjectId Id;

        [BsonElement("HighID")] internal int HighID;
        [BsonElement("LowID")] internal int LowID;

        [BsonElement("Profile")] internal BsonDocument Profile;

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarDb"/> class.
        /// </summary>
        internal AvatarDb(int highId, int lowId, string json)
        {
            this.HighID  = highId;
            this.LowID   = lowId;
            this.Profile = BsonDocument.Parse(json.Replace("_TypeName", "$type"));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarDb"/> class.
        /// </summary>
        internal AvatarDb(LogicClientAvatar avatar) : this(avatar.HighID, avatar.LowID, JsonConvert.SerializeObject(avatar, AvatarDb.JsonSettings).Replace("$type", "_TypeName"))
        {
            // AvatarDb.
        }

        /// <summary>
        /// Creates the specified avatar.
        /// </summary>
        internal static async Task Create(LogicClientAvatar avatar)
        {
            await Mongo.Avatars.InsertOneAsync(new AvatarDb(avatar));
        }

        /// <summary>
        /// Creates the avatar in the database.
        /// </summary>
        internal static async Task<AvatarDb> Save(LogicClientAvatar avatar)
        {
            var updatedEntity = await Mongo.Avatars.FindOneAndUpdateAsync(avatarDb =>
                    
                avatarDb.HighID == avatar.HighID &&
                avatarDb.LowID == avatar.LowID,

                Builders<AvatarDb>.Update.Set(avatarDb => avatarDb.Profile, BsonDocument.Parse(JsonConvert.SerializeObject(avatar, AvatarDb.JsonSettings).Replace("$type", "_TypeName")))
            );

            if (updatedEntity != null)
            {
                if (updatedEntity.HighID == avatar.Identifier.High && updatedEntity.LowID == avatar.Identifier.Low)
                {
                    return updatedEntity;
                }
            }

            return null;
        }

        /// <summary>
        /// Loads this instance from the database.
        /// </summary>
        internal static async Task<AvatarDb> Load(int highId, int lowId)
        {
            if (lowId > 0)
            {
                var entities = await Mongo.Avatars.FindAsync(avatar => avatar.HighID == highId && avatar.LowID == lowId);

                if (entities != null)
                {
                    var entity = entities.FirstOrDefault();

                    if (entity != null)
                    {
                        return entity;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// Deletes this instance from the database.
        /// </summary>
        internal static async Task<bool> Delete(int highId, int lowId)
        {
            if (lowId > 0)
            {
                var result = await Mongo.Avatars.DeleteOneAsync(avatarDb => avatarDb.HighID == highId && avatarDb.LowID == lowId);

                if (result.IsAcknowledged)
                {
                    if (result.DeletedCount > 0)
                    {
                        return result.DeletedCount == 1;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Deserializes the specified entity.
        /// </summary>
        internal bool Deserialize(out LogicClientAvatar avatar)
        {
            if (this.Profile != null)
            {
                avatar = JsonConvert.DeserializeObject<LogicClientAvatar>(this.Profile.ToJson().Replace("_TypeName", "$type"), AvatarDb.JsonSettings);

                if (avatar != null)
                {
                    return true;
                }
            }
            else
            {
                avatar = null;
            }

            return false;
        }
    }
}