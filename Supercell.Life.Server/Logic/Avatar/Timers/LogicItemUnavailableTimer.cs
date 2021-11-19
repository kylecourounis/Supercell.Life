namespace Supercell.Life.Server.Logic.Avatar.Timers
{
    using Newtonsoft.Json;

    using Supercell.Life.Titan.Logic.Math;

    internal class LogicItemUnavailableTimer
    {
        internal LogicClientAvatar Avatar;
        
        [JsonProperty("item")]  internal int Item;
        [JsonProperty("timer")] internal LogicTimer Timer;

        /// <summary>
        /// Gets a value indicating whether this <see cref="LogicItemUnavailableTimer"/> has started.
        /// </summary>
        internal bool Started
        {
            get
            {
                return this.Timer.Started;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicItemUnavailableTimer"/> class.
        /// </summary>
        public LogicItemUnavailableTimer()
        {
            this.Timer = new LogicTimer();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicItemUnavailableTimer"/> class.
        /// </summary>
        public LogicItemUnavailableTimer(LogicClientAvatar avatar)
        {
            this.Avatar = avatar;
            this.Timer  = new LogicTimer(avatar.Time);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        internal void Start(int item)
        {
            this.Item = item;
            this.Timer.StartTimer(this.Avatar.Time, 60 * 60);

            this.Avatar.ItemUnavailable.AddItem(item, this.Timer.RemainingSecs * 15);
        }

        /// <summary>
        /// Finishes this instance.
        /// </summary>
        internal void Finish()
        {
            if (this.Started)
            {
                this.Timer.StopTimer();

                this.Avatar.ItemUnavailable.Remove(this.Item);
                this.Item = -1;

                this.Avatar.Save();
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
                
                this.Update();

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
                this.Update();

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

        /// <summary>
        /// Updates this instance.
        /// </summary>
        internal void Update()
        {
            this.Avatar.ItemUnavailable.Set(this.Item, this.Timer.RemainingSecs * 15);
        }
    }
}