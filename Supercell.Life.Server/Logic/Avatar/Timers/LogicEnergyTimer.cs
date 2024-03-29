﻿namespace Supercell.Life.Server.Logic.Avatar.Timers
{
    using Newtonsoft.Json;

    using Supercell.Life.Titan.Logic.Json;
    using Supercell.Life.Titan.Logic.Math;

    internal class LogicEnergyTimer
    {
        internal LogicClientAvatar Avatar;

        [JsonProperty] internal LogicTimer Timer;

        /// <summary>
        /// Gets a value indicating whether this <see cref="LogicEnergyTimer"/> has started.
        /// </summary>
        internal bool Started
        {
            get
            {
                return this.Timer.Started;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicEnergyTimer"/> class.
        /// </summary>
        internal LogicEnergyTimer()
        {
            this.Timer = new LogicTimer();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicEnergyTimer"/> class.
        /// </summary>
        internal LogicEnergyTimer(LogicClientAvatar avatar)
        {
            this.Avatar = avatar;
            this.Timer  = new LogicTimer(avatar.Time);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        internal void Start()
        {
            this.Timer.StartTimer(this.Avatar.Time, 300);
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        internal void Stop()
        {
            this.Timer.StopTimer();
            this.Avatar.Save();
        }

        /// <summary>
        /// Fast forwards this instance by the specified number of seconds.
        /// </summary>
        internal void FastForward(int seconds)
        {
            this.Timer.FastForward(seconds);
        }

        /// <summary>
        /// Ticks this instance.
        /// </summary>
        internal void Tick()
        {
            if (!this.Started)
            {
                if (this.Avatar.Energy < this.Avatar.MaxEnergy)
                {
                    this.Start();
                }
            }
            else
            {
                while (this.Timer.RemainingSecs <= 0)
                {
                    if (++this.Avatar.Energy < this.Avatar.MaxEnergy)
                    {
                        this.Start();
                    }
                    else this.Stop();
                }
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
        /// Gets the remaining time for this <see cref="LogicEnergyTimer"/>.
        /// </summary>
        internal int GetTimeRemaining()
        {
            return this.Started ? 300 - LogicMath.Abs(this.Timer.RemainingSecs - this.Avatar.TimeSinceLastSave) : this.Avatar.TimeSinceLastSave;
        }
        
        /// <summary>
        /// Saves this instance to the specified <see cref="LogicJSONObject"/>.
        /// </summary>
        internal void Save(LogicJSONObject json)
        {
            LogicJSONObject energy = new LogicJSONObject();
            energy.Put("enemyK", new LogicJSONNumber());
            energy.Put("given", new LogicJSONNumber());
            energy.Put("time", new LogicJSONNumber(this.Timer.RemainingSecs));

            json.Put("energyVars", energy);
        }
    }
}