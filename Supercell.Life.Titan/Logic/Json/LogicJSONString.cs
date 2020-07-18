namespace Supercell.Life.Titan.Logic.Json
{
    using System.Text;

    public class LogicJSONString : LogicJSONNode
    {
        private string Value;

        public override LogicJSONNode.NodeType Type
        {
            get
            {
                return LogicJSONNode.NodeType.String;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicJSONString"/> class.
        /// </summary>
        public LogicJSONString(string value = "")
        {
            this.Value = value;
        }

        /// <summary>
        /// Gets the string value.
        /// </summary>
        public string GetStringValue()
        {
            return this.Value;
        }

        /// <summary>
        /// Sets the string value.
        /// </summary>
        public void SetStringValue(string value)
        {
            this.Value = value;
        }

        /// <summary>
        /// Converts this instance to JSON.
        /// </summary>
        public override void WriteToString(StringBuilder builder)
        {
            LogicJSONParser.WriteString(this.Value, builder);
        }
    }
}