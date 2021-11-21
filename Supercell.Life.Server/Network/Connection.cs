namespace Supercell.Life.Server.Network
{
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Sockets;
    using System.Reflection;

    using Supercell.Life.Titan.Helpers;
    using Supercell.Life.Titan.Logic.Enums;

    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Collections;
    using Supercell.Life.Server.Logic.Game.Objects;
    using Supercell.Life.Server.Protocol;
    using Supercell.Life.Server.Protocol.Messages;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class Connection
    {
        internal readonly Socket Socket;
        internal Token Token;

        internal readonly Messaging Messaging;
        
        internal LogicGameMode GameMode;

        internal State State = State.Disconnected;

        // LogicDefines
        internal string MACAddress;
        internal string DeviceModel;
        // ------------

        /// <summary>
        /// Initializes a new instance of the <see cref="Connection"/> class.
        /// </summary>
        internal Connection(Socket socket)
        {
            this.Socket    = socket;
            this.Messaging = new Messaging(this);
            this.GameMode  = new LogicGameMode(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Connection"/> class.
        /// </summary>
        internal Connection(Socket socket, Token token) : this(socket)
        {
            this.Token = token;
        }

        /// <summary>
        /// Gets the end point of this connection.
        /// </summary>
        internal IPEndPoint EndPoint
        {
            get
            {
                return (IPEndPoint)this.Socket.RemoteEndPoint;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="Connection"/> is connected.
        /// </summary>
        internal bool IsConnected
        {
            get
            {
                if (this.Socket.Connected)
                {
                    try
                    {
                        if (!this.Socket.Poll(1000, SelectMode.SelectRead) || this.Socket.Available != 0)
                        {
                            return true;
                        }
                    }
                    catch (Exception)
                    {
                        return false;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Sends a chat message.
        /// </summary>
        internal void SendChatMessage(string message, bool system = true)
        {
            if (system)
            {
                new GlobalChatLineMessage(this)
                {
                    Chat = new GlobalChatLine
                    {
                        Message = message,
                        Sender  = this.GameMode.Avatar,
                        System  = true
                    }
                }.Send();
            }
            else
            {
                Connections.ForEach(connection => new GlobalChatLineMessage(connection)
                {
                    Chat = new GlobalChatLine
                    {
                        Message = message,
                        Sender  = this.GameMode.Avatar,
                        WhoSent = this.GameMode.Avatar.Identifier == connection.GameMode.Avatar.Identifier,
                        Regex   = true
                    }
                }.Send());
            }
        }

        /// <summary>
        /// Shows the values.
        /// </summary>
        internal void ShowValues()
        {
            foreach (FieldInfo field in this.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).Where(field => field != null))
            {
                Debugger.Info(field.Name.Pad() + " : " + (!string.IsNullOrEmpty(field.Name) ? (field.GetValue(this) != null ? field.GetValue(this).ToString() : "(null)") : "(null)").Pad(40));
            }
        }
    }
}