namespace Supercell.Life.Titan.Logic.Json
{
    using System.Text;

    public class LogicJSONBoolean : LogicJSONNode
    {
        private readonly bool Value;

        /// <summary>
        /// Gets the node type.
        /// </summary>
        public override LogicJSONNode.NodeType Type
        {
            get
            {
                return LogicJSONNode.NodeType.Boolean;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicJSONBoolean"/> class.
        /// </summary>
        public LogicJSONBoolean(bool value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Determines whether this <see cref="LogicJSONBoolean"/>'s value is true.
        /// </summary>
        public bool IsTrue()
        {
            return this.Value;
        }
        
        /// <summary>
        /// Converts this instance to JSON.
        /// </summary>
        public override void WriteToString(StringBuilder builder)
        {
            builder.Append(this.Value.ToString().ToLower());
        }
    }
}