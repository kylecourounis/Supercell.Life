namespace Supercell.Life.Server.Logic.Avatar.Slots
{
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Logic.Game;

    internal class LogicResources : LogicDataSlots
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicResources"/> class.
        /// </summary>
        internal LogicResources(LogicClientAvatar avatar) : base(avatar)
        {
            // LogicResources.
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        internal override void Initialize()
        {
            this.SetResourceCount(CommodityType.Gold, Globals.StartingGold);
            this.SetResourceCount(CommodityType.Energy, Globals.InitialMaxEnergy);

            this.SetResourceCount(CommodityType.Orb1, 1000000);
            this.SetResourceCount(CommodityType.Orb2, 1000000);
            this.SetResourceCount(CommodityType.Orb3, 1000000);
            this.SetResourceCount(CommodityType.Orb4, 1000000);
        }

        /// <summary>
        /// Sets the count for the specified resource.
        /// </summary>
        internal void SetResourceCount(CommodityType resource, int count)
        {
            if (resource != CommodityType.Diamonds)
            {
                this.Set((int)resource, count);
            }
            else
            {
                Debugger.Warning("SetResourceCount() shouldn't be used for diamonds.");
            }
        }

        /// <summary>
        /// Gets the count of the specified resource.
        /// </summary>
        internal int GetResourceCount(CommodityType resource)
        {
            if (resource != CommodityType.Diamonds)
            {
                return this.GetCount((int)resource);
            }

            Debugger.Warning("GetResourceCount() shouldn't be used for diamonds.");

            return 0;
        }
    }
}