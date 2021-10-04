namespace Supercell.Life.Server.Network
{
    using System;
    using System.Net.Sockets;

    using Supercell.Life.Titan.Core;
    
    internal class Token : IDisposable
    {
        internal Connection Connection;
        internal SocketAsyncEventArgs Args;

        internal byte[] Buffer;

        internal bool Aborting;
        internal bool Disposed;

        internal int Tries;

        /// <summary>
        /// Initializes a new instance of the <see cref="Token"/> class.
        /// </summary>
        internal Token(SocketAsyncEventArgs args, Connection connection)
        {
            this.Connection       = connection;
            this.Connection.Token = this;

            this.Args             = args;
            this.Args.UserToken   = this;

            this.Buffer           = new byte[Constants.BufferSize];
        }

        /// <summary>
        /// Sets the data.
        /// </summary>
        internal void SetData()
        {
            if (!this.Disposed)
            {
                this.Buffer = new byte[this.Args.BytesTransferred];
                Array.Copy(this.Args.Buffer, 0, this.Buffer, 0, this.Args.BytesTransferred);
            }

            this.Tries += 1;
        }

        /// <summary>
        /// Processes this instance.
        /// </summary>
        internal void Process()
        {
            if (this.Tries > 10)
            {
                ServerConnection.Disconnect(this.Args);
            }
            else
            {
                this.Tries = 0;
                this.Connection.Messaging.OnReceive(this.Buffer);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Buffer     = null;
            this.Connection = null;

            this.Tries      = 0;

            this.Disposed   = true;
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Token"/> class.
        /// </summary>
        ~Token()
        {
            if (!this.Aborting)
            {
                ServerConnection.Disconnect(this.Args);
            }

            if (!this.Disposed)
            {
                this.Dispose();
            }

            GC.SuppressFinalize(this);
        }
    }
}