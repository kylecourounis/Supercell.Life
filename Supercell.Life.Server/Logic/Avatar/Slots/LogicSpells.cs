namespace Supercell.Life.Server.Logic.Avatar.Slots
{
    using Newtonsoft.Json.Linq;

    using Supercell.Life.Titan.Logic.Json;

    internal class LogicSpells : LogicDataSlots
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicSpells"/> class.
        /// </summary>
        internal LogicSpells(LogicClientAvatar avatar) : base(avatar)
        {
            // LogicSpells.
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal override void Initialize()
        {
        }

        /// <summary>
        /// Adds a spell used in the previous battle.
        /// </summary>
        internal void AddPreviousSpell(int id, int count)
        {
            this.Avatar.PreviousSpells.Add(new JObject
            {
                { "id",  id    },
                { "cnt", count }
            });

            this.Avatar.Save();
        }

        /// <summary>
        /// Saves the spell arrays to the specified <see cref="LogicJSONObject"/>.
        /// </summary>
        internal void Save(LogicJSONObject json)
        {
            json.Put("prev_spells", LogicJSONParser.ParseArray(this.Avatar.PreviousSpells.ToString()));
            json.Put("spells_lvls", this.SpellLevels);

            this.Avatar.SpellTimer.Save(json);
        }

        /// <summary>
        /// Gets the spell levels as a <see cref="LogicJSONArray"/>.
        /// </summary>
        internal LogicJSONArray SpellLevels
        {
            get
            {
                LogicJSONArray array = new LogicJSONArray();

                foreach (var spell in this.Values)
                {
                    array.Add(spell.Save());
                }

                return array;
            }
        }
    }
}
