namespace Supercell.Life.Server.Logic.Avatar.Slots
{
    using Supercell.Life.Titan.Logic.Json;

    using Supercell.Life.Server.Logic.Avatar.Items;

    internal class LogicItems
    {
        internal readonly LogicClientAvatar Avatar;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicItems"/> class.
        /// </summary>
        internal LogicItems(LogicClientAvatar avatar)
        {
            this.Avatar = avatar;
        }

        /// <summary>
        /// Saves the item arrays to the specified <see cref="LogicJSONObject"/>.
        /// </summary>
        internal void Save(LogicJSONObject json)
        {
            json.Put("item_lvls", this.ItemLevels);
            json.Put("item_attach", this.ItemAttach);
        }

        /// <summary>
        /// Gets the player's items as a JSON array.
        /// </summary>
        internal LogicJSONArray ItemLevels
        {
            get
            {
                LogicJSONArray array = new LogicJSONArray();

                foreach (Item item in this.Avatar.ItemLevels.Values)
                {
                    LogicJSONObject jsonObj = new LogicJSONObject();

                    jsonObj.Put("id", new LogicJSONNumber(item.Id));
                    jsonObj.Put("cnt", new LogicJSONNumber(item.Count));

                    array.Add(jsonObj);
                }

                return array;
            }
        }

        /// <summary>
        /// Gets the items and the hero that they are attached to.
        /// </summary>
        internal LogicJSONArray ItemAttach
        {
            get
            {
                LogicJSONArray array = new LogicJSONArray();

                foreach (Item hero in this.Avatar.HeroLevels.Values)
                {
                    if (this.Avatar.ItemAttachedTo.Get(hero.Id) != null)
                    {
                        LogicJSONObject jsonObj = new LogicJSONObject();

                        jsonObj.Put("id", new LogicJSONNumber(hero.Id));
                        jsonObj.Put("cnt", new LogicJSONNumber(this.Avatar.ItemAttachedTo.Get(hero.Id).Id));

                        array.Add(jsonObj);
                    }
                }

                return array;
            }
        }
    }
}
