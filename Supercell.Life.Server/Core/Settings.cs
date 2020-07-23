namespace Supercell.Life.Server.Core
{
    using System;

    using Supercell.Life.Titan.Logic.Enums;

    using Supercell.Life.Server.Database;

    internal static class Settings
    {
        /// <summary>
        /// Whether we should save/find player in <see cref="Mongo"/> or a local file.
        /// </summary>
        internal const DBMS Database = DBMS.Mongo;

        /// <summary>
        /// Gets the maintenance time.
        /// </summary>
        internal static DateTime MaintenanceTime
        {
            get
            {
                return DateTime.UtcNow.AddMinutes(-1);
            }
        }

        /// <summary>
        /// Array of IP Address authorized to log in to the server even if it's in maintenance/updating/local.
        /// </summary>
        internal static readonly string[] AuthorizedIP =
        {
        };
    }
}