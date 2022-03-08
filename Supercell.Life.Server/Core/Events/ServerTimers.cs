namespace Supercell.Life.Server.Core.Events
{
    using System;
    using System.Timers;

    using Supercell.Life.Server.Logic.Collections;

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

            ServerTimers.TeamGoalTimers.Start();
            ServerTimers.SaveAll.Start();

            ServerTimers.Initialized = true;
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

        /// <summary>
        /// An instance of <see cref="Timer"/> that periodically updates the Team Goal timers.
        /// </summary>
        private static Timer TeamGoalTimers
        {
            get
            {
                Timer timer = new Timer
                {
                    Interval = 10000,
                    AutoReset = true
                };

                timer.Elapsed += delegate
                {
                    Alliances.ForEach(alliance =>
                    {
                        alliance.TeamGoalTimer.Tick();
                        alliance.Update = DateTime.UtcNow;
                    });
                };

                return timer;
            }
        }
    }
}
