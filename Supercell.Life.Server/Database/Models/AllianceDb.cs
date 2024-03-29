﻿namespace Supercell.Life.Server.Database.Models
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using MongoDB.Bson;
    using MongoDB.Bson.Serialization.Attributes;
    using MongoDB.Driver;

    using Newtonsoft.Json;

    using Supercell.Life.Server.Helpers.Json;
    using Supercell.Life.Server.Logic.Alliance;

    internal class AllianceDb
    {
        /// <summary>
        /// The settings for the <see cref="JsonConvert"/> class.
        /// </summary>
        [BsonIgnore]
        internal static readonly JsonSerializerSettings JsonSettings = new JsonSerializerSettings
        {
            TypeNameHandling            = TypeNameHandling.None,            MissingMemberHandling   = MissingMemberHandling.Ignore,
            DefaultValueHandling        = DefaultValueHandling.Include,     NullValueHandling       = NullValueHandling.Ignore,
            ReferenceLoopHandling       = ReferenceLoopHandling.Ignore,     Converters              = new List<JsonConverter> { new TimerConverter(), new AllianceStreamEntryConverter() },
            Formatting                  = Formatting.Indented
        };

        [BsonId] public BsonObjectId Id;

        [BsonElement("HighID")] public int HighID;
        [BsonElement("LowID")] public int LowID;

        [BsonElement("Profile")] public BsonDocument Profile;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceDb"/> class.
        /// </summary>
        internal AllianceDb(int highId, int lowId, string json)
        {
            this.HighID  = highId;
            this.LowID   = lowId;
            this.Profile = BsonDocument.Parse(json);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllianceDb"/> class.
        /// </summary>
        internal AllianceDb(Alliance alliance) : this(alliance.HighID, alliance.LowID, JsonConvert.SerializeObject(alliance, AllianceDb.JsonSettings))
        {
            // AllianceDb.
        }

        /// <summary>
        /// Creates the specified alliance.
        /// </summary>
        internal static async Task Create(Alliance alliance)
        {
            await Mongo.Alliances.InsertOneAsync(new AllianceDb(alliance));
        }

        /// <summary>
        /// Creates the alliance in the database.
        /// </summary>
        internal static async Task<AllianceDb> Save(Alliance alliance)
        {
            var updatedEntity = await Mongo.Alliances.FindOneAndUpdateAsync(allianceDb =>
                allianceDb.HighID == alliance.HighID &&
                allianceDb.LowID == alliance.LowID,

                Builders<AllianceDb>.Update.Set(allianceDb => allianceDb.Profile, BsonDocument.Parse(JsonConvert.SerializeObject(alliance, AllianceDb.JsonSettings)))
            );

            if (updatedEntity != null)
            {
                if (updatedEntity.HighID == alliance.HighID && updatedEntity.LowID == alliance.LowID)
                {
                    return updatedEntity;
                }
            }

            return null;
        }

        /// <summary>
        /// Loads this instance from the database.
        /// </summary>
        public static async Task<AllianceDb> Load(int highId, int lowId)
        {
            if (lowId > 0)
            {
                var entities = await Mongo.Alliances.FindAsync(allianceDb => allianceDb.HighID == highId && allianceDb.LowID == lowId);

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
        public static async Task<bool> Delete(int highId, int lowId)
        {
            if (lowId > 0)
            {
                var result = await Mongo.Alliances.DeleteOneAsync(allianceDb => allianceDb.HighID == highId && allianceDb.LowID == lowId);

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
        public bool Deserialize(out Alliance alliance)
        {
            if (this.Profile != null)
            {
                alliance = JsonConvert.DeserializeObject<Alliance>(this.Profile.ToJson(), AllianceDb.JsonSettings);

                if (alliance != null)
                {
                    return true;
                }
            }
            else
            {
                alliance = null;
            }

            return false;
        }
    }
}
