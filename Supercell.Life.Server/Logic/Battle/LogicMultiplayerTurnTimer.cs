namespace Supercell.Life.Server.Logic.Battle
{
    using Newtonsoft.Json;

    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Logic.Game;

    internal class LogicMultiplayerTurnTimer
    {
        internal LogicBattle Battle;

        internal int EnemyReconnectTurns;

        internal LogicTime Time;

        [JsonProperty("timer")] internal LogicTimer Timer;

        /// <summary>
        /// Gets a value indicating whether this <see cref="LogicMultiplayerTurnTimer"/> has started.
        /// </summary>
        internal bool Started
        {
            get
            {
                return this.Timer.Started;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicMultiplayerTurnTimer"/> class.
        /// </summary>
        public LogicMultiplayerTurnTimer()
        {
            this.Timer = new LogicTimer();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicMultiplayerTurnTimer"/> class.
        /// </summary>
        internal LogicMultiplayerTurnTimer(LogicBattle battle)
        {
            this.Battle = battle;
            this.Time   = new LogicTime();
            this.Timer  = new LogicTimer(this.Time);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        internal void Start()
        {
            this.Timer.StartTimer(this.Time, Globals.PVPFirstTurnTimeSeconds);
        }

        /// <summary>
        /// Finishes this instance.
        /// </summary>
        internal void Reset()
        {
            if (this.Started)
            {
                this.Timer.StopTimer();
                this.Timer.StartTimer(this.Time, Globals.PVPMaxTurnTimeSeconds);
            }
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
                    this.Reset();
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
