﻿namespace Supercell.Life.Server.Logic.Avatar.Timers
{
    using Newtonsoft.Json;

    using Supercell.Life.Titan.Logic.Json;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Logic.Avatar.Slots;
    using Supercell.Life.Server.Logic.Game;

    internal class LogicSailingTimer
    {
        internal LogicClientAvatar Avatar;

        [JsonProperty] internal LogicTimer Timer;

        [JsonProperty] internal LogicDataSlots Heroes;
        [JsonProperty] internal LogicDataSlots HeroLevels;

        /// <summary>
        /// Gets a value indicating whether this <see cref="LogicSailingTimer"/> has started.
        /// </summary>
        internal bool Started
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

            this.Heroes     = new LogicDataSlots(avatar);
            this.HeroLevels = new LogicDataSlots(avatar);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        internal void Start()
        {
            this.Timer.StartTimer(this.Avatar.Time, 3600 * Globals.ShipSailDurationHours);
        }

        /// <summary>
        /// Finishes this instance.
        /// </summary>
        internal void Finish()
        {
            if (this.Started)
            {
                this.Timer.StopTimer();

                this.Avatar.Variables.Set(LogicVariables.SailRewardUnclaimed.GlobalID, 0);

                this.Avatar.Save();
            }
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
                json.Put("sailTime", new LogicJSONNumber(this.Timer.RemainingSecs));
            }
        }
    }
}
