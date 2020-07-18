namespace Supercell.Life.Titan.Logic.Json
{
    using System.Text;

    public class LogicJSONNull : LogicJSONNode
    {
        /// <summary>
        /// Gets the node type.
        /// </summary>
        public override LogicJSONNode.NodeType Type
        {
            get
            {
                return LogicJSONNode.NodeType.Null;
            }
        }

        /// <summary>
        /// Converts this instance to JSON.
        /// </summary>
        public override void WriteToString(StringBuilder builder)
        {
            builder.Append("null");
        }
    }
}