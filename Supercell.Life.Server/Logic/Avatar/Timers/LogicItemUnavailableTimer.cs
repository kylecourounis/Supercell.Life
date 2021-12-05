namespace Supercell.Life.Server.Logic.Avatar.Timers
{
    using System.Collections.Generic;
    using System.Linq;

    using Newtonsoft.Json;

    using Supercell.Life.Titan.Logic.Math;

    internal class LogicItemUnavailableTimer
    {
        internal LogicClientAvatar Avatar;

        [JsonProperty("timers")] internal Dictionary<int, LogicTimer> Items;

        /// <summary>
        /// Gets a value indicating whether this <see cref="LogicItemUnavailableTimer"/> has started.
        /// </summary>
        internal bool Started
        {
            get
            {
                return this.Items.Values.Any(timer => timer.Started);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicItemUnavailableTimer"/> class.
        /// </summary>
        public LogicItemUnavailableTimer()
        {
            this.Items = new Dictionary<int, LogicTimer>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicItemUnavailableTimer"/> class.
        /// </summary>
        public LogicItemUnavailableTimer(LogicClientAvatar avatar)
        {
            this.Avatar = avatar;
            this.Items  = new Dictionary<int, LogicTimer>();
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        internal void Start(int item)
        {
            this.Items.Add(item, new LogicTimer(this.Avatar.Time));

            var added = this.Items.Last();
            added.Value.StartTimer(this.Avatar.Time, 60 * 60);

            this.Avatar.ItemUnavailable.AddItem(item, added.Value.RemainingSecs * 15);
        }

        /// <summary>
        /// Finishes this instance.
        /// </summary>
        internal void Finish(int item)
        {
            var timer = this.Items[item];

            if (timer.Started)
            {
                timer.StopTimer();

                this.Avatar.ItemUnavailable.Remove(item);
                this.Items.Remove(item);

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
                foreach (var (item, timer) in this.Items)
                {
                    timer.FastForward(seconds);

                    if (timer.RemainingSecs <= 0)
                    {
                        this.Finish(item);
                    }
                }

                this.Update();
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

                foreach (var (item, timer) in this.Items)
                {
                    if (timer.RemainingSecs <= 0)
                    {
                        this.Finish(item);
                    }
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
                foreach (var timer in this.Items.Values)
                {
                    timer.AdjustSubTick();
                }
            }
        }

        /// <summary>
        /// Updates this instance.
        /// </summary>
        internal void Update()
        {
            foreach (var (item, timer) in this.Items)
            {
                this.Avatar.ItemUnavailable.Set(item, timer.RemainingSecs * 15);
            }
        }
    }
}