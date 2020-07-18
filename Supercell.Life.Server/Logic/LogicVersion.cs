namespace Supercell.Life.Server.Logic
{
    using Supercell.Life.Titan.Logic.Enums;

    using Supercell.Life.Server.Files;
    
    internal static class LogicVersion
    {
        internal const int Major = 0;
        internal const int Minor = 0;
        internal const int Build = 1620;

        private const Mode ServerMode = Mode.Production;

        /// <summary>
        /// Gets the version string.
        /// </summary>
        internal static string VersionString
        {
            get
            {
                return $"{Fingerprint.Version[0]}.{Fingerprint.Version[1]}.{Fingerprint.Version[2]}";
            }
        }

        /// <summary>
        /// Gets the type of the server as a string.
        /// </summary>
        internal static string ServerType
        {
            get
            {
                string type = "prod";

                if (LogicVersion.IsIntegration)
                {
                    type = "integration";
                }
                else if (LogicVersion.IsStage)
                {
                    type = "stage";
                }

                return type;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this server is in production mode.
        /// </summary>
        internal static bool IsProd
        {
            get
            {
                return LogicVersion.ServerMode == Mode.Production;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this server is in stage mode.
        /// </summary>
        internal static bool IsStage
        {
            get
            {
                return LogicVersion.ServerMode == Mode.Stage;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this server is in integration mode.
        /// </summary>
        internal static bool IsIntegration
        {
            get
            {
                return LogicVersion.ServerMode == Mode.Integration;
            }
        }
    }
}
