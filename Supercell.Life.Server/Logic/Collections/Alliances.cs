namespace Supercell.Life.Server.Logic.Collections
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
    using Supercell.Life.Server.Logic.Alliance;

    internal class Alliances
    {
        private static ConcurrentDictionary<long, Alliance> Pool;

        private static int Seed;

        /// <summary>
        /// Gets the count.
        /// </summary>
        internal static int Count
        {
            get
            {
                return Alliances.Pool.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Alliances"/> is initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the <see cref="Alliances"/> class.
        /// </summary>
        internal static void Init()
        {
            if (Alliances.Initialized)
            {
                return;
            }

            Alliances.Pool = new ConcurrentDictionary<long, Alliance>();

            switch (Settings.Database) 
            {
                case DBMS.Mongo:
                {
                    foreach (AllianceDb dbEntry in Mongo.Alliances.Find(db => true).ToList())
                    {
                        if (dbEntry != null)
                        {
                            Alliance alliance = new Alliance(dbEntry.HighID, dbEntry.LowID);

                            JsonConvert.PopulateObject(dbEntry.Profile.ToJson(), alliance, AllianceDb.JsonSettings);

                            Alliances.Add(alliance);
                        }
                    }

                    Alliances.Seed = Mongo.AllianceSeed;

                    break;
                }
                case DBMS.File: 
                {
                    DirectoryInfo directory = new DirectoryInfo($"{System.IO.Directory.GetCurrentDirectory()}/Saves/Alliances/");

                    directory.CreateIfNotExists();
                    directory.DeleteIfExists(".json");

                    Parallel.ForEach(directory.GetFiles("*.json"), file =>
                    {
                        string[] id = Path.GetFileNameWithoutExtension(file.Name).Split('-');
                        Alliances.Add(Alliances.Get(LogicStringUtil.ConvertToInt(id[0]), LogicStringUtil.ConvertToInt(id[1])));
                    });

                    Alliances.Seed = directory.GetFiles("*.json").Length;
                    break;
                }
            }

            Console.WriteLine($"Loaded {Avatars.Count} {((Avatars.Count != 1) ? "avatars" : "avatar")} and {Alliances.Count} {((Alliances.Count != 1) ? "alliances" : "alliance")} into memory." + Environment.NewLine);

            Alliances.Initialized = true;
        }

        /// <summary>
        /// Adds the specified alliance.
        /// </summary>
        internal static void Add(Alliance alliance)
        {
            if (Alliances.Pool.ContainsKey(alliance.Identifier))
            {
                if (!Alliances.Pool.TryUpdate(alliance.Identifier, alliance, alliance))
                {
                    Debugger.Error("Unsuccessfully updated the specified alliance to the dictionary.");
                }
            }
            else
            {
                if (!Alliances.Pool.TryAdd(alliance.Identifier, alliance))
                {
                    Debugger.Error("Unsuccessfully added the specified alliance to the dictionary.");
                }

                if (!alliance.TeamGoalTimer.Started)
                {
                    alliance.TeamGoalTimer.Start();
                }

                alliance.TeamGoalTimer.AdjustSubTick();
                alliance.TeamGoalTimer.FastForward((int)DateTime.UtcNow.Subtract(alliance.Update).TotalSeconds);
            }
        }

        /// <summary>
        /// Removes the specified alliance.
        /// </summary>
        internal static void Remove(Alliance alliance)
        {
            if (Alliances.Pool.ContainsKey(alliance.Identifier))
            {
                if (!Alliances.Pool.TryRemove(alliance.Identifier, out Alliance tmpAlliance))
                {
                    Debugger.Error("Unsuccessfully removed the specified alliance from the dictionary.");
                }
                else
                {
                    if (!tmpAlliance.Equals(alliance))
                    {
                        Debugger.Error("Successfully removed a alliance from the list but the returned alliance was not equal to our alliance.");
                    }
                }
            }

            Alliances.Save(alliance);
        }

        internal static void Delete(Alliance alliance, DBMS database = Settings.Database)
        {
            Alliances.Remove(alliance);

            switch (database)
            {
                case DBMS.Mongo:
                {
                    AllianceDb.Delete(alliance.HighID, alliance.LowID).GetAwaiter().GetResult();
                    break;
                }
                case DBMS.File:
                {
                    new FileInfo($"{Directory.GetCurrentDirectory()}/Saves/Alliances/{alliance.HighID}-{alliance.LowID}.json").DeleteIfExists();
                    break;
                }
            }

            Alliances.Save();
        }

        /// <summary>
        /// Gets the alliance using the specified identifier in the specified database.
        /// </summary>
        internal static Alliance Get(LogicLong identifier, DBMS database = Settings.Database, bool store = true)
        {
            return Alliances.Get(identifier.High, identifier.Low, database);
        }

        /// <summary>
        /// Gets the alliance using the specified identifier in the specified database.
        /// </summary>
        internal static Alliance Get(int highId, int lowId, DBMS database = Settings.Database, bool store = true)
        {
            long id = (long)highId << 32 | (uint)lowId;
            
            if (!Alliances.Pool.TryGetValue(id, out Alliance alliance))
            {
                switch (database)
                {
                    case DBMS.Mongo:
                    {
                        AllianceDb save = Mongo.Alliances.Find(db => db.HighID == highId && db.LowID == lowId).SingleOrDefault();

                        if (save != null)
                        {
                            alliance = Alliances.Load(save.Profile.ToJson());

                            if (alliance == null)
                            {
                                Debugger.Error($"Unable to load alliance with the ID {highId}-{lowId}.");
                            }
                        }

                        break;
                    }
                    case DBMS.File:
                    {
                        FileInfo file = new FileInfo($"{Directory.GetCurrentDirectory()}/Saves/Alliances/{highId}-{lowId}.json");

                        if (file.Exists)
                        {
                            string json = file.ReadAllText();

                            if (!json.IsNullOrEmptyOrWhitespace())
                            {
                                alliance = JsonConvert.DeserializeObject<Alliance>(json, AllianceDb.JsonSettings);
                            }
                            else
                            {
                                Debugger.Error($"The data returned wasn't null but empty, at Get({highId}, {lowId}, File, {store}).");
                            }
                        }
                         
                        break;
                    }
                }
            }

            return alliance;
        }
        
        /// <summary>
        /// Creates a list of random alliances using the specified number.
        /// </summary>
        internal static List<Alliance> GetRandomAlliances(int amount = 50)
        {
            return Alliances.Pool.Values.OrderBy(alliance => Loader.Random.Rand(Alliances.Count)).Take(amount).ToList();
        }
        
        /// <summary>
        /// Puts the alliances in descending order by trophy count.
        /// </summary>
        internal static List<Alliance> OrderByDescending()
        {
            return Alliances.Pool.Values.OrderByDescending(alliance => alliance.Score).Take(200).ToList();
        }
        
        /// <summary>
        /// Creates a alliance with the specified identifier in the specified database.
        /// </summary>
        internal static Alliance Create(LogicLong id, DBMS database = Settings.Database, bool store = true)
        {
            return Alliances.Create(id.High, id.Low, database);
        }

        /// <summary>
        /// Creates a alliance with the specified identifier in the specified database.
        /// </summary>
        internal static Alliance Create(int high = 0, int low = 0, DBMS database = Settings.Database, bool store = true)
        {
            if (low == 0)
            {
                Alliance alliance = new Alliance(high, Interlocked.Increment(ref Alliances.Seed));

                switch (database)
                {
                    case DBMS.Mongo:
                    {
                        AllianceDb.Create(alliance).GetAwaiter().GetResult();
                        break;
                    }
                    case DBMS.File:
                    {
                        FileInfo file = new FileInfo($"{Directory.GetCurrentDirectory()}/Saves/Alliances/{alliance}.json");

                        if (!file.Exists)
                        {
                            file.WriteAllText(JsonConvert.SerializeObject(alliance, AllianceDb.JsonSettings));
                        }

                        break;
                    }
                }

                Alliances.Add(alliance);

                return alliance;
            }

            return null;
        }

        /// <summary>
        /// Updates the specified alliance in the cache.
        /// </summary>
        internal static void Update(Alliance alliance)
        {
            alliance.Update = DateTime.UtcNow;

            if (!Alliances.Pool.TryUpdate(alliance.Identifier, alliance, Alliances.Get(alliance.Identifier)))
            {
                Debugger.Error($"Error updating alliance with ID {alliance.Identifier}");
            }
        }

        /// <summary>
        /// Saves the specified alliance in the specified database.
        /// </summary>
        internal static void Save(Alliance alliance, DBMS database = Settings.Database)
        {
            if (alliance != null)
            {
                alliance.Update = DateTime.UtcNow;

                switch (database)
                {
                    case DBMS.Mongo:
                    {
                        AllianceDb.Save(alliance).GetAwaiter().GetResult();
                        break;
                    }
                    case DBMS.File:
                    {
                        new FileInfo($"{Directory.GetCurrentDirectory()}/Saves/Alliances/{alliance}.json").WriteAllText(JsonConvert.SerializeObject(alliance, AllianceDb.JsonSettings));
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// Uses the specified JSON to initialize an instance of <see cref="Alliance"/>.
        /// </summary>
        private static Alliance Load(string json)
        {
            Alliance alliance = new Alliance();
            JsonConvert.PopulateObject(json, alliance, AllianceDb.JsonSettings);
            return alliance;
        }
        
        /// <summary>
        /// Saves this instance.
        /// </summary>
        internal static void Save(DBMS database = Settings.Database)
        {
            Alliances.ForEach(alliance =>
            {
                try
                {
                    Alliances.Save(alliance, database);
                }
                catch (Exception)
                {
                    Debugger.Debug($"Did not succeed in saving alliance [{alliance}].");
                }
            });
            
            Debugger.Info($"Saved {Alliances.Count} alliances.");
        }

        /// <summary>
        /// Executes an action on every alliance in the collection.
        /// </summary>
        internal static void ForEach(Action<Alliance> action)
        {
            int count = 0;

            Parallel.ForEach(Alliances.Pool.Values, alliance =>
            {
                action.Invoke(alliance);
                count++;
            });

            // Debugger.Debug($"Executed an action on {count} alliances.");
        }
    }
}