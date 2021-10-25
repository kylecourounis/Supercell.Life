namespace Supercell.Life.Server.Logic.Avatar.Slots
{
    using System.Linq;

    using Supercell.Life.Titan.Logic.Json;

    using Supercell.Life.Server.Logic.Avatar.Items;

    internal class LogicItems
    {
        internal readonly LogicClientAvatar Avatar;

        #region Item IDs
        internal const int EnergyRecycler  = 37000000;
        internal const int PlunderThunder  = 37000001;
        internal const int RobbersHood     = 37000002;
        internal const int LuckyFix        = 37000003;
        internal const int CaptainsBounce  = 37000004;
        internal const int GalaxyDefender  = 37000005;
        internal const int MegaFlex        = 37000006;
        internal const int PowerProtector  = 37000007;
        internal const int ComboCruncher   = 37000008;
        internal const int AdventurersSnap = 37000009;
        internal const int SmackAttack     = 37000010;
        internal const int BubbleArmor     = 37000011;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicItems"/> class.
        /// </summary>
        internal LogicItems(LogicClientAvatar avatar)
        {
            this.Avatar = avatar;
        }

        /// <summary>
        /// Gets the XP boost percentage based on the level of plunder thunder.
        /// </summary>
        internal int PlunderThunderPercentage
        {
            get
            {
                int percentage = 5;
                int increment = 2;

                for (int i = 1; i <= this.Avatar.ItemLevels.GetCount(LogicItems.PlunderThunder); i++)
                {
                    percentage += increment;
                    increment++;
                }

                return percentage;
            }
        }

        /// <summary>
        /// Determines whether the item with the specified identifier is attached to any of the heroes in the player's loadout. 
        /// </summary>
        internal bool IsAttached(int itemId)
        {
            return this.Avatar.ItemAttachedTo.Values.Any(item => item.Id.Equals(this.Avatar.ItemLevels.Get(itemId).Id) && this.Avatar.Team.Any(hero => hero.ToObject<int>().Equals(item.Count)));
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
