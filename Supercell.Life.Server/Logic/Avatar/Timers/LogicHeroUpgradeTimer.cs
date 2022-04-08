namespace Supercell.Life.Server.Logic.Avatar.Timers
{
    using Newtonsoft.Json;

    using Supercell.Life.Titan.Logic.Json;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Files.CsvLogic;

    internal class LogicHeroUpgradeTimer
    {
        internal LogicClientAvatar Avatar;

        [JsonProperty] internal int HeroData;
        [JsonProperty] internal LogicTimer Timer;

        /// <summary>
        /// Gets a value indicating whether this <see cref="LogicHeroUpgradeTimer"/> has started.
        /// </summary>
        internal bool Started
        {
            get
            {
                return this.Timer.Started;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicHeroUpgradeTimer"/> class.
        /// </summary>
        public LogicHeroUpgradeTimer()
        {
            this.Timer = new LogicTimer();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicHeroUpgradeTimer"/> class.
        /// </summary>
        public LogicHeroUpgradeTimer(LogicClientAvatar avatar)
        {
            this.Avatar = avatar;
            this.Timer  = new LogicTimer(avatar.Time);
        }

        /// <summary>
        /// Determines whether the specified hero can be upgraded.
        /// </summary>
        internal bool CanUpgrade(LogicHeroData hero)
        {
            return this.Avatar.HeroLevels.ContainsKey(hero.GlobalID) && hero.Cost.Size > this.Avatar.HeroLevels[hero.GlobalID].Count + 1;
        }

        /// <summary>
        /// Starts upgrading the specified hero.
        /// </summary>
        internal void Start(LogicHeroData hero)
        {
            if (this.CanUpgrade(hero))
            {
                this.HeroData = hero.GlobalID;
                this.Timer.StartTimer(this.Avatar.Time, hero.UpgradeTimeSeconds[this.Avatar.HeroLevels.GetCount(hero.GlobalID)]);
            }
        }

        /// <summary>
        /// Finishes this instance.
        /// </summary>
        internal void Finish()
        {
            this.Timer.StopTimer();

            this.Avatar.HeroLevels.AddItem(this.HeroData, 1);
            this.HeroData = -1;

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
                json.Put("upgradingHero", new LogicJSONNumber(this.HeroData));
                json.Put("upgradingHeroTime", new LogicJSONNumber(this.Timer.RemainingSecs));
            }
        }
    }
}