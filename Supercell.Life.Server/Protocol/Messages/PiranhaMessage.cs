namespace Supercell.Life.Server.Protocol.Messages
{
    using System;
    using System.IO;
    using System.Reflection;
    using System.Text;

    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Helpers;
    
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class PiranhaMessage
    {
        private short Identifier;
        internal int Length;
        internal short Version;

        internal int Offset;

        /// <summary>
        /// Gets or sets the type of this <see cref="PiranhaMessage"/>.
        /// </summary>
        internal Message Type
        {
            get => (Message)this.Identifier;
            set => this.Identifier = (short)value;
        }

        /// <summary>
        /// Gets the service node for this <see cref="PiranhaMessage"/>.
        /// </summary>
        internal virtual ServiceNode Node
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Gets the connection.
        /// </summary>
        internal Connection Connection
        {
            get;
        }

        /// <summary>
        /// Gets the stream.
        /// </summary>
        internal ByteStream Stream
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PiranhaMessage"/> class.
        /// </summary>
        internal PiranhaMessage(Connection connection)
        {
            this.Connection = connection;
            this.Stream     = new ByteStream();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PiranhaMessage"/> class.
        /// </summary>
        internal PiranhaMessage(Connection connection, ByteStream stream)
        {
            this.Connection = connection;
            this.Stream     = stream;
        }
        
        /// <summary>
        /// Gets a value indicating whether this <see cref="PiranhaMessage"/> is a client to server message.
        /// </summary>
        internal bool IsClientToServerMessage => this.Identifier - 10000 < 10000;

        /// <summary>
        /// Gets a value indicating whether this <see cref="PiranhaMessage"/> is a server to client message.
        /// </summary>
        internal bool IsServerToClientMessage => this.Identifier - 20000 > 0;

        /// <summary>
        /// Gets this <see cref="PiranhaMessage"/> as a byte array.
        /// </summary>
        internal byte[] ToArray()
        {
            using (ByteStream packet = new ByteStream())
            {
                packet.WriteShort(this.Identifier);
                packet.WriteInt24(this.Length);
                packet.WriteShort(this.Version);
                packet.Write(this.Stream.ToArray());

                return packet.ToArray();
            }
        }

        /// <summary>
        /// Decodes this <see cref="PiranhaMessage"/> using the <see cref="ByteStream"/>.
        /// </summary>
        internal virtual void Decode()
        {
            // Decode.
        }

        /// <summary>
        /// Encodes this <see cref="PiranhaMessage"/> using the <see cref="ByteStream"/>.
        /// </summary>
        internal virtual void Encode()
        {
            // Encode.
        }

        /// <summary>
        /// Handles this <see cref="PiranhaMessage"/>.
        /// </summary>
        internal virtual void Handle()
        {
            // Handle.
        }

        /// <summary>
        /// Encrypts this <see cref="PiranhaMessage"/> using the RC4 cryptography.
        /// </summary>
        internal void Encrypt()
        {
            byte[] encrypted = this.Connection.Messaging.Crypto.Encrypt(this.Stream.ToArray());

            this.Stream = new ByteStream(encrypted);
            this.Length = this.Stream.Length;
        }

        /// <summary>
        /// Decrypts this <see cref="PiranhaMessage"/> using the RC4 cryptography.
        /// </summary>
        internal void Decrypt()
        {
            byte[] decrypted = this.Connection.Messaging.Crypto.Decrypt(this.Stream.ReadBytes(this.Length));

            this.Stream = new ByteStream(decrypted);
            this.Length = this.Stream.Length;
        }

        /// <summary>
        /// Shows the buffer of this <see cref="PiranhaMessage"/>.
        /// </summary>
        internal void ShowBuffer()
        {
            Debugger.Debug(this.Stream.ToHexa());
        }

        /// <summary>
        /// Shows the values of this <see cref="PiranhaMessage"/>.
        /// </summary>
        internal void ShowValues()
        {
            foreach (FieldInfo field in this.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
            {
                Debugger.Info(field.Name.Pad() + " : " + (!string.IsNullOrEmpty(field.Name) ? (field.GetValue(this) != null ? field.GetValue(this).ToString() : "(null)") : "(null)").Pad(40));
            }
        }

        /// <summary>
        /// Dumps the contents of this <see cref="PiranhaMessage"/> to a file.
        /// </summary>
        internal void Dump()
        {
            FileInfo file = new FileInfo($"{Directory.GetCurrentDirectory()}/Logs/Dumps/{this.Type}-{this.Identifier}.log");
            file.Directory.CreateIfNotExists();

            StringBuilder builder = new StringBuilder();

            builder.AppendLine($"Type : {this.Type} ({this.Identifier})");
            builder.AppendLine($"Length : {this.Length}");
            builder.AppendLine($"Version : {this.Version}" + Environment.NewLine);

            builder.AppendLine($"CTS : {this.IsClientToServerMessage}");
            builder.AppendLine($"STC : {this.IsServerToClientMessage}" + Environment.NewLine);

            builder.AppendLine($"Payload : {this.Stream.ToHexa()}" + Environment.NewLine);

            string output = builder.ToString();

            file.AppendAllText(output);
        }
    }
}