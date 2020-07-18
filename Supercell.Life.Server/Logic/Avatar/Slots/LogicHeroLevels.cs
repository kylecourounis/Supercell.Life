namespace Supercell.Life.Server.Logic.Avatar.Slots
{
    using Newtonsoft.Json.Linq;

    using Supercell.Life.Titan.Logic.Json;

    using Supercell.Life.Server.Logic.Avatar.Items;
    using Supercell.Life.Server.Logic.Game;

    internal class LogicHeroLevels : LogicDataSlot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicHeroLevels"/> class.
        /// </summary>
        internal LogicHeroLevels(int capacity = 15) : base(capacity)
        {
            // LogicHeroLevels.
        }

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
            this.Set(new Item(Globals.StartingCharacter.GlobalID, 0));

            /* Characters.ForEach((Hero, Level) =>
            {
                this.Set(new Item(Hero.GlobalID, Level));
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
                    LogicJSONObject jsonObj = new LogicJSONObject();
                    jsonObj.Put("ch", new LogicJSONNumber(hero.Value<int>()));
                    array.Add(jsonObj);
                }

                return array;
            }
        }
    }
}