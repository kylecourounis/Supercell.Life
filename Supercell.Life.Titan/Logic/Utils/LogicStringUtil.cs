namespace Supercell.Life.Titan.Logic.Utils
{
    using System.Text;

    public static class LogicStringUtil
    {
        /// <summary>
        /// Converts the specified string value to a byte array.
        /// </summary>
        public static byte[] GetBytes(string value)
        {
            return Encoding.UTF8.GetBytes(value);
        }

        /// <summary>
        /// Converts the specified string value to an integer.
        /// </summary>
        public static int ConvertToInt(string value)
        {
            if (value.Length < 1)
            {
                Debugger.Warning("Empty String");
                return -1;
            }

            return int.Parse(value);
        }

        /// <summary>
        /// Converts the specified integer value to a string.
        /// </summary>
        public static string IntToString(int value)
        {
            return value.ToString();
        }

        /// <summary>
        /// Determines whether the specified string is null, empty or whitespace.
        /// </summary>
        public static bool IsNullOrEmptyOrWhitespace(this string value)
        {
            return (string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value));
        }
    }
}
