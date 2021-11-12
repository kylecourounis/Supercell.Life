namespace Supercell.Life.Server.Protocol
{
    using System.Collections.Generic;
    using System.Linq;

    using Supercell.Life.Titan.Core;
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Library.Cryptography.RC4;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;
    using Supercell.Life.Server.Protocol.Messages;

    internal class Messaging
    {
        internal const int HeaderLength = 7;

        internal readonly Connection Connection;

        internal RC4Encrypter Crypto;

        /// <summary>
        /// Initializes a new instance of the <see cref="Messaging"/> class.
        /// </summary>
        internal Messaging(Connection connection)
        {
            this.Connection = connection;
        }

        /// <summary>
        /// Gets a value indicating whether <see cref="Messaging.Connection"/> is connected.
        /// </summary>
        internal bool IsConnected
        {
            get
            {
                return this.Connection.IsConnected;
            }
        }

        /// <summary>
        /// Sets the encrypters.
        /// </summary>
        internal void SetEncrypters()
        {
            this.Crypto = new RC4Encrypter(LogicLifeMessageFactory.RC4Key);
        }

        /// <summary>
        /// Processes the specified buffer.
        /// </summary>
        internal void OnReceive(byte[] buffer)
        {
            if (buffer != null && this.IsConnected)
            {
                if (buffer.Length >= Messaging.HeaderLength && buffer.Length <= Constants.BufferSize)
                {
                    Messaging.ReadHeader(buffer, out int id, out int length, out int version);

                    if ((id - 10000 < 10000) && (buffer.Length - Messaging.HeaderLength >= length))
                    {
                        using (ByteStream stream = new ByteStream(buffer.Skip(Messaging.HeaderLength).ToArray()))
                        {
                            PiranhaMessage message = LogicLifeMessageFactory.CreateMessageByType(id, this.Connection, stream);

                            if (message != null)
                            {
                                message.Type    = (Message)id;
                                message.Length  = length;
                                message.Version = (short)version;

                                message.Decrypt();

                                this.Connection.GameMode.MessageManager.Enqueue(message);
                            }

                            if (!this.Connection.Token.Aborting)
                            {
                                this.Connection.Token.Packet.RemoveRange(0, length + Messaging.HeaderLength);

                                if (buffer.Length - Messaging.HeaderLength - length >= Messaging.HeaderLength)
                                {
                                    this.OnReceive(stream.ReadBytes(buffer.Length - Messaging.HeaderLength - length));
                                }
                            }
                        }
                    }
                    else
                    {
                        Debugger.Error("The received buffer length is inferior the header length.");

                        this.Connection.Token.Packet.Clear();
                    }
                }
                else
                {
                    ServerConnection.Disconnect(this.Connection.Token.Args);
                }
            }
        }

        /// <summary>
        /// Reads the message header.
        /// </summary>
        private static void ReadHeader(IReadOnlyList<byte> buffer, out int id, out int length, out int version)
        {
            id      = buffer[0] << 8 | buffer[1];
            length  = buffer[2] << 16 | buffer[3] << 8 | buffer[4];
            version = buffer[5] << 8 | buffer[6];
        }

        /// <summary>
        /// Writes the message header.
        /// </summary>
        internal static void WriteHeader(PiranhaMessage message, ref byte[] buffer)
        {
            int id      = (int)message.Type;
            int length  = message.Length;
            int version = message.Version;

            // Identifier - Int16
            buffer[0] = (byte)(id >> 8);
            buffer[1] = (byte)(id);

            // Length - Int24
            buffer[2] = (byte)(length >> 16);
            buffer[3] = (byte)(length >> 8);
            buffer[4] = (byte)(length);

            // Version - Int16
            buffer[5] = (byte)(version >> 8);
            buffer[6] = (byte)(version);

            if (length >= 0x1000000)
            {
                Debugger.Error($"Trying to send too big message, type {id}");
            }
        }

        /// <summary>
        /// Sends the specified <see cref="PiranhaMessage"/>.
        /// </summary>
        internal void Send(PiranhaMessage message) => this.Connection.GameMode.MessageManager.Enqueue(message);
    }
}
