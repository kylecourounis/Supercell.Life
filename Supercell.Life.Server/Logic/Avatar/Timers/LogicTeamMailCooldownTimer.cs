namespace Supercell.Life.Server.Logic.Avatar.Timers
{
    using Newtonsoft.Json;

    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Logic.Game;

    internal class LogicTeamMailCooldownTimer
    {
        internal LogicClientAvatar Avatar;

        internal LogicTime Time;

        [JsonProperty] internal LogicTimer Timer;

        /// <summary>
        /// Gets a value indicating whether this <see cref="LogicTeamMailCooldownTimer"/> has started.
        /// </summary>
        internal bool Started
        {
            get
            {
                return this.Timer.Started;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicTeamMailCooldownTimer"/> class.
        /// </summary>
        public LogicTeamMailCooldownTimer()
        {
            this.Timer = new LogicTimer();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicTeamMailCooldownTimer"/> class.
        /// </summary>
        internal LogicTeamMailCooldownTimer(LogicClientAvatar avatar)
        {
            this.Avatar = avatar;
            this.Time   = new LogicTime();
            this.Timer  = new LogicTimer(this.Time);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        internal void Start()
        {
            this.Timer.StartTimer(this.Time, Globals.TeamMailSendCooldownTime);
        }

        /// <summary>
        /// Finishes this instance.
        /// </summary>
        internal void Finish()
        {
            if (this.Started)
            {
                this.Timer.StopTimer();
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
