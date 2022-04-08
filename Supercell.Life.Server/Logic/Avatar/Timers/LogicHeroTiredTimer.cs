namespace Supercell.Life.Server.Logic.Avatar.Timers
{
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic.Enums;

    internal class LogicHeroTiredTimer
    {
        internal LogicClientAvatar Avatar;

        [JsonProperty("Timers")] internal Dictionary<int, LogicTimer> Heroes;

        /// <summary>
        /// Gets a value indicating whether this <see cref="LogicHeroTiredTimer"/> has started.
        /// </summary>
        internal bool Started
        {
            get
            {
                return this.Heroes.Values.Any(timer => timer.Started);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicHeroTiredTimer"/> class.
        /// </summary>
        public LogicHeroTiredTimer()
        {
            this.Heroes = new Dictionary<int, LogicTimer>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicItemUnavailableTimer"/> class.
        /// </summary>
        public LogicHeroTiredTimer(LogicClientAvatar avatar)
        {
            this.Avatar = avatar;
            this.Heroes = new Dictionary<int, LogicTimer>();
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        internal void Start(int hero)
        {
            this.Heroes.Add(hero, new LogicTimer(this.Avatar.Time));

            LogicHeroData data = (LogicHeroData)CSV.Tables.Get(Gamefile.Heroes).GetDataWithID(hero);

            var added = this.Heroes.Last();
            added.Value.StartTimer(this.Avatar.Time, data.TiredTimer * 60);

            this.Avatar.HeroTired.AddItem(hero, added.Value.RemainingSecs);
        }

        /// <summary>
        /// Finishes this instance.
        /// </summary>
        internal void Finish()
        {
            foreach (var (hero, timer) in this.Heroes)
            {
                if (timer.RemainingSecs <= 0)
                {
                    timer.StopTimer();

                    this.Avatar.HeroTired.Remove(hero);
                    this.Heroes.Remove(hero);

                    this.Avatar.Save();
                }
            }
        }

        /// <summary>
        /// Fast forwards this instance by the specified number of seconds.
        /// </summary>
        internal void FastForward(int seconds)
        {
            foreach (var timer in this.Heroes.Values.Where(timer => timer.RemainingSecs <= 0))
            {
                timer.FastForward(seconds);
            }

            this.Update();
            this.Finish();
        }

        /// <summary>
        /// Ticks this instance.
        /// </summary>
        internal void Tick()
        {
            this.Update();
            this.Finish();
        }

        /// <summary>
        /// Adjusts the subtick of this instance.
        /// </summary>
        internal void AdjustSubTick()
        {
            foreach (var timer in this.Heroes.Values)
            {
                timer.AdjustSubTick();
            }
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        internal void Update()
        {
            foreach (var (hero, timer) in this.Heroes)
            {
                this.Avatar.HeroTired.Set(hero, timer.RemainingSecs);
            }
        }
    }
}