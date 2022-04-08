namespace Supercell.Life.Server.Logic.Avatar.Timers
{
    using Newtonsoft.Json;

    using Supercell.Life.Titan.Logic.Json;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic.Enums;

    internal class LogicBoosterTimer
    {
        internal LogicClientAvatar Avatar;

        internal LogicBoosterData BoostPackage;

        [JsonProperty] internal int BoosterID;
        [JsonProperty] internal LogicTimer Timer;

        /// <summary>
        /// Gets a value indicating whether this <see cref="LogicBoosterTimer"/> has started.
        /// </summary>
        internal bool Started
        {
            get
            {
                return this.Timer.Started;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicBoosterTimer"/> class.
        /// </summary>
        public LogicBoosterTimer()
        {
            this.Timer = new LogicTimer();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicBoosterTimer"/> class.
        /// </summary>
        public LogicBoosterTimer(LogicClientAvatar avatar)
        {
            this.Avatar = avatar;
            this.Timer  = new LogicTimer(avatar.Time);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        internal void Start()
        {
            this.BoosterID = this.BoostPackage.GlobalID;

            this.Timer.StopTimer();
            this.Timer.StartTimer(this.Avatar.Time, this.BoostPackage.TimeDays * 86400);
        }

        /// <summary>
        /// Finishes this instance.
        /// </summary>
        internal void Finish()
        {
            this.Timer.StopTimer();

            this.BoosterID = 0;
            this.BoostPackage = null;

            this.Avatar.Save();
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
            if (this.Started)
            {
                this.BoostPackage = (LogicBoosterData)CSV.Tables.Get(Gamefile.Boosters).GetDataWithID(this.BoosterID);

                json.Put("xp_boost_t", new LogicJSONNumber(this.Timer.RemainingSecs));
                json.Put("xp_boost_p", new LogicJSONNumber(this.BoostPackage.BoostPercentage));
            }
        }
    }
}