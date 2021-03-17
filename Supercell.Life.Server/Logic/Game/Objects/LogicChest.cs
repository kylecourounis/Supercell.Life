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

        internal byte[] byte_2B415A = { 2, 0, 0, 0, 0, 0, 1, 0, 5, 0, 0, 2, 0, 0, 1, 0, 1 };

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

            var v40 = this.byte_2B415A[1 % 50 % 50];

            var gold = Loader.Random.Rand(expLevelData.MapChestMinGold, expLevelData.MapChestMaxGold);
            var xp   = Loader.Random.Rand(expLevelData.MapChestMinXP, expLevelData.MapChestMaxXP);

            this.Gold = gold;
            this.XP   = xp;
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

        //internal static int sub_197A18(LogicClientAvatar a1, int a2)
        //{
        //    LogicClientAvatar v3;        // r2
        //    int v4;        // r2
        //    int v5; // r2
        //    int v6;        // r0

        //    if (a2 < 1)
        //        return 0;

        //    v3 = a1;
        //    if (a1 == null)
        //        v3 = null;

        //    v4 = v3 ^ (v3 << 13) ^ ((v3 ^ (v3 << 13)) >> 17);
        //    v5 = v4 ^ 32 * v4;
        //    a1 = v5; 

        //    if (v5 <= -1)
        //        v6 = -v5;
        //    else
        //        v6 = v5;

        //    return v6 % a2;
        //}
    }
}
