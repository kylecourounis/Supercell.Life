namespace Supercell.Life.Server.Protocol
{
    using System;
    using System.Collections.Concurrent;
    using System.Threading;

    using Supercell.Life.Titan.Helpers;

    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Messages;

    internal class MessageManager
    {
        private static Thread SendThread;
        private static Thread ReceiveThread;

        private static ConcurrentQueue<PiranhaMessage> SendQueue;
        private static ConcurrentQueue<PiranhaMessage> ReceiveQueue;

        private static AutoResetEvent SendResetEvent;
        private static AutoResetEvent ReceiveResetEvent;

        private static bool Started;

        /// <summary>
        /// Gets a value indicating whether this <see cref="MessageManager"/> is initialized.
        /// </summary>
        internal static bool Initialized
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the <see cref="MessageManager"/> class.
        /// </summary>
        internal static void Init()
        {
            if (MessageManager.Initialized)
            {
                return;
            }
            
            MessageManager.SendQueue         = new ConcurrentQueue<PiranhaMessage>();
            MessageManager.ReceiveQueue      = new ConcurrentQueue<PiranhaMessage>();

            MessageManager.SendResetEvent    = new AutoResetEvent(false);
            MessageManager.ReceiveResetEvent = new AutoResetEvent(false);
            
            MessageManager.SendThread        = new Thread(MessageManager.SendMessage);
            MessageManager.ReceiveThread     = new Thread(MessageManager.ReceiveMessage);
            
            MessageManager.StartThreads();

            MessageManager.Initialized = true;
        }

        /// <summary>
        /// Starts the send/receive threads.
        /// </summary>
        internal static void StartThreads()
        {
            if (!MessageManager.Started)
            {
                MessageManager.Started = true;

                MessageManager.SendThread.Start();
                MessageManager.ReceiveThread.Start();
            }
        }

        /// <summary>
        /// Stops the send/receive threads.
        /// </summary>
        internal static void StopThreads()
        {
            if (MessageManager.Started)
            {
                MessageManager.Started = false;

                MessageManager.SendResetEvent.Close();
                MessageManager.ReceiveResetEvent.Close();
            }
        }

        /// <summary>
        /// Dequeues a message and then processes it.
        /// </summary>
        private static void ReceiveMessage()
        {
            while (MessageManager.Started)
            {
                MessageManager.ReceiveResetEvent.WaitOne();

                while (MessageManager.ReceiveQueue.TryDequeue(out PiranhaMessage message))
                {
                    Debugger.Info($"Packet {message.Type.ToString().Pad(35)} received from {message.Connection.EndPoint}.");

                    try
                    {
                        message.Decode();
                        message.Handle();

                        message.Connection.Avatar.Save();
                    }
                    catch (Exception exception)
                    {
                        Debugger.Error($"A {exception.GetType().Name} occured while handling the following message : ID {(int)message.Type}, Length {message.Length}, Version {message.Version}.");
                        Debugger.Error($"{exception.Message} [{message.Connection.Avatar.HighID}-{message.Connection.Avatar.LowID}]" + Environment.NewLine + exception.StackTrace);
                    }
                }
            }
        }

        /// <summary>
        /// Dequeues a message and sends it.
        /// </summary>
        private static void SendMessage()
        {
            while (MessageManager.Started)
            {
                MessageManager.SendResetEvent.WaitOne();

                while (MessageManager.SendQueue.TryDequeue(out PiranhaMessage message))
                {
                    if (message.IsServerToClientMessage)
                    {
                        message.Encode();
                        message.Encrypt();

                        ServerConnection.Send(message);

                        Debugger.Info($"Packet {message.Type.ToString().Pad(35)}    sent to    {message.Connection.EndPoint}.");

                        message.Handle();

                        message.Connection.Avatar.Save();
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="PiranhaMessage"/> is a client or server message and queues it accordingly.
        /// </summary>
        internal static void Enqueue(PiranhaMessage message)
        {
            if (message.IsClientToServerMessage)
            {
                MessageManager.ReceiveQueue.Enqueue(message);
                MessageManager.ReceiveResetEvent.Set();
            }
            else
            {
                MessageManager.SendQueue.Enqueue(message);
                MessageManager.SendResetEvent.Set();
            }
        }
    }
}
