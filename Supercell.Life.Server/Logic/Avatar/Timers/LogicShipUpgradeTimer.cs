namespace Supercell.Life.Server.Logic.Avatar.Timers
{
    using Newtonsoft.Json;

    using Supercell.Life.Titan.Logic.Json;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Logic.Game;

    internal class LogicShipUpgradeTimer
    {
        internal LogicClientAvatar Avatar;
        
        [JsonProperty("timer")] internal LogicTimer Timer;

        /// <summary>
        /// Gets a value indicating whether this <see cref="LogicShipUpgradeTimer"/> has started.
        /// </summary>
        internal bool Upgrading
        {
            get
            {
                return this.Timer.Started;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicShipUpgradeTimer"/> class.
        /// </summary>
        public LogicShipUpgradeTimer()
        {
            this.Timer = new LogicTimer();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicShipUpgradeTimer"/> class.
        /// </summary>
        public LogicShipUpgradeTimer(LogicClientAvatar avatar)
        {
            this.Avatar = avatar;
            this.Timer  = new LogicTimer(avatar.Time);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        internal void Start()
        {
            this.Timer.StartTimer(this.Avatar.Time, 3600 * ((LogicGlobalData)CSV.Tables.Get(Gamefile.Globals).GetDataByName("SHIP_UPGRADE_DURATION_HOURS")).NumberArray[this.Avatar.ShipLevel]);
        }

        /// <summary>
        /// Finishes this instance.
        /// </summary>
        internal void Finish()
        {
            if (this.Upgrading)
            {
                this.Timer.StopTimer();

                int cost = LogicGamePlayUtil.GetSpeedUpCost(this.Timer.RemainingSecs, LogicGamePlayUtil.GetSpeedUpCostMultiplier(1));
                Debugger.Debug(cost);

                this.Avatar.CommodityChangeCountHelper(CommodityType.Diamonds, -cost);

                this.Avatar.ShipLevel++;
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
                json.Put("ship_upgr", new LogicJSONNumber(this.Timer.RemainingSecs));
            }
        }
    }
}