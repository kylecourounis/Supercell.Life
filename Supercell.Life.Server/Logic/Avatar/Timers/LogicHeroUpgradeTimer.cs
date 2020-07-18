namespace Supercell.Life.Server.Logic.Avatar.Timers
{
    using Newtonsoft.Json;

    using Supercell.Life.Titan.Logic.Json;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;

    internal class LogicHeroUpgradeTimer
    {
        internal LogicClientAvatar Avatar;

        [JsonProperty("hero_data")] internal int HeroData;
        [JsonProperty("upg_timer")] internal LogicTimer Timer;

        /// <summary>
        /// Gets a value indicating whether this <see cref="LogicHeroUpgradeTimer"/> has started.
        /// </summary>
        internal bool Upgrading
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
            if (!this.Upgrading)
            {
                if (this.Avatar.HeroLevels.ContainsKey(hero.GlobalID))
                {
                    return hero.Cost.Count > this.Avatar.HeroLevels[hero.GlobalID].Count + 1;
                }
            }

            return false;
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
            if (this.Upgrading)
            {
                this.Timer.StopTimer();

                this.Avatar.HeroLevels.AddItem(this.HeroData, 1);
                this.HeroData = -1;
                
                this.Avatar.Save();
            }
        }

        /// <summary>
        /// Fast forwards this instance by the specified number of seconds.
        /// </summary>
        internal void FastForward(int seconds)
        {
            if (this.Upgrading)
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
            if (this.Timer.Started)
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
            if (this.Upgrading)
            {
                this.Timer.AdjustSubTick();
            }
        }

        /// <summary>
        /// Saves this instance to the specified <see cref="LogicJSONObject"/>.
        /// </summary>
        internal void Save(LogicJSONObject json)
        {
            if (this.Upgrading)
            {
                json.Put("upgradingHero", new LogicJSONNumber(this.HeroData));
                json.Put("upgradingHeroTime", new LogicJSONNumber(this.Timer.RemainingSecs));
            }
        }
    }
}