namespace Supercell.Life.Titan.Logic.Json
{
    using System.Text;

    public class LogicJSONNumber : LogicJSONNode
    {
        private int Value;

        /// <summary>
        /// Gets the node type.
        /// </summary>
        public override LogicJSONNode.NodeType Type
        {
            get
            {
                return LogicJSONNode.NodeType.Number;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicJSONNumber"/> class.
        /// </summary>
        public LogicJSONNumber(int value = 0)
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets the int value.
        /// </summary>
        public int GetIntValue()
        {
            return this.Value;
        }

        /// <summary>
        /// Sets the int value.
        /// </summary>
        public void SetIntValue(int value)
        {
            this.Value = value;
        }
        
        /// <summary>
        /// Converts this instance to JSON.
        /// </summary>
        public override void WriteToString(StringBuilder builder)
        {
            builder.Append(this.Value);
        }
    }
}