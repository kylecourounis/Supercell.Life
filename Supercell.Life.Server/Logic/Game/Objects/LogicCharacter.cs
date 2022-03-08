namespace Supercell.Life.Server.Logic.Game.Objects
{
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Logic.Enums;

    internal class LogicCharacter
    {
        internal LogicHeroData HeroData;

        internal int Level;

        internal int Hitpoints;

        internal int Damage;

        internal int SpecialAttackDamage;

        internal int Bounces;

        internal LogicVector2 Position;

        /// <summary>
        /// Gets the global identifier.
        /// </summary>
        internal int GlobalID
        {
            get
            {
                return this.HeroData.GlobalID;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicCharacter"/> class.
        /// </summary>
        internal LogicCharacter(LogicClientAvatar avatar, int globalID)
        {
            this.HeroData = (LogicHeroData)CSV.Tables.Get(Gamefile.Heroes).GetDataWithID(globalID);
            this.Level    = avatar.HeroLevels.GetCount(this.GlobalID);
            this.Position = new LogicVector2();

            this.Initialize();
        }

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        private void Initialize()
        {
            this.Hitpoints           = this.HeroData.Hitpoints[this.Level];
            this.Damage              = this.HeroData.Damage[this.Level];
            this.SpecialAttackDamage = this.HeroData.SpecialAttackDamage[this.Level];
        }

        /// <summary>
        /// Subtracts hitpoints from this <see cref="LogicCharacter"/>.
        /// </summary>
        internal void HitCharacter(int damage)
        {
            if (this.Hitpoints - damage > 0)
            {
                this.Hitpoints -= damage;
            }
        }
    }
}
