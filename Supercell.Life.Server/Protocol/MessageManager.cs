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
        private readonly Thread SendThread;
        private readonly Thread ReceiveThread;

        private readonly ConcurrentQueue<PiranhaMessage> SendQueue;
        private readonly ConcurrentQueue<PiranhaMessage> ReceiveQueue;

        private readonly AutoResetEvent SendResetEvent;
        private readonly AutoResetEvent ReceiveResetEvent;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageManager"/> class.
        /// </summary>
        internal MessageManager()
        {
            this.SendQueue         = new ConcurrentQueue<PiranhaMessage>();
            this.ReceiveQueue      = new ConcurrentQueue<PiranhaMessage>();

            this.SendResetEvent    = new AutoResetEvent(false);
            this.ReceiveResetEvent = new AutoResetEvent(false);

            this.SendThread        = new Thread(this.SendMessage);
            this.ReceiveThread     = new Thread(this.ReceiveMessage);
        }

        /// <summary>
        /// Starts the send/receive threads.
        /// </summary>
        internal void StartThreads()
        {
            this.SendThread.Start();
            this.ReceiveThread.Start();
        }

        /// <summary>
        /// Stops the send/receive threads.
        /// </summary>
        internal void StopThreads()
        {
            this.SendResetEvent.Close();
            this.ReceiveResetEvent.Close();
        }

        /// <summary>
        /// Dequeues a message and then processes it.
        /// </summary>
        private void ReceiveMessage()
        {
            while (true)
            {
                this.ReceiveResetEvent.WaitOne();

                while (this.ReceiveQueue.TryDequeue(out PiranhaMessage message))
                {
                    Debugger.Info($"Packet {message.Type.ToString().Pad(35)} received from {message.Connection.EndPoint}.");

                    try
                    {
                        message.Decode();
                        message.Handle();

                        message.Connection.GameMode.Avatar.Save();
                    }
                    catch (Exception exception)
                    {
                        Debugger.Error($"A {exception.GetType().Name} occured while handling the following message : ID {(int)message.Type}, Length {message.Length}, Version {message.Version}.");
                        Debugger.Error($"{exception.Message} [{message.Connection.GameMode.Avatar.HighID}-{message.Connection.GameMode.Avatar.LowID}]" + Environment.NewLine + exception.StackTrace);
                    }
                }
            }
        }

        /// <summary>
        /// Dequeues a message and sends it.
        /// </summary>
        private void SendMessage()
        {
            while (true)
            {
                this.SendResetEvent.WaitOne();

                while (this.SendQueue.TryDequeue(out PiranhaMessage message))
                {
                    if (message.IsServerToClientMessage)
                    {
                        message.Encode();
                        message.Encrypt();

                        ServerConnection.Send(message);

                        Debugger.Info($"Packet {message.Type.ToString().Pad(35)}    sent to    {message.Connection.EndPoint}.");

                        message.Handle();

                        message.Connection.GameMode.Avatar.Save();
                    }
                }
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="PiranhaMessage"/> is a client or server message and queues it accordingly.
        /// </summary>
        internal void Enqueue(PiranhaMessage message)
        {
            if (message.IsClientToServerMessage)
            {
                this.ReceiveQueue.Enqueue(message);
                this.ReceiveResetEvent.Set();
            }
            else
            {
                this.SendQueue.Enqueue(message);
                this.SendResetEvent.Set();
            }
        }
    }
}
