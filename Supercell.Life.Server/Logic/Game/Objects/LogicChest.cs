namespace Supercell.Life.Server.Logic.Game.Objects
{
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
            
            int v101 = this.Avatar.GameMode.Random.Rand(max - min);
            int v103 = this.Avatar.GameMode.Random.Rand(max - min);
            
            var (modGold, modXP) = this.Generate(v101, v103, min);

            Debugger.Debug($"CHEST: mod range: [{min} -> {max}] -> MOD GOLD: = {modGold} MOD XP: = {modXP}");
            Debugger.Debug($"{this.Avatar.Gold % modGold}, {this.Avatar.ExpPoints % modXP}");
        }

        /// <summary>
        /// Creates a map chest.
        /// </summary>
        internal void CreateMapChest()
        {
            LogicExperienceLevelData expLevelData = (LogicExperienceLevelData)CSV.Tables.Get(Gamefile.ExperienceLevels).GetDataWithID(this.Avatar.ExpLevel - 1);
            
            int gold = this.Avatar.GameMode.Random.Rand(expLevelData.MapChestMinGold, expLevelData.MapChestMaxGold);
            int xp   = this.Avatar.GameMode.Random.Rand(expLevelData.MapChestMinXP, expLevelData.MapChestMaxXP);
            
            Debugger.Debug($"{gold}, {xp}");
            
            this.Avatar.CommodityChangeCountHelper(CommodityType.Gold, gold);
            this.Avatar.CommodityChangeCountHelper(CommodityType.Experience, xp);

            this.Avatar.MapChestTimer.Start();
        }
        
        private (int, long) Generate(int a1, int a2, int modifier)
        {
            long v104 = (1717986919L * (a2 + modifier)) >> 32;

            int v322  = 10 * ((a1 + modifier) / 10);
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
