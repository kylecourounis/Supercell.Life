namespace Supercell.Life.Server.Logic.Avatar.Timers
{
    using System.Linq;

    using Newtonsoft.Json;

    using Supercell.Life.Titan.Logic;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic.Enums;

    internal class LogicSpellTimer
    {
        internal LogicClientAvatar Avatar;

        private readonly LogicArrayList<LogicSpellData> Spells;

        [JsonProperty("spells")] internal LogicArrayList<int> SpellIDs;
        [JsonProperty("timer")]  internal LogicTimer Timer;

        /// <summary>
        /// Gets a value indicating whether this <see cref="LogicSpellTimer"/> has started.
        /// </summary>
        internal bool Started
        {
            get
            {
                return this.Timer.Started;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicSpellTimer"/> class.
        /// </summary>
        public LogicSpellTimer()
        {
            this.Timer = new LogicTimer();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicSpellTimer"/> class.
        /// </summary>
        public LogicSpellTimer(LogicClientAvatar avatar)
        {
            this.Avatar   = avatar;
            this.Spells   = new LogicArrayList<LogicSpellData>();
            this.SpellIDs = new LogicArrayList<int>();
            this.Timer    = new LogicTimer(avatar.Time);
            
            foreach (int spellId in this.SpellIDs)
            {
                this.Spells.Add((LogicSpellData)CSV.Tables.Get(Gamefile.Spells).GetDataWithID(spellId));
            }
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        internal void Start()
        {
            this.Spells.Add((LogicSpellData)CSV.Tables.Get(Gamefile.Spells).GetDataWithID(this.SpellIDs.Last()));

            if (this.Spells.Count <= 1)
            {
                this.Timer.StartTimer(this.Avatar.Time, this.Spells.Last().CreateTime);
            }
            else
            {
                int newTime = this.Timer.RemainingSecs + this.Spells.Last().CreateTime;

                this.Timer.StopTimer();
                this.Timer.StartTimer(this.Avatar.Time, newTime);
            }
        }

        /// <summary>
        /// Resets this instance.
        /// </summary>
        internal void Reset(LogicSpellData toRemove)
        {
            if (this.Spells.Count > 0)
            {
                if (this.SpellIDs.Contains(toRemove.GlobalID))
                {
                    int newTime = this.Timer.RemainingSecs - toRemove.CreateTime;

                    this.Spells.Remove(toRemove);
                    this.SpellIDs.Remove(this.SpellIDs.FindIndex(value => value.Equals(toRemove.GlobalID)));

                    this.Timer.StopTimer();
                    this.Timer.StartTimer(this.Avatar.Time, newTime);
                }
            }
        }

        /// <summary>
        /// Finishes this instance.
        /// </summary>
        internal void Finish()
        {
            if (this.Started)
            {
                this.Timer.StopTimer();

                foreach (LogicSpellData spell in this.Spells)
                {
                    this.Avatar.SpellsReady.AddItem(spell.GlobalID, 1);
                }
                
                this.Spells.Clear();
                this.SpellIDs.Clear();

                this.Avatar.Save();
            }
        }

        /// <summary>
        /// Fast forwards this instance by the specified number of seconds.
        /// </summary>
        internal void FastForward(int seconds)
        {
            if (this.Timer.Started)
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
            this.Timer.AdjustSubTick();
        }
    }
}