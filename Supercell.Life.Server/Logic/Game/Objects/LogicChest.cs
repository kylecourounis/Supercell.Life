namespace Supercell.Life.Server.Logic.Game.Objects
{
    using Supercell.Life.Server.Core;
    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Logic.Enums;
    
    internal class LogicChest
    {
        private readonly LogicClientAvatar Avatar;

        /// <summary>
        /// Adds to the player's gold.
        /// </summary>
        private int Gold
        {
            set
            {
                this.Avatar.AddGold(value);
            }
        }

        /// <summary>
        /// Adds to the player's experience.
        /// </summary>
        private int XP
        {
            set
            {
                this.Avatar.AddXP(value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicChest"/> class.
        /// </summary>
        internal LogicChest(LogicClientAvatar avatar)
        {
            this.Avatar = avatar;
        }

        /// <summary>
        /// Creates a map chest.
        /// </summary>
        internal void CreateMapChest()
        {
            LogicExperienceLevelData expLevelData = (LogicExperienceLevelData)CSV.Tables.Get(Gamefile.ExperienceLevels).GetDataWithID(this.Avatar.ExpLevel - 1);

            this.Gold = Loader.Random.Rand(expLevelData.MapChestMinGold, expLevelData.MapChestMaxGold);
            this.XP   = Loader.Random.Rand(expLevelData.MapChestMinXP, expLevelData.MapChestMaxXP);
        }

        /// <summary>
        /// Creates a level chest.
        /// </summary>
        internal void CreateLevelChest()
        {
            // TODO
        }

        /// <summary>
        /// Creates a mega chest.
        /// </summary>
        internal void CreateMegaChest()
        {
            // TODO
        }
    }
}
