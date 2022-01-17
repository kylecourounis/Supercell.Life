namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using System.Linq;

    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Avatar.Slots;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Logic.Game;
    using Supercell.Life.Server.Network;

    internal class LogicCollectShipRewardCommand : LogicCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicCollectShipRewardCommand"/> class.
        /// </summary>
        public LogicCollectShipRewardCommand(Connection connection) : base(connection)
        {
            // LogicCollectShipRewardCommand.
        }

        internal override void Decode(ByteStream stream)
        {
            base.Decode(stream);
        }

        internal override void Execute(LogicGameMode gamemode)
        {
            this.CalculateLoot();

            gamemode.Avatar.Variables.Remove(LogicVariables.SailRewardUnclaimed.GlobalID);
        }

        // ===================
        // Everything below is still a bit of a WIP
        // ===================

        private void CalculateLoot()
        {
            LogicGameMode gamemode = this.Connection.GameMode;

            int gold = 0;
            int xp   = 0;

            for (int i = 0; i < gamemode.Avatar.Sailing.HeroLevels.Select(pair => pair.Value.Count).OrderByDescending(x => x).ToList().Count; i++)
            {
                int level = gamemode.Avatar.Sailing.HeroLevels.Select(hero => gamemode.Avatar.HeroLevels.Get(hero.Value.Id)).Select(item => item.Count).ToArray()[i];

                switch (i)
                {
                    case 0:
                    {
                        gold += Globals.ShipGoldPerHeroLevel[level];
                        xp   += Globals.ShipXPPerHeroLevel[level];

                        break;
                    }
                    case 1:
                    {
                        gold += Globals.ShipGoldPerHeroLevel[level] / 2;
                        xp   += Globals.ShipXPPerHeroLevel[level] / 2;

                        break;
                    }
                    case 2:
                    {
                        gold += Globals.ShipGoldPerHeroLevel[level] / 2;
                        xp   += Globals.ShipXPPerHeroLevel[level] / 2;

                        break;
                    }
                }
            }

            double goldMultiplier = 1.0 + LogicCollectShipRewardCommand.MakeDouble(gamemode.Random.Rand(Globals.ShipGoldVariation));
            double xpMultiplier   = 1.0 + LogicCollectShipRewardCommand.MakeDouble(gamemode.Random.Rand(Globals.ShipXPVariation));

            Debugger.Debug($"{(int)(gold * goldMultiplier)}, {(int)(xp * xpMultiplier)}");

            gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Gold, (int)(gold * goldMultiplier));
            gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Experience, (int)(xp * xpMultiplier));
        }

        private static double MakeDouble(int randNum)
        {
            if (randNum < 10)
            {
                return double.Parse($"0.0{randNum}");
            }

            return double.Parse($"0.{randNum}");
        }
    }
}
