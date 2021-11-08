namespace Supercell.Life.Server.Logic.Avatar.Slots
{
    using Newtonsoft.Json.Linq;

    using Supercell.Life.Titan.Logic.Json;

    using Supercell.Life.Server.Logic.Game;

    internal class LogicHeroLevels : LogicDataSlots
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicHeroLevels"/> class.
        /// </summary>
        internal LogicHeroLevels(LogicClientAvatar avatar, int capacity = 15) : base(avatar, capacity)
        {
            // LogicHeroLevels.
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal override void Initialize()
        {
            this.Set(new LogicDataSlot(Globals.StartingCharacter.GlobalID, 0));

            /* LogicCharacters.ForEach((hero, level) =>
            {
                this.Set(new Item(hero.GlobalID, level));
            }); */
        }

        /// <summary>
        /// Gets the player's current hero loadout.
        /// </summary>
        internal LogicJSONArray CurrentLoadout
        {
            get
            {
                LogicJSONArray array = new LogicJSONArray();

                foreach (var hero in this.Avatar.Team)
                {
                    LogicJSONObject json = new LogicJSONObject();
                    json.Put("ch", new LogicJSONNumber(hero.Value<int>()));
                    array.Add(json);
                }

                return array;
            }
        }
    }
}