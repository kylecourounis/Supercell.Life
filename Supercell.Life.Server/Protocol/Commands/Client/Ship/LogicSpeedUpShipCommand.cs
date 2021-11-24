namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Logic;
    using Supercell.Life.Server.Logic.Enums;
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
            var cost = 0;

            if (LogicMath.Max(0, 0) >= 1)
            {

            }

            gamemode.Avatar.CommodityChangeCountHelper(CommodityType.Diamonds, -cost);

            gamemode.Avatar.Sailing.Finish();
        }

        /* 
        private int CalculateSpeedUpCost() //sub_123974
        {
            var v12 = sub_B0C64(v11, (int*)v3) / 100;
            var v14 = 0;
            var v15 = 0;

            if (gamemode.Avatar.Sailing.Heroes != null)
            {
                v14 = (int)(-2004318071L * (gamemode.Avatar.Sailing.Heroes.Count >> 32)) + gamemode.Avatar.Sailing.Heroes.Count;
                v15 = 100 * LogicMath.Max(0, (int)((v14 >> 3) + (v14 >> 31)));
            }
            else
            {
                v15 = 0;
            }

            var v16 = 0;
            var v17 = v16 * LogicMath.Max(v12, 1);
            var v18 = (ulong)(1374389535L * LogicMath.Clamp(v15 / (3600 * gamemode.Avatar.Orb1), 1, 100));

            var v19 = (v18 >> 5) + (v18 >> 31);

            Debugger.Debug(v19);

            return (int)v19;
        }
        
        private int sub_B0C64(int a1, int* a2)
        {
            int v2;          // r4
            int v3;           // r8
            int v4;    // r0
            _DWORD* v5;       // r1
            int v6;           // r6
            int v7;    // r5
            int v8;           // r11
            int v9;           // r0
            int v10; // r0

            v2 = a2;
            v3 = a1;
            sub_B0BA4((int)a2);
            v4 = v2[2];
            if (v4 < 1)
                return 0;

            v5 = *(_DWORD**)(v3 + 404);
            v6 = 0;
            v7 = 0;
            v8 = *(_DWORD*)(*(_DWORD*)(v3 + 400) + 8) - 1;

            do
            {
                if (v7 < v5[2])
                {
                    v9 = LogicMath.Clamp(*(_DWORD*)(*v2 + 4 * v7), 0, v8);
                    v5 = *(_DWORD**)(v3 + 404);
                    v10 = (unsigned __int64)(1374389535LL * *(_DWORD*)(**(_DWORD**)(v3 + 400) + 4 * v9) * *(_DWORD*)(*v5 + 4 * v7)) >> 32;
                    v6 += ((signed int)v10 >> 5) +(v10 >> 31);
                    v4 = v2[2];
                }
                ++v7;
            }
            while (v7 < v4);

            return v6;
        }

        private int sub_B0BA4(int result)
        {
            int v1; // r9
            int v2; // r12
            int v3; // r2
            int v4; // lr
            int v5; // r3

            v1 = *(_DWORD*)(result + 8);
            if (v1 - 1 >= 1)
            {
                v2 = *(_DWORD*)result;
                v3 = 0;
                do
                {
                    v4 = *(_DWORD*)(v2 + 4 * v3);
                    v5 = *(_DWORD*)(v2 + 4 * (v3 + 1));
                    if (v4 < v5)
                    {
                        *(_DWORD*)(v2 + 4 * v3) = v5;
                        *(_DWORD*)(v2 + 4 * (v3 + 1)) = v4;
                        if (v3 > 0)
                            --v3;
                        v1 = *(_DWORD*)(result + 8);
                    }
                    ++v3;
                }
                while (v3 < v1 - 1);
            }

            return result;
        } */
    }
}
