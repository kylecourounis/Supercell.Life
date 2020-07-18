namespace Supercell.Life.Server.Files
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Threading.Tasks;

    using Supercell.Life.Titan.Files.CsvReader;

    using Supercell.Life.Server.Logic.Enums;

    internal static class CSV
    {
        internal static readonly Dictionary<Gamefile, string> Gamefiles = new Dictionary<Gamefile, string>();

        internal static Gamefiles Tables;

        /// <summary>
        /// Gets a value indicating whether this instance of <see cref="CSV"/> has been initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the <see cref="CSV"/> class.
        /// </summary>
        internal static void Init()
        {
            if (CSV.Initialized)
            {
                return;
            }

            CSV.Tables = new Gamefiles();

            CSV.Gamefiles.Add(Gamefile.Locales, "csv_logic/locales.csv");
            CSV.Gamefiles.Add(Gamefile.Resources, "csv_logic/resources.csv");
            CSV.Gamefiles.Add(Gamefile.Effects, "csv_logic/effects.csv");
            CSV.Gamefiles.Add(Gamefile.ParticleEmitters, "csv_logic/particle_emitters.csv");
            CSV.Gamefiles.Add(Gamefile.Globals, "csv_logic/globals.csv");
            CSV.Gamefiles.Add(Gamefile.Quests, "csv_logic/quests.csv");

            CSV.Gamefiles.Add(Gamefile.Achievements, "csv_logic/achievements.csv");

            CSV.Gamefiles.Add(Gamefile.Worlds, "csv_logic/worlds.csv");
            CSV.Gamefiles.Add(Gamefile.Heroes, "csv_logic/heroes.csv");

            CSV.Gamefiles.Add(Gamefile.ExperienceLevels, "csv_logic/experience_levels.csv");
            CSV.Gamefiles.Add(Gamefile.Leagues, "csv_logic/leagues.csv");

            CSV.Gamefiles.Add(Gamefile.AllianceBadges, "csv_logic/alliance_badges.csv");
            
            CSV.Gamefiles.Add(Gamefile.Taunts, "csv_logic/taunts.csv");
            CSV.Gamefiles.Add(Gamefile.Decos, "csv_logic/decos.csv");

            CSV.Gamefiles.Add(Gamefile.Variables, "csv_logic/variables.csv");

            CSV.Gamefiles.Add(Gamefile.Boosters, "csv_logic/boosters.csv");
            CSV.Gamefiles.Add(Gamefile.EnergyPackages, "csv_logic/energy_packages.csv");

            CSV.Gamefiles.Add(Gamefile.Spells, "csv_logic/spells.csv");
            CSV.Gamefiles.Add(Gamefile.Obstacles, "csv_logic/obstacles.csv");
            CSV.Gamefiles.Add(Gamefile.Items, "csv_logic/items.csv");

            Parallel.ForEach(CSV.Gamefiles, file =>
            {
                var (key, value) = file;

                if (new FileInfo($"Gamefiles/{value}").Exists)
                {
                    CSV.Tables.Initialize(new Table($"Gamefiles/{value}"), (int)key);
                }
                else
                {
                    Debugger.Error($"The CSV file at {value} is missing, aborting.");
                }
            });
            
            Console.WriteLine($"Loaded {CSV.Gamefiles.Count} Gamefiles.");

            CSV.Initialized = true;
        }
    }
}