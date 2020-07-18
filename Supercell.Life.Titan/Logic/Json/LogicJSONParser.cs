namespace Supercell.Life.Titan.Logic.Json
{
    using System.Globalization;
    using System.Text;

    public class LogicJSONParser
    {
        /// <summary>
        /// Creates the json string.
        /// </summary>
        public static string CreateJsonString(LogicJSONNode root, int ensureCapacity = 25)
        {
            StringBuilder builder = new StringBuilder(ensureCapacity);
            root.WriteToString(builder);
            return builder.ToString();
        }

        /// <summary>
        /// Writes the specified value to the <see cref="StringBuilder"/>.
        /// </summary>
        public static void WriteString(string value, StringBuilder builder)
        {
            builder.Append('"');

            if (!string.IsNullOrEmpty(value))
            {
                foreach (var character in value)
                {
                    if (character <= '\r' && character >= '\b')
                    {
                        switch (character)
                        {
                            case '\b':
                            {
                                builder.Append("\\b");
                                break;
                            }
                            case '\t':
                            {
                                builder.Append("\\t");
                                break;
                            }
                            case '\n':
                            {
                                builder.Append("\\n");
                                break;
                            }
                            case '\f':
                            {
                                builder.Append("\\f");
                                break;
                            }
                            case '\r':
                            {
                                builder.Append("\\r");
                                break;
                            }
                            default:
                            {
                                builder.Append(character);
                                break;
                            }
                        }
                    }
                    else
                    {
                        switch (character)
                        {
                            case '"':
                            {
                                builder.Append("\\\"");
                                break;
                            }
                            case '/':
                            {
                                builder.Append("\\/");
                                break;
                            }
                            case '\\':
                            {
                                builder.Append("\\\\");
                                break;
                            }
                            default:
                            {
                                builder.Append(character);
                                break;
                            }
                        }
                    }
                }
            }

            builder.Append('"');
        }

        /// <summary>
        /// Writes an error message.
        /// </summary>
        public static void ParseError(string error)
        {
            Debugger.Error($"Error parsing JSON: {error}");
        }

        /// <summary>
        /// Parses the <see cref="LogicJSONNode"/>.
        /// </summary>
        public static LogicJSONNode Parse(string json)
        {
            return LogicJSONParser.ParseValue(new CharStream(json));
        }

        /// <summary>
        /// Parses the <see cref="LogicJSONNode"/>.
        /// </summary>
        private static LogicJSONNode ParseValue(CharStream stream)
        {
            stream.SkipWhitespace();

            char character = stream.NextChar();
            LogicJSONNode node = null;

            switch (character)
            {
                case '{':
                    node = LogicJSONParser.ParseObject(stream);
                    break;
                case '[':
                    node = LogicJSONParser.ParseArray(stream);
                    break;
                case 'n':
                    node = LogicJSONParser.ParseNull(stream);
                    break;
                case 'f':
                    node = LogicJSONParser.ParseBoolean(stream);
                    break;
                case 't':
                    node = LogicJSONParser.ParseBoolean(stream);
                    break;
                case '"':
                    node = LogicJSONParser.ParseString(stream);
                    break;
                case '-':
                    node = LogicJSONParser.ParseNumber(stream);
                    break;
                default:
                    if (character >= '0' && character <= '9')
                    {
                        node = LogicJSONParser.ParseNumber(stream);
                    }
                    else
                    {
                        LogicJSONParser.ParseError("Not of any recognized value: " + character);
                    }

                    break;
            }

            return node;
        }

        /// <summary>
        /// Parses the <see cref="LogicJSONArray"/>.
        /// </summary>
        public static LogicJSONArray ParseArray(string json)
        {
            return LogicJSONParser.ParseArray(new CharStream(json));
        }

        /// <summary>
        /// Parses the <see cref="LogicJSONArray"/>.
        /// </summary>
        private static LogicJSONArray ParseArray(CharStream stream)
        {
            stream.SkipWhitespace();

            if (stream.Read() != '[')
            {
                LogicJSONParser.ParseError("Not an array");
                return null;
            }

            LogicJSONArray jsonArray = new LogicJSONArray();

            stream.SkipWhitespace();

            char nextChar = stream.NextChar();

            if (nextChar != '\0')
            {
                if (nextChar == ']')
                {
                    stream.Read();
                    return jsonArray;
                }

                while (true)
                {
                    LogicJSONNode node = LogicJSONParser.ParseValue(stream);

                    if (node != null)
                    {
                        jsonArray.Add(node);
                        stream.SkipWhitespace();

                        nextChar = stream.Read();

                        if (nextChar != ',')
                        {
                            if (nextChar == ']')
                            {
                                return jsonArray;
                            }

                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            LogicJSONParser.ParseError("Not an array");

            return null;
        }

        /// <summary>
        /// Parses the <see cref="LogicJSONObject"/> from the specified string.
        /// </summary>
        public static LogicJSONObject ParseObject(string json)
        {
            return LogicJSONParser.ParseObject(new CharStream(json));
        }

        /// <summary>
        /// Parses the <see cref="LogicJSONObject"/>.
        /// </summary>
        private static LogicJSONObject ParseObject(CharStream stream)
        {
            stream.SkipWhitespace();

            if (stream.Read() != '{')
            {
                LogicJSONParser.ParseError("Not an object");
                return null;
            }

            LogicJSONObject jsonObject = new LogicJSONObject();

            stream.SkipWhitespace();

            char nextChar = stream.NextChar();

            if (nextChar != '\0')
            {
                if (nextChar == '}')
                {
                    stream.Read();
                    return jsonObject;
                }

                while (true)
                {
                    LogicJSONString key = LogicJSONParser.ParseString(stream);

                    if (key != null)
                    {
                        stream.SkipWhitespace();

                        nextChar = stream.Read();

                        if (nextChar != ':')
                        {
                            break;
                        }

                        LogicJSONNode node = LogicJSONParser.ParseValue(stream);

                        if (node != null)
                        {
                            jsonObject.Put(key.GetStringValue(), node);
                            stream.SkipWhitespace();

                            nextChar = stream.Read();

                            if (nextChar != ',')
                            {
                                if (nextChar == '}')
                                {
                                    return jsonObject;
                                }

                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }

            LogicJSONParser.ParseError("Not an object");
            return null;
        }

        /// <summary>
        /// Parses the <see cref="LogicJSONString"/>.
        /// </summary>
        private static LogicJSONString ParseString(CharStream stream)
        {
            stream.SkipWhitespace();

            if (stream.Read() != '"')
            {
                LogicJSONParser.ParseError("Not a string");
                return null;
            }

            StringBuilder builder = new StringBuilder();

            while (true)
            {
                char nextChar = stream.Read();

                if (nextChar != '\0')
                {
                    if (nextChar != '"')
                    {
                        if (nextChar == '\\')
                        {
                            nextChar = stream.Read();

                            switch (nextChar)
                            {
                                case 'n':
                                { 
                                    nextChar = '\n';
                                    break;
                                }
                                case 'r':
                                {
                                    nextChar = '\r';
                                    break;
                                }
                                case 't':
                                {
                                    nextChar = '\t';
                                    break;
                                }
                                case 'u':
                                {
                                    nextChar = (char)int.Parse(stream.Read(4), NumberStyles.HexNumber);
                                    break;
                                }
                                case 'b':
                                {
                                    nextChar = '\b';
                                    break;
                                }
                                case 'f':
                                {
                                    nextChar = '\f';
                                    break;
                                }
                                case '\0':
                                {
                                    LogicJSONParser.ParseError("Not a string");
                                    return null;
                                }
                            }
                        }

                        builder.Append(nextChar);
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    LogicJSONParser.ParseError("Not a string");
                    return null;
                }
            }

            return new LogicJSONString(builder.ToString());
        }

        /// <summary>
        /// Parses the <see cref="LogicJSONBoolean"/>.
        /// </summary>
        private static LogicJSONBoolean ParseBoolean(CharStream stream)
        {
            stream.SkipWhitespace();

            char nextChar = stream.Read();

            switch (nextChar) 
            {
                case 'f' when stream.Read() == 'a' && stream.Read() == 'l' && stream.Read() == 's' && stream.Read() == 'e':
                {
                    return new LogicJSONBoolean(false);
                }
                case 't' when stream.Read() == 'r' && stream.Read() == 'u' && stream.Read() == 'e':
                {
                    return new LogicJSONBoolean(true);
                }
                default:
                {
                    LogicJSONParser.ParseError("Not a boolean");
                    return null;
                }
            }
        }

        /// <summary>
        /// Parses the <see cref="LogicJSONNull"/>.
        /// </summary>
        private static LogicJSONNull ParseNull(CharStream stream)
        {
            stream.SkipWhitespace();

            char nextChar = stream.Read();

            if (nextChar == 'n')
            {
                if (stream.Read() == 'u' && stream.Read() == 'l' && stream.Read() == 'l')
                {
                    return new LogicJSONNull();
                }
            }

            LogicJSONParser.ParseError("Not a null");

            return null;
        }

        /// <summary>
        /// Parses the <see cref="LogicJSONNumber"/>.
        /// </summary>
        private static LogicJSONNumber ParseNumber(CharStream stream)
        {
            stream.SkipWhitespace();

            char nextChar = stream.NextChar();
            int multiplier = 1;

            if (nextChar == '-')
            {
                multiplier = -1;
                nextChar = stream.Read();
            }

            if (nextChar != ',')
            {
                int value = 0;

                while ((nextChar = stream.Read()) <= '9' && nextChar >= '0')
                {
                    value = nextChar - '0' + 10 * value;

                    if ((nextChar = stream.NextChar()) > '9' || nextChar < '0')
                    {
                        break;
                    }
                }

                if (nextChar == 'e' || nextChar == 'E' || nextChar == '.')
                {
                    LogicJSONParser.ParseError("JSON floats not supported");
                    return null;
                }

                return new LogicJSONNumber(value * multiplier);
            }

            LogicJSONParser.ParseError("Not a number");
            return null;
        }

        private class CharStream
        {
            private readonly string StringValue;
            private int Offset;

            /// <summary>
            /// Initializes a new instance of the <see cref="CharStream"/> class.
            /// </summary>
            public CharStream(string value)
            {
                this.StringValue = value;
            }

            /// <summary>
            /// Reads the char.
            /// </summary>
            public char Read()
            {
                if (this.Offset >= this.StringValue.Length)
                {
                    return '\0';
                }

                return this.StringValue[this.Offset++];
            }

            /// <summary>
            /// Returns a string read with the specified length.
            /// </summary>
            public string Read(int length)
            {
                if (this.Offset + length >= this.StringValue.Length)
                {
                    return null;
                }

                string value = this.StringValue.Substring(this.Offset, length);
                this.Offset += length;

                return value;
            }

            /// <summary>
            /// Advances the stream and returns the next character.
            /// </summary>
            public char NextChar()
            {
                if (this.Offset >= this.StringValue.Length)
                {
                    return '\0';
                }

                return this.StringValue[this.Offset];
            }

            /// <summary>
            /// Skips the whitespace.
            /// </summary>
            public void SkipWhitespace()
            {
                char character;

                do
                {
                    character = this.Read();
                } while (character <= ' ' && character != '\0');

                this.Offset--;
            }
        }
    }
}