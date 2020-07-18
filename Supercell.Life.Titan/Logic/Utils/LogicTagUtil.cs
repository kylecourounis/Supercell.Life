namespace Supercell.Life.Titan.Logic.Utils
{
    using Supercell.Life.Titan.Logic.Math;

    public static class LogicTagUtil
    {
        /// <summary>
        /// Converts an identifier (player, clan...) into a hashtag.
        /// </summary>
        public static string ToTag(LogicLong identifier)
        {
            var tag = string.Empty;

            var high = identifier >> 32;

            if (high > 255)
            {
                return tag;
            }

            var low = identifier & 0xFFFFFFFF;

            identifier = (low << 8) + high;
            while (identifier != 0)
            {
                var index = identifier % 14;
                tag = "0289PYLQGRJCUV"[(int)index] + tag;

                identifier /= 14;
            }
            
            return $"#{tag}";
        }

        /// <summary>
        /// Convert a tag to a high and low identifier.
        /// </summary>
        public static void ToHighLow(string tag, out int high, out int low)
        {
            var array = tag.Replace("#", "").ToUpper().ToCharArray();

            long id = 0;

            foreach (var val in array)
            {
                id *= 14;
                id += "0289PYLQGRJCUV".IndexOf(val);
            }

            high = (int)id % 256;
            low  = (int)(id - high) >> 8;
        }

        /// <summary>
        /// Converts a tag to an identifier.
        /// </summary>
        public static LogicLong ToLogicLong(string tag)
        {
            var array = tag.Replace("#", "").ToUpper().ToCharArray();

            long id = 0;

            foreach (var val in array)
            {
                id *= 14;
                id += "0289PYLQGRJCUV".IndexOf(val);
            }

            var high = (int)id % 256;
            var low  = (int)(id - high) >> 8;

            return new LogicLong(high, low);
        }
    }
}