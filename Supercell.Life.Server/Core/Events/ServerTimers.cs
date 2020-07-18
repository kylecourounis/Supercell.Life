namespace Supercell.Life.Server.Core.Events
{
    using System.Collections.Generic;
    using System.Reflection;
    using System.Timers;

    using Supercell.Life.Server.Logic.Slots;
    using Supercell.Life.Server.Network;

    using Debugger = Supercell.Life.Debugger;

    internal class ServerTimers
    {
        /// <summary>
        /// Gets a value indicating whether this <see cref="ServerTimers"/> is initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the <see cref="ServerTimers"/> class.
        /// </summary>
        internal static void Init()
        {
            if (ServerTimers.Initialized)
            {
                return;
            }

            ServerTimers.ReleaseSockets.Start();
            ServerTimers.SaveAll.Start();

            ServerTimers.Initialized = true;
        }

        /// <summary>
        /// An instance of <see cref="Timer"/> that releases any already disconnected sockets.
        /// </summary>
        private static Timer ReleaseSockets
        {
            get
            {
                Timer timer = new Timer
                {
                    Interval  = 30000,
                    AutoReset = true
                };

                var method = MethodBase.GetCurrentMethod();

                timer.Elapsed += delegate
                {
                    List<Connection> dead = Connections.FindAll(connection => !connection.IsConnected);

                    foreach (Connection connection in dead)
                    {
                        ServerConnection.Disconnect(connection.Token.Args);
                    }

                    Debugger.Debug($"Released {dead.Count} Sockets.", method);
                };

                return timer;
            }
        }

        /// <summary>
        /// An instance of <see cref="Timer"/> that periodically saves all clans and avatars.
        /// </summary>
        private static Timer SaveAll
        {
            get
            {
                Timer timer = new Timer
                {
                    Interval  = 20000,
                    AutoReset = true
                };

                timer.Elapsed += delegate
                {
                    Avatars.Save();
                    Alliances.Save();
                };

                return timer;
            }
        }
    }
}
