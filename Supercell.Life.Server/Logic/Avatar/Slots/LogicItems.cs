namespace Supercell.Life.Server.Logic.Avatar.Slots
{
    using Supercell.Life.Titan.Logic.Json;

    using Supercell.Life.Server.Logic.Game.Objects.Items;

    internal class LogicItems
    {
        internal readonly LogicClientAvatar Avatar;
        
        #region ItemsData
        internal LogicItem EnergyRecycler;
        internal LogicItem PlunderThunder;
        internal LogicItem RobbersHood;
        internal LogicItem LuckyFix;
        internal LogicItem CaptainsBounce;
        internal LogicItem GalaxyDefender;
        internal LogicItem MegaFlex;
        internal LogicItem PowerProtector;
        internal LogicItem ComboCruncher;
        internal LogicItem AdventurersSnap;
        internal LogicItem SmackAttack;
        internal LogicItem BubbleArmor;
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicItems"/> class.
        /// </summary>
        internal LogicItems(LogicClientAvatar avatar)
        {
            this.Avatar = avatar;
            this.Initialize();
        }

        private void Initialize()
        {
            this.EnergyRecycler  = new LogicItem(this.Avatar, "GetEnergy");
            this.PlunderThunder  = new LogicItem(this.Avatar, "GetXP");
            this.RobbersHood     = new LogicItem(this.Avatar, "GetGold");
            this.LuckyFix        = new LogicItem(this.Avatar, "BarrelOdds");
            this.CaptainsBounce  = new LogicItem(this.Avatar, "PlusCombo");
            this.GalaxyDefender  = new LogicItem(this.Avatar, "HPPlus");
            this.MegaFlex        = new LogicItem(this.Avatar, "SpecialAttackPlus");
            this.PowerProtector  = new LogicItem(this.Avatar, "GetHpEnemyCombo");
            this.ComboCruncher   = new LogicItem(this.Avatar, "StartCombo");
            this.AdventurersSnap = new LogicItem(this.Avatar, "DoubleDmg");
            this.SmackAttack     = new LogicItem(this.Avatar, "DmgPlus");
            this.BubbleArmor     = new LogicItem(this.Avatar, "Shield");
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
                    array.Add(slot.SaveToJSON());
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
