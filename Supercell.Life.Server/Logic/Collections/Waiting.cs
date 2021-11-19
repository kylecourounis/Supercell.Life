namespace Supercell.Life.Server.Logic.Collections
{
    using System.Collections.Concurrent;
    using System.Linq;

    using Supercell.Life.Titan.Logic.Enums;

    internal static class Waiting
    {
        private static ConcurrentQueue<LogicGameMode> Queue;

        private static object Gate;

        /// <summary>
        /// Initializes the <see cref="Waiting"/> class.
        /// </summary>
        internal static void Init()
        {
            Waiting.Queue = new ConcurrentQueue<LogicGameMode>();
            Waiting.Gate  = new object();
        }

        /// <summary>
        /// Dequeues and returns a <see cref="LogicGameMode"/> from this instance.
        /// </summary>
        internal static LogicGameMode Dequeue()
        {
            lock (Waiting.Gate)
            {
                if (Waiting.Queue.Count > 0)
                {
                    if (Waiting.Queue.TryDequeue(out LogicGameMode gamemode))
                    {
                        if (gamemode.Connection.State != State.Matchmaking)
                        {
                            Debugger.Warning("Dequeued avatar was not in a matchmaking state.");

                            while (Waiting.Queue.Count > 0)
                            {
                                if (Waiting.Queue.TryDequeue(out gamemode))
                                {
                                    if (gamemode.Connection.State == State.Matchmaking)
                                    {
                                        return gamemode;
                                    }

                                    Debugger.Warning("Dequeued avatar was not in a matchmaking state.");
                                }
                                else
                                {
                                    Debugger.Error("Failed to dequeue inside the while loop.");
                                }
                            }

                            Debugger.Warning("The loop used to search for an avatar currently in the matchmaking queue has terminated.");
                        }
                        else
                        {
                            return gamemode;
                        }
                    }
                    else
                    {
                        Debugger.Warning("Failed to dequeue the avatar.");
                    }
                }
                else
                {
                    Debugger.Warning("Not enough players in the waiting list at Dequeue().");
                }
            }

            return null;
        }

        /// <summary>
        /// Enqueues the specified gamemode.
        /// </summary>
        internal static void Enqueue(LogicGameMode gamemode)
        {
            if (!Waiting.Queue.Contains(gamemode))
            {
                gamemode.Connection.State = State.Matchmaking;
                Waiting.Queue.Enqueue(gamemode);
            }
        }
    }
}