namespace Supercell.Life.Server.Protocol.Commands
{
    using System.Linq;
    using System.Reflection;

    using Supercell.Life.Server.Logic;
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
        /// Initializes a new instance of the <see cref="LogicCommand"/> class.
        /// </summary>
        internal LogicCommand(Connection connection)
        {
            this.Connection = connection;
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        internal virtual void Decode(ByteStream stream)
        {
            this.ExecuteSubTick = stream.ReadInt();
            this.ExecutorID     = stream.ReadLogicLong();
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal virtual void Encode(ChecksumEncoder encoder)
        {
            encoder.WriteInt(this.ExecuteSubTick);
            encoder.WriteLogicLong(this.ExecutorID);
        }

        /// <summary>
        /// Executes this instance.
        /// </summary>
        internal virtual void Execute(LogicGameMode gamemode)
        {
            // Execute.
        }
        
        /// <summary>
        /// Shows the buffer of this <see cref="LogicCommand"/>.
        /// </summary>
        internal void ShowBuffer(ByteStream stream)
        {
            Debugger.Debug(stream.ToHexa());
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