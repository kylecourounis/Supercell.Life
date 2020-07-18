namespace Supercell.Life.Server.Logic.Avatar.Slots
{
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Logic.Game;

    internal class LogicResources : LogicDataSlot
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
            this.Set(Resource.Gold, Globals.StartingGold);
            this.Set(Resource.Energy, 15);

            this.Set(Resource.Orb1, 1000000);
            this.Set(Resource.Orb2, 1000000);
            this.Set(Resource.Orb3, 1000000);
            this.Set(Resource.Orb4, 1000000);
        }

        /// <summary>
        /// Sets the count for the specified resource.
        /// </summary>
        internal void Set(Resource resource, int count)
        {
            this.Set((int)resource, count);
        }

        /// <summary>
        /// Gets the count of the specified resource.
        /// </summary>
        internal int Get(Resource resource)
        {
            return this.GetCount((int)resource);
        }
    }
}