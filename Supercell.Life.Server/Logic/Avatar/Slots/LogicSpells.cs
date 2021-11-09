namespace Supercell.Life.Server.Logic.Avatar.Slots
{
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
        /// Saves the spell arrays to the specified <see cref="LogicJSONObject"/>.
        /// </summary>
        internal void Save(LogicJSONObject json)
        {
            json.Put("prev_spells", this.PreviousSpells);
            json.Put("spells_lvls", this.SpellLevels);
        }

        /// <summary>
        /// Gets the previous spells as a <see cref="LogicJSONArray"/>.
        /// </summary>
        internal LogicJSONArray PreviousSpells
        {
            get
            {
                LogicJSONArray array = new LogicJSONArray();

                foreach (var spell in this.Avatar.SpellsReady.Values)
                {
                    array.Add(spell.Save());
                }

                return array;
            }
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
