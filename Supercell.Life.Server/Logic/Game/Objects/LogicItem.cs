namespace Supercell.Life.Server.Logic.Game.Objects.Items
{
    using System.Linq;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Logic.Enums;

    internal class LogicItem
    {
        private readonly LogicClientAvatar Avatar;

        /// <summary>
        /// Gets the data.
        /// </summary>
        internal LogicItemsData Data
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether this item is attached to any of the heroes in the player's loadout. 
        /// </summary>
        internal bool IsAttached
        {
            get
            {
                return this.Avatar.ItemAttachedTo.Values.Any(item => item.Id.Equals(this.Data.GlobalID) && this.Avatar.Team.Any(hero => hero.ToObject<int>().Equals(item.Count)));
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicItem"/> class.
        /// </summary>
        internal LogicItem(LogicClientAvatar avatar, string name)
        {
            this.Avatar = avatar;
            this.Data   = (LogicItemsData)CSV.Tables.Get(Gamefile.Items).GetDataByName(name);
        }

        /// <summary>
        /// Returns a multiplier value based on the level of the specified item.
        /// </summary>
        internal double PercentageMultiplier(double percentage, double increment)
        {
            if (this.Avatar.ItemLevels.GetCount(this.Data.GlobalID) > 0)
            {
                for (int i = 1; i <= this.Avatar.ItemLevels.GetCount(this.Data.GlobalID); i++)
                {
                    percentage += increment;
                    increment  += 0.01;
                }
            }

            return percentage;
        }
    }
}
