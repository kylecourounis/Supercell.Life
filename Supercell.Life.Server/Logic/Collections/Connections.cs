namespace Supercell.Life.Server.Logic.Collections
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    using Supercell.Life.Titan.Logic.Enums;

    using Supercell.Life.Server.Network;

    internal static class Connections
    {
        private static ConcurrentDictionary<IntPtr, Connection> Pool;

        /// <summary>
        /// Gets the count.
        /// </summary>
        internal static int Count
        {
            get
            {
                return Connections.Pool.Count;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Connections"/> is initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the <see cref="Connections"/> class.
        /// </summary>
        internal static void Init()
        {
            if (Connections.Initialized)
            {
                return;
            }

            Connections.Pool = new ConcurrentDictionary<IntPtr, Connection>();

            Connections.Initialized = true;
        }

        /// <summary>
        /// Adds the specified connection.
        /// </summary>
        internal static void Add(Connection connection)
        {
            if (Connections.Pool.ContainsKey(connection.Socket.Handle))
            {
                Connections.Pool[connection.Socket.Handle] = connection;
            }
            else
            {
                Connections.Pool.TryAdd(connection.Socket.Handle, connection);
            }
        }

        /// <summary>
        /// Removes the specified connection.
        /// </summary>
        internal static void Remove(Connection connection)
        {
            if (Connections.Pool.ContainsKey(connection.Socket.Handle))
            {
                try
                {
                    connection.Socket.Shutdown(SocketShutdown.Both);
                }
                catch (Exception)
                {
                    // Already Closed.
                }

                connection.State = State.Disconnected;
                connection.Socket.Close();

                Connections.Pool.TryRemove(connection.Socket.Handle, out connection);
            }
        }

        /// <summary>
        /// Finds all.
        /// </summary>
        internal static List<Connection> FindAll(Predicate<Connection> predicate)
        {
            return Connections.Pool.Values.ToList().FindAll(predicate);
        }

        /// <summary>
        /// Executes an action on every connection in the collection.
        /// </summary>
        internal static void ForEach(Action<Connection> action, bool connected = true)
        {
            int index = 0;

            if (connected)
            {
                Parallel.ForEach(Connections.Pool.Values, connection =>
                {
                    if (connection.IsConnected)
                    {
                        action.Invoke(connection);
                        index++;
                    }
                });
            }
            else
            {
                Parallel.ForEach(Connections.Pool.Values, connection =>
                {
                    action.Invoke(connection);
                    index++;
                });
            }

            Debugger.Info($"Executed an action on {index} connections.");
        }
    }
}