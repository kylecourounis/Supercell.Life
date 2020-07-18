namespace Supercell.Life.Titan.Logic.Json
{
    using System.Text;

    public class LogicJSONObject : LogicJSONNode
    {
        private LogicArrayList<string> Keys;
        private LogicArrayList<LogicJSONNode> Items;

        /// <summary>
        /// Gets the node type.
        /// </summary>
        public override LogicJSONNode.NodeType Type
        {
            get
            {
                return LogicJSONNode.NodeType.Object;
            }
        }

        /// <summary>
        /// Gets the object count.
        /// </summary>
        public int ObjectCount
        {
            get
            {
                return this.Items.Size;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicJSONObject"/> class.
        /// </summary>
        public LogicJSONObject(int capacity = 25)
        {
            this.Keys = new LogicArrayList<string>(capacity);
            this.Items = new LogicArrayList<LogicJSONNode>(capacity);
        }

        /// <summary>
        /// Gets the <see cref="LogicJSONNode"/> using the specified key.
        /// </summary>
        public LogicJSONNode Get(string key)
        {
            int idx = this.Keys.IndexOf(key);
            return (idx == -1) ? null : this.Items[idx];
        }

        /// <summary>
        /// Gets the <see cref="LogicJSONArray"/> using the specified key.
        /// </summary>
        public LogicJSONArray GetJsonArray(string key)
        {
            int idx = this.Keys.IndexOf(key);

            if (idx == -1)
            {
                return null;
            }

            LogicJSONNode node = this.Items[idx];

            if (node.Type == LogicJSONNode.NodeType.Array)
            {
                return (LogicJSONArray)node;
            }

            Debugger.Warning($"type is {node.Type}, key {key}");

            return null;
        }

        /// <summary>
        /// Gets the <see cref="LogicJSONBoolean"/> using the specified key.
        /// </summary>
        public LogicJSONBoolean GetJsonBoolean(string key)
        {
            int idx = this.Keys.IndexOf(key);

            if (idx == -1)
            {
                return null;
            }

            LogicJSONNode node = this.Items[idx];

            if (node.Type == LogicJSONNode.NodeType.Boolean)
            {
                return (LogicJSONBoolean)node;
            }

            Debugger.Warning($"Type is {node.Type}, key {key}");

            return null;
        }

        /// <summary>
        /// Gets the <see cref="LogicJSONNumber"/> using the specified key.
        /// </summary>
        public LogicJSONNumber GetJsonNumber(string key)
        {
            int idx = this.Keys.IndexOf(key);

            if (idx == -1)
            {
                return null;
            }

            LogicJSONNode node = this.Items[idx];

            if (node.Type == LogicJSONNode.NodeType.Number)
            {
                return (LogicJSONNumber)node;
            }

            Debugger.Warning($"Type is {node.Type}, key {key}");

            return null;
        }

        /// <summary>
        /// Gets the <see cref="LogicJSONObject"/> using the specified key.
        /// </summary>
        public LogicJSONObject GetJsonObject(string key)
        {
            int idx = this.Keys.IndexOf(key);

            if (idx == -1)
            {
                return null;
            }

            LogicJSONNode node = this.Items[idx];

            if (node.Type == LogicJSONNode.NodeType.Object)
            {
                return (LogicJSONObject)node;
            }

            Debugger.Warning($"Type is {node.Type}, key {key}");

            return null;
        }

        /// <summary>
        /// Gets the <see cref="LogicJSONString"/> using the specified key.
        /// </summary>
        public LogicJSONString GetJsonString(string key)
        {
            int idx = this.Keys.IndexOf(key);

            if (idx == -1)
            {
                return null;
            }

            LogicJSONNode node = this.Items[idx];

            if (node.Type == LogicJSONNode.NodeType.String)
            {
                return (LogicJSONString)node;
            }

            Debugger.Warning($"Type is {node.Type}, key {key}");

            return null;
        }

        /// <summary>
        /// Appends the specified key and <see cref="LogicJSONNode"/> to this <see cref="LogicJSONObject"/>.
        /// </summary>
        public void Put(string key, LogicJSONNode item)
        {
            int keyIdx = this.Keys.IndexOf(key);

            if (keyIdx != -1)
            {
                Debugger.Error($"Already contains key {key}");
            }
            else
            {
                int idx = this.Items.IndexOf(item);

                if (idx != -1)
                {
                    Debugger.Error($"Already contains the given JSONNode pointer. Key {key}");
                }
                else
                {
                    this.Items.Add(item);
                    this.Keys.Add(key);
                }
            }
        }

        /// <summary>
        /// Removes the <see cref="LogicJSONNode"/> with the specified key.
        /// </summary>
        public void Remove(string key)
        {
            int keyIdx = this.Keys.IndexOf(key);

            if (keyIdx != -1)
            {
                this.Keys.RemoveAt(keyIdx);
                this.Items.RemoveAt(keyIdx);
            }
        }

        /// <summary>
        /// Creates the JSON string.
        /// </summary>
        public override void WriteToString(StringBuilder builder)
        {
            builder.Append('{');

            for (int i = 0; i < this.Items.Size; i++)
            {
                if (i > 0)
                {
                    builder.Append(',');
                }

                LogicJSONParser.WriteString(this.Keys[i], builder);
                builder.Append(':');
                this.Items[i].WriteToString(builder);
            }

            builder.Append('}');
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            this.WriteToString(builder);
            return builder.ToString();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="LogicJSONObject"/> class.
        /// </summary>
        ~LogicJSONObject()
        {
            if (this.Keys != null)
            {
                this.Items.Clear();
                this.Keys = null;
            }

            if (this.Items != null)
            {
                this.Items.Clear();
                this.Items = null;
            }
        }
    }
}