namespace Supercell.Life.Titan.Logic.Json
{
    using System.Text;

    public abstract class LogicJSONNode
    {
        /// <summary>
        /// Gets the node type.
        /// </summary>
        public abstract LogicJSONNode.NodeType Type
        {
            get;
        }

        /// <summary>
        /// Converts this instance to JSON.
        /// </summary>
        public abstract void WriteToString(StringBuilder builder);

        public enum NodeType
        {
            Array   = 1,
            Object  = 2,
            Number  = 3,
            String  = 4,
            Boolean = 5,
            Null    = 6
        } 
    }
}
