namespace Supercell.Life.Titan.Helpers
{
    using System;

    public class Timestamp
    {
        public static readonly DateTime Unix = new DateTime(1970, 1, 1);

        /// <summary>
        /// Gets the current timestamp as an instance of <see cref="TimeSpan"/>.
        /// </summary>
        private static TimeSpan CurrentTime
        {
            get
            {
                return DateTime.UtcNow.Subtract(Timestamp.Unix);
            }
        }

        /// <summary>
        /// Gets the current timestamp in seconds.
        /// </summary>
        public static int Seconds
        {
            get
            {
                return (int)Timestamp.CurrentTime.TotalSeconds;
            }
        }

        /// <summary>
        /// Gets the current timestamp in milliseconds.
        /// </summary>
        public static long Milliseconds
        {
            get
            {
                return (long)Timestamp.CurrentTime.TotalMilliseconds;
            }
        }
    }
}
