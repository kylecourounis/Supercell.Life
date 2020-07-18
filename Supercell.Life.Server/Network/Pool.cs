namespace Supercell.Life.Server.Network
{
    using System.Collections.Generic;
    using System.Net.Sockets;

    internal class Pool
    {
        private readonly Stack<SocketAsyncEventArgs> Stack;

        private readonly object Gate = new object();
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Pool"/> class.
        /// </summary>
        internal Pool(int capacity)
        {
            this.Stack = new Stack<SocketAsyncEventArgs>(capacity);
        }

        /// <summary>
        /// Dequeues this instance.
        /// </summary>
        internal SocketAsyncEventArgs Dequeue()
        {
            lock (this.Gate)
            {
                return (this.Stack.Count > 0) ? this.Stack.Pop() : null;
            }
        }

        /// <summary>
        /// Enqueues the specified item.
        /// </summary>
        internal void Enqueue(SocketAsyncEventArgs asyncEvent)
        {
            asyncEvent.AcceptSocket     = null;
            asyncEvent.RemoteEndPoint   = null;

            lock (this.Gate)
            {
                this.Stack.Push(asyncEvent);
            }
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        internal void Dispose()
        {
            lock (this.Gate)
            {
                this.Stack.Clear();
            }
        }
    }
}