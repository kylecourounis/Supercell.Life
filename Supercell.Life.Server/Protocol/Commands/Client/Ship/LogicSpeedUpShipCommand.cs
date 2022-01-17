namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Enums;
    using Supercell.Life.Server.Logic.Game;
    using Supercell.Life.Server.Network;

    internal class LogicSpeedUpShipCommand : LogicCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicSpeedUpShipCommand"/> class.
        /// </summary>
        public LogicSpeedUpShipCommand(Connection connection) : base(connection)
        {
            // LogicSpeedUpShipCommand.
        }

        internal override void Decode(ByteStream stream)
        {
            base.Decode(stream);
        }
        
        internal override void Execute(LogicGameMode gamemode)
        {
            int cost = LogicGamePlayUtil.GetSpeedUpCost(gamemode.Avatar.Sailing.Timer.RemainingSecs, 0);
            
            if (gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Diamonds, -cost))
            {
                gamemode.Avatar.Sailing.Finish();
            }
        }

        /**
         * Below is some stuff that I am working on.
         * This has a slightly different calculation for the speed up cost.
         * A lot of it is inaccurate.
         */
        private int CalculateSpeedUpCost(LogicGameMode gamemode) //sub_123974
        {
            var v12 = this.sub_B0C64(gamemode) / 100;
            var v14 = 0;
            var v15 = 0;

            if (gamemode.Avatar.Sailing.Heroes != null)
            {
                v14 = (int)(-2004318071L * (gamemode.Avatar.Sailing.Heroes.Count >> 32)) + gamemode.Avatar.Sailing.Heroes.Count;
                // v14 = gamemode.Avatar.Sailing.Heroes.Count;
                v15 = 100 * LogicMath.Max(0, (int)((v14 >> 3) + (v14 >> 31)));
            }
            else
            {
                v15 = 0;
            }

            var v16 = Globals.ShipSpeedUpResourceCost100;
            var v17 = v16 * LogicMath.Max(v12, 1);
            var v18 = (ulong)(1374389535L * LogicMath.Clamp(v15 / (3600 * Globals.ShipSailDurationHours), 1, 100) * v17) >> 32;
            
            var v19 = (v18 >> 5) + (v18 >> 31);

            return (int)v19;
        }
        
        private int sub_B0C64(LogicGameMode gamemode)
        {
            int v2;
            int v3;
            int v4;
            int v5;
            int v6;
            int v7;
            int v8;
            int v9;
            int v10;

            int seasickDuration = Globals.ShipSeasickDurationHours;

            v4 = gamemode.Avatar.Sailing.Heroes.Count;
            if (v4 < 1)
                return 0;

            v5 = seasickDuration;
            v6 = 0;
            v7 = 0;
            v8 = Globals.ShipGoldPerHeroLevel[0];

            do
            {
                if (v7 < v5)
                {
                    v9 = LogicMath.Clamp(gamemode.Avatar.Sailing.Heroes.Count, 0, v8);

                    v5 = seasickDuration;

                    v10 = (int)((1374389535L * Globals.ShipGoldPerHeroLevel[0] + 4 * v9) * (v5 + 4 * v7)) >> 32;
                    v6 += (v10 >> 5) +(v10 >> 31);
                }
                ++v7;
            }
            while (v7 < v4);

            return v6;
        }
    }
}
