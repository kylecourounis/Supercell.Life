namespace Supercell.Life.Titan.Logic.Json
{
    using System.Text;

    public class LogicJSONArray : LogicJSONNode
    {
        private readonly LogicArrayList<LogicJSONNode> Items;

        /// <summary>
        /// Gets the node type.
        /// </summary>
        public override LogicJSONNode.NodeType Type
        {
            get
            {
                return LogicJSONNode.NodeType.Array;
            }
        }

        /// <summary>
        /// Gets the size of the <see cref="LogicJSONArray"/>.
        /// </summary>
        public int Size
        {
            get
            {
                return this.Items.Size;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicJSONArray"/> class.
        /// </summary>
        public LogicJSONArray(int capacity = 25)
        {
            this.Items = new LogicArrayList<LogicJSONNode>(capacity);
        }

        /// <summary>
        /// Gets the <see cref="LogicJSONNode"/> at the specified index.
        /// </summary>
        public LogicJSONNode Get(int idx)
        {
            return this.Items[idx];
        }

        /// <summary>
        /// Adds the specified <see cref="LogicJSONNode"/>.
        /// </summary>
        public void Add(LogicJSONNode item)
        {
            this.Items.Add(item);
        }

        /// <summary>
        /// Gets the <see cref="LogicJSONArray"/> at the specified index.
        /// </summary>
        public LogicJSONArray GetJsonArray(int idx)
        {
            LogicJSONNode node = this.Items[idx];

            if (node.Type != LogicJSONNode.NodeType.Array)
            {
                Debugger.Warning($"Wrong type {node.Type}, Index {idx}");
                return null;
            }

            return (LogicJSONArray)node;
        }

        /// <summary>
        /// Gets the <see cref="LogicJSONBoolean"/> at the specified index.
        /// </summary>
        public LogicJSONBoolean GetJsonBoolean(int idx)
        {
            LogicJSONNode node = this.Items[idx];

            if (node.Type != LogicJSONNode.NodeType.Boolean)
            {
                Debugger.Warning($"Wrong type {node.Type}, Index {idx}");
                return null;
            }

            return (LogicJSONBoolean)node;
        }

        /// <summary>
        /// Gets the <see cref="LogicJSONNumber"/> at the specified index.
        /// </summary>
        public LogicJSONNumber GetJsonNumber(int index)
        {
            LogicJSONNode node = this.Items[index];

            if (node.Type != LogicJSONNode.NodeType.Number)
            {
                Debugger.Warning($"Wrong type {node.Type}, Index {index}");
                return null;
            }

            return (LogicJSONNumber)node;
        }

        /// <summary>
        /// Gets the <see cref="LogicJSONObject"/> at the specified index.
        /// </summary>
        public LogicJSONObject GetJsonObject(int idx)
        {
            LogicJSONNode node = this.Items[idx];

            if (node.Type != LogicJSONNode.NodeType.Object)
            {
                Debugger.Warning($"Wrong type {node.Type} Index {idx}");
                return null;
            }

            return (LogicJSONObject)node;
        }

        /// <summary>
        /// Gets the <see cref="LogicJSONString"/> at the specified index.
        /// </summary>
        public LogicJSONString GetJsonString(int idx)
        {
            LogicJSONNode node = this.Items[idx];

            if (node.Type != LogicJSONNode.NodeType.String)
            {
                Debugger.Warning($"LogicJSONObject::getJSONString wrong type {node.Type}, Index {idx}");
                return null;
            }

            return (LogicJSONString)node;
        }

        /// <summary>
        /// Removes a <see cref="LogicJSONNode"/> from the array at the specified index.
        /// </summary>
        public void RemoveAt(int idx)
        {
            this.Items.RemoveAt(idx);
        }

        /// <summary>
        /// Converts this instance to JSON.
        /// </summary>
        public override void WriteToString(StringBuilder builder)
        {
            builder.Append('[');

            for (int i = 0; i < this.Items.Size; i++)
            {
                if (i > 0)
                {
                    builder.Append(',');
                }

                this.Items[i].WriteToString(builder);
            }

            builder.Append(']');
        }
    }
}