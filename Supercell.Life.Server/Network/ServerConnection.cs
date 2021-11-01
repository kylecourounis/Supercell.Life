namespace Supercell.Life.Server.Network
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    using Supercell.Life.Titan.Core;

    using Supercell.Life.Server.Logic.Slots;
    using Supercell.Life.Server.Protocol;
    using Supercell.Life.Server.Protocol.Messages;

    internal static class ServerConnection
    {
        private static Pool ReadPool;
        private static Pool WritePool;

        private static Socket Listener;

        /// <summary>
        /// Gets a value indicating whether this <see cref="ServerConnection"/> is initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the <see cref="ServerConnection"/> class.
        /// </summary>
        internal static void Init()
        {
            if (ServerConnection.Initialized)
            {
                return;
            }

            try
            {
                ServerConnection.Listener  = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp)
                {
                    ReceiveBufferSize = Constants.BufferSize,
                    SendBufferSize    = Constants.BufferSize,
                    Blocking          = false,
                    NoDelay           = true
                };

                ServerConnection.InitPools();

                ServerConnection.Initialized = true;

                ServerConnection.Listener.Bind(new IPEndPoint(IPAddress.Any, 9339));
                ServerConnection.Listener.Listen(300);
                
                Console.WriteLine($"TCP Server is listening on {ServerConnection.Listener.LocalEndPoint}.");
                
                SocketAsyncEventArgs acceptEvent = new SocketAsyncEventArgs();
                acceptEvent.Completed += ServerConnection.OnAcceptCompleted;

                ServerConnection.StartAccept(acceptEvent);
            }
            catch (SocketException)
            {
                Console.WriteLine("Port is already in use! Press any key to exit...");

                Console.ReadKey(true);
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Initializes the read/write pools.
        /// </summary>
        private static void InitPools()
        {
            ServerConnection.ReadPool  = new Pool(Constants.MaxPlayers);
            ServerConnection.WritePool = new Pool(Constants.MaxSends);

            for (int i = 0; i < Constants.MaxPlayers; i++)
            {
                SocketAsyncEventArgs readEvent     = new SocketAsyncEventArgs();

                readEvent.SetBuffer(new byte[Constants.BufferSize], 0, Constants.BufferSize);

                readEvent.Completed               += ServerConnection.OnReceiveCompleted;
                readEvent.DisconnectReuseSocket    = true;

                ServerConnection.ReadPool.Enqueue(readEvent);
            }

            for (int i = 0; i < Constants.MaxSends; i++)
            {
                SocketAsyncEventArgs writeEvent    = new SocketAsyncEventArgs();

                writeEvent.Completed              += ServerConnection.OnSendCompleted;
                writeEvent.DisconnectReuseSocket   = true;

                ServerConnection.WritePool.Enqueue(writeEvent);
            }
        }

        /// <summary>
        /// Accepts a TCP Request.
        /// </summary>
        private static void StartAccept(SocketAsyncEventArgs acceptEvent)
        {
            acceptEvent.AcceptSocket   = null;
            acceptEvent.RemoteEndPoint = null;

            if (!ServerConnection.Listener.AcceptAsync(acceptEvent))
            {
                ServerConnection.OnAcceptCompleted(null, acceptEvent);
            }
        }

        /// <summary>
        /// Initializes the encryption.
        /// </summary>
        internal static void InitializeEncryption(Messaging messaging)
        {
            messaging.SetEncrypters();
        }

        /// <summary>
        /// Called when the client has been accepted.
        /// </summary>
        private static void OnAcceptCompleted(object sender, SocketAsyncEventArgs asyncEvent)
        {
            if (asyncEvent.SocketError == SocketError.Success)
            {
                ServerConnection.ProcessAccept(asyncEvent);
            }
            else
            {
                asyncEvent.AcceptSocket.Close();

                Debugger.Error("Something happened when accepting a new connection, aborting.");

                ServerConnection.StartAccept(asyncEvent);
            }
        }

        /// <summary>
        /// Accept the new client and store it in memory.
        /// </summary>
        private static void ProcessAccept(SocketAsyncEventArgs asyncEvent)
        {
            Socket socket = asyncEvent.AcceptSocket;

            if (socket.Connected)
            {
                SocketAsyncEventArgs readEvent = ServerConnection.ReadPool.Dequeue();

                if (readEvent == null)
                {
                    readEvent = new SocketAsyncEventArgs();

                    readEvent.SetBuffer(new byte[Constants.BufferSize], 0, Constants.BufferSize);
                    readEvent.Completed += ServerConnection.OnReceiveCompleted;

                    readEvent.DisconnectReuseSocket = false;
                }

                Connection connection = new Connection(socket);
                Token token           = new Token(readEvent, connection);

                connection.Messaging.MessageManager.StartThreads();
                ServerConnection.InitializeEncryption(connection.Messaging);

                Connections.Add(connection);

                if (!socket.ReceiveAsync(readEvent))
                {
                    ServerConnection.ProcessReceive(readEvent);
                }
            }
            else
            {
                socket.Close();
            }

            ServerConnection.StartAccept(asyncEvent);
        }

        /// <summary>
        /// Receives data from the specified client.
        /// </summary>
        private static void ProcessReceive(SocketAsyncEventArgs asyncEvent)
        {
            if (asyncEvent.BytesTransferred > 0 && asyncEvent.SocketError == SocketError.Success)
            {
                if (asyncEvent.UserToken is Token token && !token.Aborting)
                {
                    token.SetData();

                    try
                    {
                        if (token.Connection.Socket.Available == 0)
                        {
                            token.Process();

                            if (!token.Aborting)
                            {
                                if (!token.Connection.Socket.ReceiveAsync(asyncEvent))
                                {
                                    ServerConnection.ProcessReceive(asyncEvent);
                                }
                            }
                        }
                        else
                        {
                            if (!token.Connection.Socket.ReceiveAsync(asyncEvent))
                            {
                                ServerConnection.ProcessReceive(asyncEvent);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        ServerConnection.Disconnect(asyncEvent);
                    }
                }
            }
            else
            {
                ServerConnection.Disconnect(asyncEvent);
            }
        }

        /// <summary>
        /// Called when [receive completed].
        /// </summary>
        private static void OnReceiveCompleted(object sender, SocketAsyncEventArgs asyncEvent)
        {
            if (asyncEvent.SocketError == SocketError.Success)
            {
                ServerConnection.ProcessReceive(asyncEvent);
            }
            else
            {
                ServerConnection.Disconnect(asyncEvent);
            }
        }

        /// <summary>
        /// Sends the specified message.
        /// </summary>
        internal static void Send(PiranhaMessage message)
        {
            if (message.Connection.IsConnected)
            {
                SocketAsyncEventArgs writeEvent = ServerConnection.WritePool.Dequeue() ?? new SocketAsyncEventArgs
                {
                    DisconnectReuseSocket = false
                };

                writeEvent.SetBuffer(message.ToArray(), message.Offset, message.Length + 7 - message.Offset);

                writeEvent.AcceptSocket   = message.Connection.Socket;
                writeEvent.RemoteEndPoint = message.Connection.EndPoint;
                writeEvent.UserToken      = message.Connection.Token;

                if (!message.Connection.Socket.SendAsync(writeEvent))
                {
                    ServerConnection.ProcessSend(message, writeEvent);
                }
            }
            else
            {
                ServerConnection.Disconnect(message.Connection?.Token?.Args);
            }
        }

        /// <summary>
        /// Processes to send the specified message using the specified SocketAsyncEventArgs.
        /// </summary>
        private static void ProcessSend(PiranhaMessage message, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                message.Offset += args.BytesTransferred;

                if (message.Length + 7 > message.Offset)
                {
                    if (message.Connection.IsConnected)
                    {
                        args.SetBuffer(message.Offset, (message.Length + 7 - message.Offset));

                        if (!message.Connection.Socket.SendAsync(args))
                        {
                            ServerConnection.ProcessSend(message, args);
                        }
                    }
                    else
                    {
                        ServerConnection.OnSendCompleted(null, args);
                        ServerConnection.Disconnect(message.Connection.Token.Args);
                    }
                }
                else
                {
                    ServerConnection.OnSendCompleted(null, args);
                }
            }
            else
            {
                ServerConnection.OnSendCompleted(null, args);
                ServerConnection.Disconnect(message.Connection.Token.Args);
            }
        }

        /// <summary>
        /// Called when [send completed].
        /// </summary>
        private static void OnSendCompleted(object sender, SocketAsyncEventArgs asyncEvent)
        {
            if (asyncEvent.DisconnectReuseSocket)
            {
                ServerConnection.WritePool.Enqueue(asyncEvent);
            }
            else
            {
                asyncEvent.Dispose();
            }
        }

        /// <summary>
        /// Closes the specified client's socket.
        /// </summary>
        internal static void Disconnect(SocketAsyncEventArgs asyncEvent)
        {
            if (asyncEvent == null) return;

            Token token = asyncEvent.UserToken as Token;

            if (token.Aborting)
            {
                return;
            }

            token.Aborting = true;

            Connections.Remove(token.Connection);

            if (asyncEvent.DisconnectReuseSocket)
            {
                ServerConnection.ReadPool.Enqueue(asyncEvent);
            }
            else
            {
                asyncEvent.Dispose();
            }

            token.Dispose();
        }
    }
}
