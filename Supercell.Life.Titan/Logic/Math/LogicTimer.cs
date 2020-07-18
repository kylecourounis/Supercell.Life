namespace Supercell.Life.Titan.Logic.Math
{
    using System;
    
    public class LogicTimer
    {
        public LogicTime Time;

        public int TotalSeconds;
        public int StartSubTick = -1;

        /// <summary>
        /// Gets the remaining number of seconds on this <see cref="LogicTimer"/>.
        /// </summary>
        public int RemainingSecs
        {
            get
            {
                if (this.TotalSeconds > 0)
                {
                    if (this.StartSubTick != -1)
                    {
                        int ticks = LogicTime.GetSecondsInTicks(this.TotalSeconds) + this.StartSubTick - this.Time.ClientSubTick;

                        if (ticks > 0)
                        {
                            return LogicMath.Max(
                                (int)(uint)((((-2004318071L * (ticks + 59) >> 32) + ticks + 59) >> 31)
                                              + ((((-2004318071L * (ticks + 59)) >> 32) + ticks + 59) >> 5)),
                                1);
                        }

                        return 0;
                    }

                    return this.TotalSeconds;
                }

                return 0;
            }
        }

        /// <summary>
        /// Gets the remaining number of milliseconds on this <see cref="LogicTimer"/>.
        /// </summary>
        public int RemainingMS
        {
            get
            {
                if (this.TotalSeconds > 0)
                {
                    if (this.StartSubTick != -1)
                    {
                        int ticks = LogicTime.GetSecondsInTicks(this.TotalSeconds) + this.StartSubTick - this.Time.ClientSubTick;
                        int ms = 1000 * (ticks / 60);

                        if (ticks % 60 > 0)
                        {
                            ms += 2133 * (ticks % 60) >> 7;
                        }

                        return ms;
                    }

                    return this.TotalSeconds;
                }

                return 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="LogicTimer"/> has started.
        /// </summary>
        public bool Started
        {
            get
            {
                return this.StartSubTick != -1;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicTimer"/> class.
        /// </summary>
        public LogicTimer()
        {
            // LogicTimer.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicTimer"/> class.
        /// </summary>
        public LogicTimer(LogicTime time)
        {
            this.Time = time;
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void StartTimer(int seconds)
        {
            if (this.Time != null)
            {
                this.TotalSeconds += seconds;

                if (!this.Started)
                {
                    this.StartSubTick = this.Time.ClientSubTick;
                }
            }
            else throw new Exception("Unable to start timer when the 'Time' is null. Start Timer with StartTimer(Time,Secs) method.");
        }

        /// <summary>
        /// Starts the timer.
        /// </summary>
        public void StartTimer(LogicTime time, int seconds)
        {
            this.Time = time;
            this.TotalSeconds = seconds;
            this.StartSubTick = this.Time.ClientSubTick;
        }

        /// <summary>
        /// Stops the timer.
        /// </summary>
        public void StopTimer()
        {
            if (this.Started)
            {
                this.StartSubTick = -1;
                this.TotalSeconds = 0;
            }
        }

        /// <summary>
        /// Fast forwards this instance by the specified number of seconds.
        /// </summary>
        public void FastForward(int seconds)
        {
            if (this.StartSubTick != -1)
            {
                if (seconds >= 0)
                {
                    this.TotalSeconds -= seconds;
                }
            }
        }

        /// <summary>
        /// Adjusts the subtick of this instance.
        /// </summary>
        public void AdjustSubTick()
        {
            if (this.Started)
            {
                this.TotalSeconds = (LogicTime.GetSecondsInTicks(this.TotalSeconds) + this.StartSubTick) / 60;
                this.StartSubTick = 0;
            }
        }
    }
}