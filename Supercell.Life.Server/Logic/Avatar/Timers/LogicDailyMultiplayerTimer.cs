namespace Supercell.Life.Server.Logic.Avatar.Timers
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;

    using Supercell.Life.Titan.Logic.Json;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Logic.Avatar;

    internal class LogicDailyMultiplayerTimer
    {
        internal LogicClientAvatar Avatar;

        internal LogicTime Time;

        [JsonProperty] internal JArray HeroUsedInDaily;

        [JsonProperty] internal LogicTimer Timer;

        /// <summary>
        /// Gets a value indicating whether this <see cref="LogicDailyMultiplayerTimer"/> has started.
        /// </summary>
        internal bool Started
        {
            get
            {
                return this.Timer.Started;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicDailyMultiplayerTimer"/> class.
        /// </summary>
        public LogicDailyMultiplayerTimer()
        {
            this.Timer = new LogicTimer();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicDailyMultiplayerTimer"/> class.
        /// </summary>
        internal LogicDailyMultiplayerTimer(LogicClientAvatar avatar)
        {
            this.Avatar          = avatar;
            this.Time            = new LogicTime();
            this.Timer           = new LogicTimer(this.Time);
            this.HeroUsedInDaily = new JArray();
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        internal void Start()
        {
            if (this.Started)
            {
                return;
            }

            this.Timer.StartTimer(this.Time, 3600 * 24);
            this.HeroUsedInDaily.Clear();
        }

        /// <summary>
        /// Finishes this instance.
        /// </summary>
        internal void Finish()
        {
            this.Timer.StopTimer();
        }

        /// <summary>
        /// Fast forwards this instance by the specified number of seconds.
        /// </summary>
        internal void FastForward(int seconds)
        {
            this.Timer.FastForward(seconds);

            if (this.Timer.RemainingSecs <= 0)
            {
                this.Finish();
            }
        }

        /// <summary>
        /// Ticks this instance.
        /// </summary>
        internal void Tick()
        {
            if (this.Timer.RemainingSecs <= 0)
            {
                this.Finish();
            }
        }

        /// <summary>
        /// Adjusts the subtick of this instance.
        /// </summary>
        internal void AdjustSubTick()
        {
            this.Timer.AdjustSubTick();
        }

        /// <summary>
        /// Saves this instance to the specified <see cref="LogicJSONObject"/>.
        /// </summary>
        internal void Save(LogicJSONObject json)
        {
            json.Put("freePvp", new LogicJSONNumber(this.Timer.RemainingSecs));

            var heroUsedInDaily = new LogicJSONArray();

            foreach (int hero in this.HeroUsedInDaily.ToObject<int[]>())
            {
                heroUsedInDaily.Add(new LogicJSONNumber(hero));
            }

            json.Put("heroUsedInDaily", heroUsedInDaily);
        }
    }
}
