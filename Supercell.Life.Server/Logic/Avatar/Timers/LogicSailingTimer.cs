namespace Supercell.Life.Server.Logic.Avatar.Timers
{
    using Newtonsoft.Json;

    using Supercell.Life.Titan.Logic.Json;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Avatar.Items;
    using Supercell.Life.Server.Logic.Avatar.Slots;
    using Supercell.Life.Server.Logic.Enums;

    internal class LogicSailingTimer
    {
        internal LogicClientAvatar Avatar;

        [JsonProperty("timer")] internal LogicTimer Timer;

        [JsonProperty]          internal LogicDataSlot Heroes;
        [JsonProperty]          internal LogicDataSlot HeroLevels;

        /// <summary>
        /// Gets a value indicating whether this <see cref="LogicSailingTimer"/> has started.
        /// </summary>
        internal bool Sailing
        {
            get
            {
                return this.Timer.Started;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicSailingTimer"/> class.
        /// </summary>
        internal LogicSailingTimer()
        {
            this.Timer = new LogicTimer();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicSailingTimer"/> class.
        /// </summary>
        internal LogicSailingTimer(LogicClientAvatar avatar)
        {
            this.Avatar     = avatar;
            this.Timer      = new LogicTimer(avatar.Time);

            this.Heroes     = new LogicDataSlot(avatar);
            this.HeroLevels = new LogicDataSlot(avatar);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        internal void Start()
        {
            this.Timer.StartTimer(this.Avatar.Time, 25200);
        }

        /// <summary>
        /// Finishes this instance.
        /// </summary>
        internal void Finish()
        {
            if (this.Sailing)
            {
                this.Timer.StopTimer();

                LogicGlobalData globals = (LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SHIP_SEASICK_DURATION_HOURS");
                this.Avatar.Seasick = globals.NumberValue;

                this.Avatar.Variables.Set(LogicVariables.SailRewardUnclaimed.GlobalID, 1);
                
                foreach (Item hero in this.Heroes.Values)
                {
                    this.Avatar.HeroTired.AddItem(hero.Id, hero.Count);
                }

                this.Avatar.Sailing.Heroes.Clear();
                this.Avatar.Sailing.HeroLevels.Clear();

                this.Avatar.Save();
            }
        }

        /// <summary>
        /// Fast forwards this instance by the specified number of seconds.
        /// </summary>
        internal void FastForward(int seconds)
        {
            if (this.Sailing)
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
            if (this.Sailing)
            {
                this.Timer.AdjustSubTick();
            }
        }

        /// <summary>
        /// Saves this instance to the specified <see cref="LogicJSONObject"/>.
        /// </summary>
        internal void Save(LogicJSONObject json)
        {
            if (this.Sailing)
            {
                json.Put("sailTime", new LogicJSONNumber(this.Timer.RemainingSecs));
            }
        }
    }
}
