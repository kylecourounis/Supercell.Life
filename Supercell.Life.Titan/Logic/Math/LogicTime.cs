namespace Supercell.Life.Titan.Logic.Math
{
    public class LogicTime
    {
        public int ClientSubTick;

        /// <summary>
        /// Gets the tick.
        /// </summary>
        public int Tick
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the full tick.
        /// </summary>
        public int FullTick
        {
            get;
            private set;
        }
        
        /// <summary>
        /// Gets a value indicating whether the current tick is a full tick.
        /// </summary>
        public bool IsFullTick
        {
            get
            {
                return ((this.Tick + 1) & 3) == 0;
            }
        }

        /// <summary>
        /// Gets the total number of milliseconds.
        /// </summary>
        public int TotalMS
        {
            get
            {
                int ms = 1000 * (this.ClientSubTick / 60);

                if (this.ClientSubTick % 60 > 0)
                {
                    ms += (2133 * (this.ClientSubTick % 60)) >> 7;
                }

                return ms;
            }
        }

        /// <summary>
        /// Gets the total number of seconds.
        /// </summary>
        public int TotalSecs
        {
            get
            {
                return this.ClientSubTick > 0 ? LogicMath.Max((int)(uint)((((-2004318071L * (this.ClientSubTick + 59) >> 32) + this.ClientSubTick + 59) >> 31) + ((((-2004318071L * (this.ClientSubTick + 59)) >> 32) + this.ClientSubTick + 59) >> 5)), 1) : 0;
            }
        }
        
        /// <summary>
        /// Increases the tick.
        /// </summary>
        public void IncreaseTick()
        {
            this.Tick++;

            if ((this.Tick & 3) == 0)
            {
                this.FullTick++;
            }
        }

        /// <summary>
        /// Gets the specified number of seconds in ticks.
        /// </summary>
        public static int GetSecondsInTicks(int seconds)
        {
            return seconds * 60;
        }
    }
}