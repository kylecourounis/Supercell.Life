namespace Supercell.Life.Server.Logic.Avatar.Timers
{
    using System.Linq;

    using Newtonsoft.Json;

    using Supercell.Life.Titan.Logic.Math;
    using Supercell.Life.Titan.Logic.Json;

    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Logic.Game;

    internal class LogicBonusChestRespawnTimer
    {
        internal LogicClientAvatar Avatar;

        internal LogicTime Time;

        [JsonProperty] internal int ReplayQuest;
        [JsonProperty] internal int ReplayChestTimes;

        [JsonProperty] internal int PreviousReplayQuest;

        [JsonProperty("timer")] internal LogicTimer Timer;

        /// <summary>
        /// Gets a value indicating whether this <see cref="LogicBonusChestRespawnTimer"/> has started.
        /// </summary>
        internal bool Started
        {
            get
            {
                return this.Timer.Started;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicBonusChestRespawnTimer"/> class.
        /// </summary>
        public LogicBonusChestRespawnTimer()
        {
            this.Timer = new LogicTimer();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicBonusChestRespawnTimer"/> class.
        /// </summary>
        internal LogicBonusChestRespawnTimer(LogicClientAvatar avatar)
        {
            this.Avatar = avatar;
            this.Time   = new LogicTime();
            this.Timer  = new LogicTimer(this.Time);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        internal void Start()
        {
            this.SetReplayQuest();

            this.Timer.StartTimer(this.Time, 3600 * Globals.ReplayChestRespawnHours);
        }

        /// <summary>
        /// Finishes this instance.
        /// </summary>
        internal void Finish()
        {
            if (this.Started)
            {
                this.Timer.StopTimer();
            }

            this.ReplayQuest = 0;
            this.ReplayChestTimes = 0;

            this.Start();
        }

        /// <summary>
        /// Fast forwards this instance by the specified number of seconds.
        /// </summary>
        internal void FastForward(int seconds)
        {
            if (this.Started)
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
            if (this.Started)
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
            if (this.Started)
            {
                this.Timer.AdjustSubTick();
            }
        }

        /// <summary>
        /// Sets the replay quest.
        /// </summary>
        private void SetReplayQuest()
        {
            if (this.ReplayQuest == 0)
            {
                if (this.Avatar.NpcProgress != null)
                {
                    var basicQuests = this.Avatar.NpcProgress.Values.ToList();

                    if (basicQuests.Count > 1)
                    {
                        this.PreviousReplayQuest = this.ReplayQuest;

                        var completedQuests = basicQuests.Take(basicQuests.Count - 1).ToList();
                        this.ReplayQuest = completedQuests[this.Avatar.GameMode.Random.Rand(completedQuests.Count - 1)].Data.GlobalID;

                        if (this.PreviousReplayQuest == 0)
                        {
                            this.PreviousReplayQuest = this.ReplayQuest;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Saves this instance to the specified <see cref="LogicJSONObject"/>.
        /// </summary>
        internal void Save(LogicJSONObject json)
        {
            if (this.Avatar.ExpLevel >= Globals.ReplayChestAvailableOnXPLevel)
            {
                json.Put("m_replayChestAutomaticRespawnTimer", new LogicJSONNumber(this.Timer.RemainingSecs));
                json.Put("m_replayChestRespawnTimer", new LogicJSONNumber(this.Timer.RemainingSecs));
                json.Put("mapDailyBonusChestRespawnTimer", new LogicJSONNumber(this.Timer.RemainingSecs));
                
                json.Put("replayChestQuest", new LogicJSONNumber(this.ReplayQuest));
                json.Put("replayChestTimes", new LogicJSONNumber(this.ReplayChestTimes));
                json.Put("last_replay_level", new LogicJSONNumber(this.PreviousReplayQuest));
            }
        }
    }
}
