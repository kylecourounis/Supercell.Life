namespace Supercell.Life.Server.Logic.Alliance
{
    using Newtonsoft.Json;

    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Logic.Game;

    internal class LogicTeamGoalTimer
    {
        internal Alliance Alliance;

        internal LogicTime Time;

        [JsonProperty("timer")] internal LogicTimer Timer;

        /// <summary>
        /// Gets a value indicating whether this <see cref="LogicTeamGoalTimer"/> has started.
        /// </summary>
        internal bool Started
        {
            get
            {
                return this.Timer.Started;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicTeamGoalTimer"/> class.
        /// </summary>
        public LogicTeamGoalTimer()
        {
            this.Timer = new LogicTimer();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicTeamGoalTimer"/> class.
        /// </summary>
        internal LogicTeamGoalTimer(Alliance alliance)
        {
            this.Alliance   = alliance;
            this.Time       = new LogicTime();
            this.Timer      = new LogicTimer(this.Time);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        internal void Start()
        {
            this.Timer.StartTimer(this.Time, 3600 * Globals.TeamGoalSeasonDurationHours);
        }

        /// <summary>
        /// Finishes this instance.
        /// </summary>
        internal void Finish()
        {
            if (this.Started)
            {
                this.Timer.StopTimer();

                // Set new objectives here

                this.Start();
            }
        }

        /// <summary>
        /// Fast forwards this instance by the specified number of seconds.
        /// </summary>
        internal void FastForward(int seconds)
        {
            if (this.Started)
            {
                this.Timer.FastForward(seconds);

                if (this.Timer.RemainingSecs <= 0)
                {
                    this.Finish();
                }
            }
        }

        /// <summary>
        /// Ticks this instance.
        /// </summary>
        internal void Tick()
        {
            if (this.Started)
            {
                if (this.Timer.RemainingSecs <= 0)
                {
                    this.Finish();
                }
            }
        }

        /// <summary>
        /// Adjusts the subtick of this instance.
        /// </summary>
        internal void AdjustSubTick()
        {
            if (this.Started)
            {
                this.Timer.AdjustSubTick();
            }
        }
    }
}
