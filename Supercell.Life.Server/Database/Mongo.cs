namespace Supercell.Life.Server.Database
{
    using System;

    using MongoDB.Driver;

    using Supercell.Life.Server.Database.Models;

    internal static class Mongo
    {
        internal static IMongoCollection<AvatarDb> Avatars;
        internal static IMongoCollection<AllianceDb> Alliances;

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Mongo"/> has been already initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes the <see cref="Mongo"/> class.
        /// </summary>
        public static void Init()
        {
            if (Mongo.Initialized)
            {
                return;
            }

            var mongoClient = new MongoClient("mongodb://127.0.0.1:27017/");
            var mongoDb     = mongoClient.GetDatabase("SmashLand");

            Console.WriteLine($"Connected to Mongo Database at {mongoClient.Settings.Server.Host}.");
            Console.WriteLine();

            if (mongoDb.GetCollection<AvatarDb>("Avatars") == null)
            {
                mongoDb.CreateCollection("Avatars");
            }
            if (mongoDb.GetCollection<AllianceDb>("Alliances") == null)
            {
                mongoDb.CreateCollection("Alliances");
            }

            Mongo.Avatars   = mongoDb.GetCollection<AvatarDb>("Avatars");
            Mongo.Alliances = mongoDb.GetCollection<AllianceDb>("Alliances");

            Mongo.Avatars.Indexes.CreateOne(Builders<AvatarDb>.IndexKeys.Combine(
                Builders<AvatarDb>.IndexKeys.Ascending(db => db.HighID),
                Builders<AvatarDb>.IndexKeys.Descending(db => db.LowID)),

                new CreateIndexOptions
                {
                    Name = "entityIds",
                    Background = true
                }
            );

            Mongo.Alliances.Indexes.CreateOne(Builders<AllianceDb>.IndexKeys.Combine(
                Builders<AllianceDb>.IndexKeys.Ascending(db => db.HighID),
                Builders<AllianceDb>.IndexKeys.Descending(db => db.LowID)),

                new CreateIndexOptions
                {
                    Name = "entityIds",
                    Background = true
                }
            );

            Mongo.Initialized = true;
        }


        /// <summary>
        /// Gets the seed for the specified collection.
        /// </summary>
        internal static int AvatarSeed
        {
            get
            {
                return Mongo.Avatars.Find(db => db.HighID == 0)
                           .Sort(Builders<AvatarDb>.Sort.Descending(db => db.LowID))
                           .Limit(1)
                           .SingleOrDefault()?.LowID ?? 0;
            }
        }

        /// <summary>
        /// Gets the seed for the specified collection.
        /// </summary>
        internal static int AllianceSeed
        {
            get
            {
                return Mongo.Alliances.Find(db => db.HighID == 0)
                           .Sort(Builders<AllianceDb>.Sort.Descending(db => db.LowID))
                           .Limit(1)
                           .SingleOrDefault()?.LowID ?? 0;
            }
        }
    }
}