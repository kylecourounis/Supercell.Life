namespace Supercell.Life.Server.Core.Events
{
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading;
    using System.Threading.Tasks;

    using Supercell.Life.Titan.Logic.Enums;
    
    using Supercell.Life.Server.Logic.Collections;
    using Supercell.Life.Server.Protocol.Messages;
    using Supercell.Life.Server.Protocol.Messages.Server;

    using Debugger = Supercell.Life.Debugger;

    internal static class EventsHandler
    {
        [DllImport("Kernel32")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool enabled);

        private static EventHandler ExitHandler;
        private delegate void EventHandler();

        /// <summary>
        /// Gets a value indicating whether this <see cref="EventsHandler"/> is initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the <see cref="EventsHandler"/> class.
        /// </summary>
        internal static void Init()
        {
            if (EventsHandler.Initialized)
            {
                return;
            }

            EventsHandler.ExitHandler += EventsHandler.Exit;
            EventsHandler.SetConsoleCtrlHandler(EventsHandler.ExitHandler, true);

            EventsHandler.Initialized = true;
        }

        /// <summary>
        /// Shuts down the server.
        /// </summary>
        internal static void Exit()
        {
            var method = MethodBase.GetCurrentMethod();

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Shutting down...");

            Debugger.Info("Server is shutting down...");

            Task task = Task.Run(() =>
            {
                if (Connections.Count > 0)
                {
                    Connections.ForEach(connection =>
                    {
                        if (connection != null && connection.State >= State.LoggedIn)
                        {
                            new MaintenanceInboundMessage(connection).Send();
                            new DisconnectedMessage(connection).Send();
                        }
                    });

                    Debugger.Info("Warned every player about the maintenance.", method);
                }
            });
            
            int timeout = 1000;

            bool saveAvatars = false;
            bool saveAlliances = false;

            if (Avatars.Count > 0)
            {
                saveAvatars = true;
                timeout    += 500;
            }

            if (Alliances.Count > 0)
            {
                saveAlliances = true;
                timeout      += 500;
            }

            if (saveAvatars || saveAlliances)
            {
                Task pTask = new Task(() => Avatars.Save());
                Task cTask = new Task(() => Alliances.Save());

                if (saveAvatars)
                    pTask.Start();
                if (saveAlliances)
                    cTask.Start();

                if (saveAvatars)
                    pTask.Wait();
                if (saveAlliances)
                    cTask.Wait();
            }
            else
            {
                Debugger.Info("Server has nothing to save, shutting down immediately.");
            }

            task.Wait(5000);
            
            Thread.Sleep(timeout);
            Environment.Exit(0);
        }
    }
}