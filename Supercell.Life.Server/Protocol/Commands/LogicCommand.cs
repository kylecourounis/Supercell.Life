namespace Supercell.Life.Server.Protocol.Commands
{
    using System.Linq;
    using System.Reflection;

    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Helpers;
    using Supercell.Life.Titan.Logic.Math;
    
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class LogicCommand
    {
        private int Identifier;

        internal int ExecuteSubTick;

        internal LogicLong ExecutorID;

        internal int Subtick;

        /// <summary>
        /// Gets or sets the type of this <see cref="LogicCommand"/>
        /// </summary>
        internal Command Type
        {
            get => (Command)this.Identifier;
            set => this.Identifier = (int)value;
        }

        /// <summary>
        /// Gets the Connection.
        /// </summary>
        internal Connection Connection
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the stream.
        /// </summary>
        internal ByteStream Stream
        {
            get;
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicCommand"/> class.
        /// </summary>
        internal LogicCommand(Connection connection)
        {
            this.Connection = connection;
            this.Stream     = new ByteStream();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicCommand"/> class.
        /// </summary>
        internal LogicCommand(Connection connection, ByteStream stream)
        {
            this.Connection = connection;
            this.Stream     = stream;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal virtual void Decode()
        {
            // Decode.
        }


        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal virtual void Encode()
        {
            // Encode.
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        internal virtual void Execute()
        {
            // Process.
        }

        /// <summary>
        /// Reads the header.
        /// </summary>
        internal void ReadHeader()
        {
            this.ExecuteSubTick = this.Stream.ReadInt();
            this.ExecutorID     = this.Stream.ReadLogicLong();
        }

        /// <summary>
        /// Writes the header.
        /// </summary>
        internal void WriteHeader()
        {
            this.Stream.WriteInt(this.ExecuteSubTick);
            this.Stream.WriteLogicLong(this.ExecutorID);
        }

        /// <summary>
        /// Shows the buffer of this <see cref="LogicCommand"/>.
        /// </summary>
        internal void ShowBuffer()
        {
            Debugger.Debug(this.Stream.ToHexa());
        }
        
        /// <summary>
        /// Shows the values of this <see cref="LogicCommand"/>.
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