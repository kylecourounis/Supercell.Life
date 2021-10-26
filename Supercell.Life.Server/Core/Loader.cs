namespace Supercell.Life.Server.Core
{
    using Supercell.Life.Titan.Logic.Enums;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Core.API;
    using Supercell.Life.Server.Core.Events;
    using Supercell.Life.Server.Database;
    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Logic.Game;
    using Supercell.Life.Server.Logic.Slots;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol;
    using Supercell.Life.Server.Protocol.Commands;

    internal static class Loader
    {
        internal static LogicRandom Random;

        /// <summary>
        /// Gets a value indicating whether this <see cref="Loader"/> is initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Initializes the <see cref="Loader"/> class.
        /// </summary>
        internal static void Init()
        {
            if (Loader.Initialized)
            {
                return;
            }
            
            Loader.Random = new LogicRandom();

            LogicCommandManager.Init();
            MessageManager.Init();

            Fingerprint.Init();
            CSV.Init();
            Levels.Init();
            Globals.Init();

            if (Settings.Database == DBMS.Mongo)
            {
                Mongo.Init();
            }

            Connections.Init();
            Avatars.Init();
            Alliances.Init();
            Battles.Init();

            ServerConnection.Init();
            APIHandler.Init();

            ServerTimers.Init();

            Loader.Initialized = true;
            
            EventsHandler.Init();
            Tests.Init();
        }
    }
}