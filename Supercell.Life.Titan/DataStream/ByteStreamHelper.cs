namespace Supercell.Life.Titan.DataStream
{
    using System;
    using System.Linq;

    public static class ByteStreamHelper
    {
        /// <summary>
        /// Converts the specified buffer to a short.
        /// </summary>
        internal static short ToInt16(this byte[] buffer)
        {
            return BitConverter.ToInt16(buffer.Reverse().ToArray(), 0);
        }

        /// <summary>
        /// Converts the specified buffer to an int.
        /// </summary>
        internal static int ToInt24(this byte[] buffer)
        {
            byte[] int24 = new byte[4];

            int24[0] = 0;
            int24[1] = buffer[0];
            int24[2] = buffer[1];
            int24[3] = buffer[2];

            return int24.ToInt32();
        }

        /// <summary>
        /// Converts the specified buffer to an int.
        /// </summary>
        internal static int ToInt32Endian(this byte[] buffer)
        {
            return BitConverter.ToInt32(buffer, 0);
        }

        /// <summary>
        /// Converts the specified buffer to an int.
        /// </summary>
        internal static int ToInt32(this byte[] buffer)
        {
            return BitConverter.ToInt32(buffer.Reverse().ToArray(), 0);
        }

        /// <summary>
        /// Converts the specified buffer to a long.
        /// </summary>
        internal static long ToInt64(this byte[] buffer)
        {
            return BitConverter.ToInt64(buffer.Reverse().ToArray(), 0);
        }

        /// <summary>
        /// Converts a hexadecimal string to a byte array.
        /// </summary>
        public static byte[] HexaToBytes(this string value)
        {
            string hexa = value.Replace("-", string.Empty).Replace(" ", string.Empty).Replace("\t", string.Empty);
            return Enumerable.Range(0, hexa.Length / 2).Select(x => Convert.ToByte(hexa.Substring(x * 2, 2), 16)).ToArray();
        }
    }
}
