namespace Supercell.Life.Server.Logic.Game.Objects
{
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Files;
    using Supercell.Life.Server.Files.CsvLogic;
    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Logic.Enums;

    internal class LogicChest
    {
        private readonly LogicClientAvatar Avatar;

        internal byte[] byte_2B415A = { 2, 0, 0, 0, 0, 0, 1, 0, 5, 0, 0, 2, 0, 0, 1, 0, 1 };

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicChest"/> class.
        /// </summary>
        internal LogicChest()
        {
            // LogicChest.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicChest"/> class.
        /// </summary>
        internal LogicChest(LogicClientAvatar avatar)
        {
            this.Avatar = avatar;
        }

        /// <summary>
        /// Creates the chest.
        /// </summary>
        internal void CreateChest(ChestType type)
        {
            int min;
            int max;

            switch (type)
            {
                case ChestType.Small:
                {
                    min = Globals.SmallChestModifierMin;
                    max = Globals.SmallChestModifierMax;
                    break;
                }
                case ChestType.Medium:
                {
                    min = Globals.MedChestModifierMin;
                    max = Globals.MedChestModifierMax;
                    break;
                }
                case ChestType.Big:
                {
                    min = Globals.BigChestModifierMin;
                    max = Globals.BigChestModifierMax;
                    break;
                }
                case ChestType.Multiplayer:
                {
                    min = Globals.MultiplayerChestModifier;
                    max = Globals.MultiplayerChestModifier;
                    break;
                }
                default:
                {
                    Debugger.Error("Invalid Chest Type");
                    min = 100;
                    max = 100;
                    break;
                }
            }

            int v101 = this.Avatar.Connection.GameMode.Random.Rand(max - min);

            var (modGold, modXP) = this.Generate(v101, min);

            Debugger.Debug($"CHEST: mod range: [{min} -> {max}] -> MOD GOLD: = {modGold} MOD XP: = {modXP}");
        }

        /// <summary>
        /// Creates a map chest.
        /// </summary>
        internal void CreateMapChest()
        {
            LogicExperienceLevelData expLevelData = (LogicExperienceLevelData)CSV.Tables.Get(Gamefile.ExperienceLevels).GetDataWithID(this.Avatar.ExpLevel - 1);

            var (goldSeed, xpSeed, modify) = this.GetSeeds();
            
            int gold = this.RandomWithSeed(goldSeed, expLevelData.MapChestMinGold, expLevelData.MapChestMaxGold);
            int xp   = this.RandomWithSeed(xpSeed, expLevelData.MapChestMinXP, expLevelData.MapChestMaxXP);

            switch (modify)
            {
                case CommodityType.Gold:
                {
                    gold += goldSeed / 2;
                    break;
                }
                case CommodityType.Experience:
                {
                    xp += LogicMath.Abs(xpSeed);
                    break;
                }
            }
            
            this.Avatar.CommodityChangeCountHelper(CommodityType.Gold, gold);
            this.Avatar.CommodityChangeCountHelper(CommodityType.Experience, xp);

            this.Avatar.MapChestTimer.Start();
        }

        /// <summary>
        /// Generates a pseudo-random number between the specified range with the specified seed.
        /// </summary>
        private int RandomWithSeed(int seed, int min, int max)
        {
            this.Avatar.Connection.GameMode.Random.SetIteratedRandomSeed(seed);
            return this.Avatar.Connection.GameMode.Random.Rand(min, max);
        }
        
        /// <summary>
        /// Gets the seeds for LogicRandom based on the number of map chests that have been opened.
        /// </summary>
        private (int, int, CommodityType) GetSeeds()
        {
            /*
             * Triple Structure:
             *   The first integer is the seed for the gold generation
             *   The second integer is the seed for the XP generation
             *   The CommodityType determines which one needs slight modifications after the numbers have been generated
             */

            switch (this.Avatar.Connection.GameMode.MapChestsOpened)
            {
                case 2:
                {
                    return (6, 2, CommodityType.Gold);
                }
                default:
                {
                    return (0, -2, CommodityType.Experience);
                }
            }
        }

        private (int, long) Generate(int rand, int modifier)
        {
            long v104 = (1717986919L * (rand + modifier)) >> 32;

            int v322  = 10 * ((rand + modifier) / 10);
            long v323 = 10 * (((int)v104 >> 2) + (v104 >> 31));

            return (v322, v323);
        }

        internal enum ChestType
        {
            Small       = 0,
            Medium      = 1,
            Big         = 2,
            Multiplayer = 3
        }
    }
}
