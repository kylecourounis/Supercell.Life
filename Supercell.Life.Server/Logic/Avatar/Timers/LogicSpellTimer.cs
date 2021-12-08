namespace Supercell.Life.Server.Logic.Avatar.Timers
{
    using System.Collections.Generic;

    using Newtonsoft.Json;

    using Supercell.Life.Titan.Logic;
    using Supercell.Life.Titan.Logic.Json;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic.Avatar.Slots;
    using Supercell.Life.Server.Logic.Enums;

    internal class LogicSpellTimer
    {
        internal LogicClientAvatar Avatar;

        [JsonProperty("spells")] internal LogicArrayList<int> SpellIDs;
        [JsonProperty("timer")]  internal LogicTimer Timer;

        /// <summary>
        /// Gets the spell IDs as instances of <see cref="LogicSpellData"/>.
        /// </summary>
        private List<LogicSpellData> Spells
        {
            get
            {
                return this.SpellIDs.ConvertAll(id => (LogicSpellData)CSV.Tables.Get(Gamefile.Spells).GetDataWithID(id));
            }
        }
        
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
            this.SpellIDs = new LogicArrayList<int>();
            this.Timer    = new LogicTimer(avatar.Time);
        }
        
        /// <summary>
        /// Adds the spell to the queue.
        /// </summary>
        internal void AddSpell(LogicSpellData spell)
        {
            this.SpellIDs.Add(spell.GlobalID);

            if (!this.Started)
            {
                this.Start();
            }
        }

        /// <summary>
        /// Removes a spell from the queue.
        /// </summary>
        internal void RemoveSpell(LogicSpellData spell, int slot)
        {
            if (this.SpellIDs.Contains(spell.GlobalID))
            {
                this.SpellIDs.Remove(this.SpellIDs[slot]);
            }

            if (this.SpellIDs.Count == 0)
            {
                this.Finish();
            }
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        internal void Start()
        {
            this.Timer.StartTimer(this.Avatar.Time, this.Spells[0].CreateTime);
        }

        /// <summary>
        /// Finishes this instance.
        /// </summary>
        internal void Finish()
        {
            if (this.Started)
            {
                this.Timer.StopTimer();

                foreach (var spell in this.Spells)
                {
                    this.Avatar.SpellsReady.AddItem(spell.GlobalID, 1);
                }

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
                while (this.Timer.RemainingSecs <= 0)
                {
                    if (this.SpellIDs.Count > 0)
                    {
                        this.Avatar.SpellsReady.AddItem(this.Spells[0].GlobalID, 1);

                        this.SpellIDs.RemoveAt(0);

                        this.Start();
                    }
                    else
                    {
                        this.Finish();
                    }
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
        /// Saves this instance to the specified <see cref="LogicJSONObject"/>.
        /// </summary>
        internal void Save(LogicJSONObject json)
        {
            LogicJSONArray productionSlots = new LogicJSONArray();

            foreach (int spell in this.SpellIDs)
            {
                productionSlots.Add(new LogicDataSlot(spell, 1).Save());
            }

            LogicJSONObject jObject = new LogicJSONObject();

            jObject.Put("sp_prod_slots", productionSlots);
            jObject.Put("t", new LogicJSONNumber(this.Timer.RemainingSecs));

            json.Put("sp_prod_prod", jObject);
        }
    }
}