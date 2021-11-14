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
    using Supercell.Life.Titan.Logic.Json;

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
        /// Loads this <see cref="LogicCommand"/> from the specified <see cref="LogicJSONObject"/>.
        /// </summary>
        internal virtual void LoadCommandFromJSON(LogicJSONObject json)
        {
            LogicJSONObject b = json.GetJsonObject("b");

            if (b != null)
            {
                this.ExecuteSubTick = b.GetJsonNumber("t").GetIntValue();
                this.ExecutorID = new LogicLong(b.GetJsonNumber("id1").GetIntValue(), b.GetJsonNumber("id2").GetIntValue());
            }
            else
            {
                Debugger.Error($"Replay {this.GetType().Name} load failed! Base missing!");
            }
        }

        /// <summary>
        /// Saves the base of this <see cref="LogicCommand"/> to the returned <see cref="LogicJSONObject"/>.
        /// </summary>
        internal virtual LogicJSONObject SaveCommandToJSON()
        {
            LogicJSONObject b = new LogicJSONObject();

            b.Put("t", new LogicJSONNumber(this.ExecuteSubTick));
            b.Put("id1", new LogicJSONNumber(this.ExecutorID.High));
            b.Put("id2", new LogicJSONNumber(this.ExecutorID.Low));

            LogicJSONObject json = new LogicJSONObject();

            json.Put("b", b);

            return json;
        }

        /// <summary>
        /// Saves this <see cref="LogicCommand"/> to the returned <see cref="LogicJSONObject"/>.
        /// </summary>
        internal LogicJSONObject SaveToJSON()
        {
            LogicJSONObject json = new LogicJSONObject();

            json.Put("ct", new LogicJSONNumber((int)this.Type));
            json.Put("c", this.SaveCommandToJSON());

            return json;
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