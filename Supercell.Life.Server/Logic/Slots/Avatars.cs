namespace Supercell.Life.Server.Logic.Slots
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using MongoDB.Bson;
    using MongoDB.Driver;

    using Newtonsoft.Json;

    using Supercell.Life.Titan.Helpers;
    using Supercell.Life.Titan.Logic.Enums;
    using Supercell.Life.Titan.Logic.Math;
    using Supercell.Life.Titan.Logic.Utils;

    using Supercell.Life.Server.Core;
    using Supercell.Life.Server.Database;
    using Supercell.Life.Server.Database.Models;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Network;

    internal class Avatars
    {
        private static ConcurrentDictionary<long, LogicClientAvatar> Pool;

        private static int Seed;

        /// <summary>
        /// Gets the count.
        /// </summary>
        internal static int Count
        {
            get
            {
                return Avatars.Pool.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Avatars"/> is initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the <see cref="Avatars"/> class.
        /// </summary>
        internal static void Init()
        {
            if (Avatars.Initialized)
            {
                return;
            }

            Avatars.Pool = new ConcurrentDictionary<long, LogicClientAvatar>();

            switch (Settings.Database)
            {
                case DBMS.Mongo:
                {
                    foreach (AvatarDb dbEntry in Mongo.Avatars.Find(db => true).ToList())
                    {
                        if (dbEntry != null)
                        {
                            LogicClientAvatar avatar = new LogicClientAvatar(null, new LogicLong(dbEntry.HighID, dbEntry.LowID));

                            JsonConvert.PopulateObject(dbEntry.Profile.ToJson(), avatar, AvatarDb.JsonSettings);

                            Avatars.Add(avatar);
                        }
                    }

                    Avatars.Seed = Mongo.AvatarSeed;
                        
                    break;
                }
                case DBMS.File: 
                {
                    DirectoryInfo directory = new DirectoryInfo($"{System.IO.Directory.GetCurrentDirectory()}/Saves/Players/");

                    directory.CreateIfNotExists();
                    directory.DeleteIfExists(".json");

                    Parallel.ForEach(directory.GetFiles("*.json"), file =>
                    {
                        string[] id = Path.GetFileNameWithoutExtension(file.Name).Split('-');
                        Avatars.Add(Avatars.Get(new LogicLong(LogicStringUtil.ConvertToInt(id[0]), LogicStringUtil.ConvertToInt(id[1]))));
                    });

                    Avatars.Seed = directory.GetFiles("*.json").Length;
                    break;
                }
            }

            Avatars.Initialized = true;
        }

        /// <summary>
        /// Adds the specified avatar.
        /// </summary>
        internal static void Add(LogicClientAvatar avatar)
        {
            if (Avatars.Pool.ContainsKey(avatar.Identifier))
            {
                if (!Avatars.Pool.TryUpdate(avatar.Identifier, avatar, avatar))
                {
                    Debugger.Error("Unsuccessfully updated the specified player to the dictionary.");
                }
            }
            else
            {
                if (!Avatars.Pool.TryAdd(avatar.Identifier, avatar))
                {
                    Debugger.Error("Unsuccessfully added the specified player to the dictionary.");
                }
            }
        }

        /// <summary>
        /// Removes the specified avatar.
        /// </summary>
        internal static void Remove(LogicClientAvatar avatar)
        {
            if (Avatars.Pool.ContainsKey(avatar.Identifier))
            {
                if (!Avatars.Pool.TryRemove(avatar.Identifier, out LogicClientAvatar tmpAvatar))
                {
                    Debugger.Error("Unsuccessfully removed the specified player from the dictionary.");
                }
                else
                {
                    if (!tmpAvatar.Equals(avatar))
                    {
                        Debugger.Error("Successfully removed a player from the list but the returned player was not equal to the player.");
                    }
                }
            }

            avatar.Save();
        }

        /// <summary>
        /// Gets the player using the specified identifier in the specified database.
        /// </summary>
        internal static LogicClientAvatar Get(LogicLong id, DBMS database = Settings.Database)
        {
            if (!Avatars.Pool.TryGetValue(id, out LogicClientAvatar avatar))
            {
                switch (database)
                {
                    case DBMS.Mongo:
                    {
                        AvatarDb save = Mongo.Avatars.Find(db => db.HighID == id.High && db.LowID == id.Low).SingleOrDefault();

                        if (save != null)
                        {
                            avatar = Avatars.Load(save.Profile.ToJson());

                            if (avatar == null)
                            {
                                Debugger.Error($"Unable to load account with the ID {id}, recreating...");
                                avatar = Avatars.Create(null, id);
                            }
                        }

                        break;
                    }
                    case DBMS.File:
                    {
                        FileInfo file = new FileInfo($"{Directory.GetCurrentDirectory()}/Saves/Players/{id}.json");

                        if (file.Exists)
                        {
                            string json = file.ReadAllText();

                            if (!json.IsNullOrEmptyOrWhitespace())
                            {
                                avatar = JsonConvert.DeserializeObject<LogicClientAvatar>(json, AvatarDb.JsonSettings);
                            }
                            else
                            {
                                Debugger.Error($"The data returned was null/empty/whitespace at Get({id}, {database}).");
                            }
                        }
                            
                        break;
                    }
                }
            }

            return avatar;
        }

        /// <summary>
        /// Gets the player using the specified identifier in the specified database.
        /// </summary>
        internal static LogicClientAvatar Get(Connection connection, LogicLong id, DBMS database = Settings.Database)
        {
            int high = id.High;
            int low  = id.Low;

            if (!Avatars.Pool.TryGetValue(id, out LogicClientAvatar avatar))
            {
                switch (database)
                {
                    case DBMS.Mongo:
                    {
                        AvatarDb save = AvatarDb.Load(high, low).GetAwaiter().GetResult();

                        if (save != null)
                        {
                            avatar = Avatars.Load(save.Profile.ToJson());

                            if (avatar == null)
                            {
                                Debugger.Error($"Unable to load account with the ID {id}.");
                                return null;
                            }

                            avatar.Connection = connection;
                            connection.Avatar = avatar;
                        }

                        break;
                    }
                    case DBMS.File:
                    {
                        FileInfo file = new FileInfo($"{Directory.GetCurrentDirectory()}/Saves/Players/{id}.json");

                        if (file.Exists)
                        {
                            string json = file.ReadAllText();

                            if (!json.IsNullOrEmptyOrWhitespace())
                            {
                                avatar = JsonConvert.DeserializeObject<LogicClientAvatar>(json, AvatarDb.JsonSettings);

                                avatar.Connection = connection;
                                connection.Avatar = avatar;
                            }
                            else
                            {
                                Debugger.Error($"The data returned was null/empty/whitespace at Get({id}, {database}).");
                            }
                        }

                        break;
                    }
                }
            }

            return avatar;
        }

        /// <summary>
        /// Creates a new player using specified identifier.
        /// </summary>
        internal static LogicClientAvatar Create(Connection connection, LogicLong id, DBMS database = Settings.Database)
        {
            return Avatars.Create(connection, id.High, id.Low, database);
        }

        /// <summary>
        /// Creates a new player using the specified identifier in the specified database.
        /// </summary>
        internal static LogicClientAvatar Create(Connection connection, int highId = 0, int lowId = 0, DBMS database = Settings.Database)
        {
            int low = (lowId == 0) ? Interlocked.Increment(ref Avatars.Seed) : lowId;

            LogicClientAvatar avatar = new LogicClientAvatar(connection, new LogicLong(highId, low))
            {
                Connection = connection,
                Token      = Loader.Random.GenerateRandomString()
            };
            
            switch (database)
            {
                case DBMS.Mongo:
                {
                    AvatarDb.Create(avatar).GetAwaiter().GetResult();
                    break;
                }
                case DBMS.File:
                {
                    FileInfo file = new FileInfo($"{Directory.GetCurrentDirectory()}/Saves/Players/{avatar}.json");

                    if (!file.Exists) 
                    { 
                        file.WriteAllText(JsonConvert.SerializeObject(avatar, AvatarDb.JsonSettings));
                    }

                    break;
                }
            }

            Avatars.Add(avatar);

            return avatar;
        }
        
        /// <summary>
        /// Saves the specified player in the specified database.
        /// </summary>
        internal static async void Save(LogicClientAvatar avatar, DBMS database = Settings.Database)
        {
            switch (database)
            {
                case DBMS.Mongo:
                {
                    await AvatarDb.Save(avatar);
                    break;
                }
                case DBMS.File:
                {
                    FileInfo file = new FileInfo($"{Directory.GetCurrentDirectory()}/Saves/Players/{avatar}.json");

                    if (file.Exists)
                    {
                        file.WriteAllText(JsonConvert.SerializeObject(avatar, AvatarDb.JsonSettings));
                    }

                    break;
                }
            }
        }

        /// <summary>
        /// Deletes the specified player in the specified database.
        /// </summary>
        internal static async void Delete(LogicClientAvatar avatar, DBMS database = Settings.Database)
        {
            switch (database)
            {
                case DBMS.Mongo:
                {
                    await AvatarDb.Delete(avatar.HighID, avatar.LowID);
                    break;
                }
                case DBMS.File:
                {
                    new FileInfo($"{Directory.GetCurrentDirectory()}/Saves/Players/{avatar}.json").DeleteIfExists();
                    break;
                }
            }
        }

        /// <summary>
        /// Deletes the specified player in the specified database.
        /// </summary>
        internal static async void Delete(int high, int low, DBMS database = Settings.Database)
        {
            switch (database)
            {
                case DBMS.Mongo:
                {
                    await AvatarDb.Delete(high, low);
                    break;
                }
                case DBMS.File:
                {
                    new FileInfo($"{Directory.GetCurrentDirectory()}/Saves/Players/{high}-{low}.json").DeleteIfExists();
                    break;
                }
            }
        }

        /// <summary>
        /// Puts the players in descending order by trophy count.
        /// </summary>
        internal static List<LogicClientAvatar> OrderByDescending()
        {
            return Avatars.Pool.Values.OrderByDescending(avatar => avatar.Score).Take(200).ToList();
        }

        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the specified predicate.
        /// </summary>
        internal static List<LogicClientAvatar> FindAll(Predicate<LogicClientAvatar> predicate)
        {
            return Avatars.Pool.Values.ToList().FindAll(predicate);
        }

        /// <summary>
        /// Uses the specified JSON to initialize an instance of <see cref="LogicClientAvatar"/>.
        /// </summary>
        private static LogicClientAvatar Load(string json)
        {
            LogicClientAvatar avatar = new LogicClientAvatar();
            JsonConvert.PopulateObject(json, avatar, AvatarDb.JsonSettings);
            return avatar;
        }

        /// <summary>
        /// Saves the specified DBMS.
        /// </summary>
        internal static void Save(DBMS database = Settings.Database, bool connected = false)
        {
            Avatars.ForEach(avatar =>
            {
                try
                {
                    avatar.Save(database);
                }
                catch (Exception exception)
                {
                    Debugger.Error($"{exception.GetType().Name} : did not succeed in saving player [{avatar}].");
                }
            }, connected);

            Debugger.Info($"Saved {Avatars.Count} avatars.");
        }

        /// <summary>
        /// Executes an action on every avatar in the collection.
        /// </summary>
        internal static void ForEach(Action<LogicClientAvatar> action, bool connected = true)
        {
            int count = 0;

            if (connected)
            {
                Parallel.ForEach(Avatars.Pool.Values, avatar =>
                {
                    if (avatar.Connection != null)
                    {
                        if (avatar.Connection.IsConnected)
                        {
                            action.Invoke(avatar);
                            count++;
                        }
                    }
                });
            }
            else
            {
                Parallel.ForEach(Avatars.Pool.Values, avatar =>
                {
                    action.Invoke(avatar);
                    count++;
                });
            }

            Debugger.Debug($"Executed an action on {count} avatars.");
        }
    }
}