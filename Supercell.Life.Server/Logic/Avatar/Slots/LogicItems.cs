namespace Supercell.Life.Server.Logic.Avatar.Slots
{
    using System.Linq;

    using Supercell.Life.Titan.Logic.Json;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic.Enums;

    internal class LogicItems
    {
        internal readonly LogicClientAvatar Avatar;

        #region ItemsData

        internal static readonly LogicItemsData EnergyRecycler  = (LogicItemsData)CSV.Tables.Get(Gamefile.Items).GetDataByName("GetEnergy");
        internal static readonly LogicItemsData PlunderThunder  = (LogicItemsData)CSV.Tables.Get(Gamefile.Items).GetDataByName("GetXP");
        internal static readonly LogicItemsData RobbersHood     = (LogicItemsData)CSV.Tables.Get(Gamefile.Items).GetDataByName("GetGold");
        internal static readonly LogicItemsData LuckyFix        = (LogicItemsData)CSV.Tables.Get(Gamefile.Items).GetDataByName("BarrelOdds");
        internal static readonly LogicItemsData CaptainsBounce  = (LogicItemsData)CSV.Tables.Get(Gamefile.Items).GetDataByName("PlusCombo");
        internal static readonly LogicItemsData GalaxyDefender  = (LogicItemsData)CSV.Tables.Get(Gamefile.Items).GetDataByName("HPPlus");
        internal static readonly LogicItemsData MegaFlex        = (LogicItemsData)CSV.Tables.Get(Gamefile.Items).GetDataByName("SpecialAttackPlus");
        internal static readonly LogicItemsData PowerProtector  = (LogicItemsData)CSV.Tables.Get(Gamefile.Items).GetDataByName("GetHpEnemyCombo");
        internal static readonly LogicItemsData ComboCruncher   = (LogicItemsData)CSV.Tables.Get(Gamefile.Items).GetDataByName("StartCombo");
        internal static readonly LogicItemsData AdventurersSnap = (LogicItemsData)CSV.Tables.Get(Gamefile.Items).GetDataByName("DoubleDmg");
        internal static readonly LogicItemsData SmackAttack     = (LogicItemsData)CSV.Tables.Get(Gamefile.Items).GetDataByName("DmgPlus");
        internal static readonly LogicItemsData BubbleArmor     = (LogicItemsData)CSV.Tables.Get(Gamefile.Items).GetDataByName("Shield");

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
        internal double PlunderThunderPercentage
        {
            get
            {
                double percentage = 1.05;
                double increment  = 0.02;

                for (int i = 1; i <= this.Avatar.ItemLevels.GetCount(LogicItems.PlunderThunder.GlobalID); i++)
                {
                    percentage += increment;
                    increment  += 0.01;
                }

                return percentage;
            }
        }

        /// <summary>
        /// Determines whether the item with the specified identifier is attached to any of the heroes in the player's loadout. 
        /// </summary>
        internal bool IsAttached(LogicItemsData itemData)
        {
            return this.Avatar.ItemAttachedTo.Values.Any(item => item.Id.Equals(this.Avatar.ItemLevels.Get(itemData.GlobalID).Id) && this.Avatar.Team.Any(hero => hero.ToObject<int>().Equals(item.Count)));
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
                
                foreach (var slot in this.Avatar.ItemLevels.Values)
                {
                    array.Add(slot.Save());
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
                
                foreach (var hero in this.Avatar.HeroLevels.Values)
                {
                    if (this.Avatar.ItemAttachedTo.Get(hero.Id) != null)
                    {
                        LogicJSONObject json = new LogicJSONObject();

                        json.Put("id", new LogicJSONNumber(hero.Id));
                        json.Put("cnt", new LogicJSONNumber(this.Avatar.ItemAttachedTo.Get(hero.Id).Id));

                        array.Add(json);
                    }
                }

                return array;
            }
        }
    }
}
