namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using System.Linq;

    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Avatar.Slots;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Logic.Game;
    using Supercell.Life.Server.Network;

    internal class LogicClaimShipRewardCommand : LogicCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicClaimShipRewardCommand"/> class.
        /// </summary>
        public LogicClaimShipRewardCommand(Connection connection) : base(connection)
        {
            // LogicClaimShipRewardCommand.
        }

        internal override void Decode(ByteStream stream)
        {
            base.Decode(stream);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            this.CalculateLoot();

            gamemode.Avatar.Variables.Remove(LogicVariables.SailRewardUnclaimed.GlobalID);

            gamemode.Avatar.Sailing.Heroes.Clear();
            gamemode.Avatar.Sailing.HeroLevels.Clear();
        }
        
        /// <summary>
        /// Calculates the loot based on the heroes that were sent out to sail.
        /// </summary>
        private void CalculateLoot()
        {
            LogicGameMode gamemode = this.Connection.GameMode;

            int gold = 0;
            int xp   = 0;

            int[] sailingLevels = gamemode.Avatar.Sailing.HeroLevels.OrderByDescending(pair => pair.Value.Count).Select(pair => pair.Value.Count).ToArray();

            for (int i = 0; i < sailingLevels.Length; i++)
            {
                int level = sailingLevels[i];
                
                switch (i)
                {
                    case 0:
                    {
                        gold += Globals.ShipGoldPerHeroLevel[level];
                        xp   += Globals.ShipXPPerHeroLevel[level];

                        break;
                    }
                    case 1:
                    case 2:
                    {
                        gold += Globals.ShipGoldPerHeroLevel[level] / 2;
                        xp   += Globals.ShipXPPerHeroLevel[level] / 2;

                        break;
                    }
                }
            }
            
            double goldMultiplier = 1.0 + LogicClaimShipRewardCommand.MakeDouble(gamemode.Random.Rand(Globals.ShipGoldVariation));
            double xpMultiplier   = 1.0 + LogicClaimShipRewardCommand.MakeDouble(gamemode.Random.Rand(Globals.ShipXPVariation));

            gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Gold, (int)(gold * goldMultiplier));
            gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Experience, (int)(xp * xpMultiplier));
        }

        /// <summary>
        /// Makes a double with the specified integer in the hundredths place if it's less than ten, or after the decimal point if it's greater than ten.
        /// </summary>
        private static double MakeDouble(int randNum)
        {
            return randNum < 10 ? double.Parse($"0.0{randNum}") : double.Parse($"0.{randNum}");
        }
    }
}
