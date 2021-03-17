namespace Supercell.Life.Server.Logic.Slots
{
    using System.Collections.Concurrent;
    using System.Linq;

    using Supercell.Life.Titan.Logic.Enums;

    using Supercell.Life.Server.Logic.Avatar;

    internal static class Waiting
    {
        private static ConcurrentQueue<LogicClientAvatar> Queue;

        private static object Gate;

        /// <summary>
        /// Initializes the <see cref="Waiting"/> class.
        /// </summary>
        internal static void Init()
        {
            Waiting.Queue = new ConcurrentQueue<LogicClientAvatar>();
            Waiting.Gate  = new object();
        }

        /// <summary>
        /// Dequeues and returns a <see cref="LogicClientAvatar"/> from this instance.
        /// </summary>
        internal static LogicClientAvatar Dequeue()
        {
            lock (Waiting.Gate)
            {
                if (Waiting.Queue.Count > 0)
                {
                    if (Waiting.Queue.TryDequeue(out LogicClientAvatar avatar))
                    {
                        if (avatar.Connection.State != State.Matchmaking)
                        {
                            Debugger.Warning("Dequeued avatar was not in a matchmaking state.");

                            while (Waiting.Queue.Count > 0)
                            {
                                if (Waiting.Queue.TryDequeue(out avatar))
                                {
                                    if (avatar.Connection.State == State.Matchmaking)
                                    {
                                        return avatar;
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
                            return avatar;
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
        /// Enqueues the specified player.
        /// </summary>
        internal static void Enqueue(LogicClientAvatar avatar)
        {
            if (!Waiting.Queue.Contains(avatar))
            {
                avatar.Connection.State = State.Matchmaking;
                Waiting.Queue.Enqueue(avatar);
            }
        }
    }
}