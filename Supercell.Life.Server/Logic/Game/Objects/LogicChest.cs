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
                this.Avatar.CommodityChangeCountHelper(LogicCommodityType.Gold, value);
            }
        }

        /// <summary>
        /// Adds to the player's experience.
        /// </summary>
        private int XP
        {
            set
            {
                this.Avatar.CommodityChangeCountHelper(LogicCommodityType.Experience, value);
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

        // ------------------------------------------------
        // Everything below here is stuff I've cross-referenced in IDA relating to the loot calculation for the map chest and the ship reward chest.
        // This is more of a just a reference for me so I can see what I'm working with.
        // Most of it is raw IDA pseudocode, and while I made some syntactical changes to the functions below, it's not enough to make it run without error.
        // ------------------------------------------------

        /* private int sub_BE278(int a1, int a2)
        {
            int v2;           // r2
            int v3;           // r12
            int v4;           // r9
            int v5;           // r2
            int v6;           // r3
            int result;       // r0

            v2 = *(int**)(a1 + 116);
            v3 = v2[2];
            if (v3 < 1)
                result = -1;

            v4 = *v2;
            v5 = 0;

            while (true)
            {
                v6 = *(_DWORD*)(v4 + 4 * v5);

                if (*(_DWORD*)(v6 + 4) == a2)
                    break;

                if (++v5 >= v3)
                    result = -1
            }

            if (v5 == -1)
                result = -1;
            else
                result = *(_DWORD*)(v6 + 8);

            return result;
        } */

        /* private int sub_197A18(int a1, int a2)
        {
            int v3;        // r2
            int v4;        // r2
            int v5;        // r2
            int v6;        // r0

            if (a2 < 1)
                return 0;

            v3 = a1;

            v4 = v3 ^ (v3 << 13) ^ ((v3 ^ (v3 << 13)) >> 17);
            v5 = v4 ^ 32 * v4;

            if (v5 <= -1)
                v6 = -v5;
            else
                v6 = v5;

            return v6 % a2;
        } */

        /* private int sub_BE120(int a1, int a2)
        {
            int v2;            // r2
            int v3;            // r12
            int v4;            // r9
            int v5;            // r2
            int v6;            // r3
            int result;        // r0

            v2 = *(_DWORD*)(a1 + 100);
            v3 = *(_DWORD*)(v2 + 8);

            if (v3 < 1)
                result = -1;

            v4 = *(_DWORD*)v2;
            v5 = 0;

            while (true)
            {
                v6 = *(_DWORD*)(v4 + 4 * v5);

                if (*(_DWORD*)(v6 + 4) == a2)
                    break;

                if (++v5 >= v3)
                    result = -1;
            }

            if (v5 == -1)
                result = -1;
            else
                result = *(_DWORD*)(v6 + 8);

            return result;
        } */

        /* private int sub_11017C(int a1, int a2)
        {
            long v2; // d8
            long v3; // d9
            long v4; // d10
            long v5; // d11
            long v6; // d12
            long v7; // d13
            long v8; // d14
            long v9; // d15
            _QWORD* v10; // r4
            _QWORD* v11; // r4
            int v12; // r5
            int v13; // r8
            int v14; // r4
            int v15; // r6
            bool v16; // nf
            byte v17; // vf
            int v18; // r6
            int v19; // r11
            int v20; // r6
            int v21; // r0
            int v22; // r1
            int v23; // r0
            int v24; // r4
            int(__fastcall * v25)(int, int); // r2
            int v26; // r0
            int v27; // r1
            uint v28; // r0
            int v29; // r0
            int v30; // r0
            uint v31; // r0
            int v33; // [sp+0h] [bp-B8h]
            int v34; // [sp+8h] [bp-B0h]
            int v35; // [sp+Ch] [bp-ACh]
            int v36; // [sp+10h] [bp-A8h]
            char v37; // [sp+14h] [bp-A4h]

            int v39; // [sp+50h] [bp-68h]
            int v40; // [sp+54h] [bp-64h]
            char v41; // [sp+60h] [bp-58h]
            long savedregs; // [sp+B8h] [bp+0h]

            v10 = (_QWORD*)((uint)&v41 & 0xFFFFFFF0);
            *v10 = v2;
            v10[1] = v3;
            v10[2] = v4;
            v10[3] = v5;
            v11 = (_QWORD*)(((uint)&v41 & 0xFFFFFFF0) + 32);
            *v11 = v6;
            v11[1] = v7;
            v11[2] = v8;
            v11[3] = v9;
            v12 = a1;
            v13 = a2;
            v14 = *(_DWORD*)(a1 + 44);
            v15 = *(_DWORD*)(v14 + 8);

            v40 = &v33;
            v39 = (0x13E | 1) + 1114542;
            v17 = __OFSUB__(v15, 1);
            v16 = v15 - 1 < 0;
            v18 = v13;

            if (!(v16 ^ v17))
            {
                v19 = 0;
                v20 = 0;
                do
                {
                    v21 = *(_DWORD*)(*(_DWORD*)v14 + 4 * v20);
                    if (v21)
                    {
                        v22 = *(_DWORD*)(v21 + 4);
                        fctx.call_site = -1;
                        v23 = sub_BE40C(v12, v22);
                        v24 = v23;
                        if (v23)
                        {
                            if (*(_DWORD*)(v23 + 112) == 7)
                            {
                                v25 = *(int(__fastcall * *)(int, int))(*(_DWORD*)v12 + 60);
                                fctx.call_site = -1;
                                v26 = v25(v12, v23);
                                v27 = *(_DWORD*)(v24 + 116);
                                fctx.call_site = -1;
                                v19 = *(_DWORD*)(**(_DWORD**)(v24 + 128) + 4 * LogicMath::clamp(v26, 0, v27 - 1));
                            }
                        }
                    }
                    v14 = *(_DWORD*)(v12 + 44); // sub_BE120()
                    ++v20;
                }
                while (v20 < *(_DWORD*)(v14 + 8));

                v18 = v13;

                if (v19)
                {
                    v35 = v12;
                    v36 = v13;
                    fctx.call_site = -1;
                    String::format((int)&v37, "item xp boost %d", v19);
                    fctx.call_site = 1;
                    v34 = v19;
                    Debugger::hudPrint((int)&v37, -1);
                    String::~String((int)&v37);
                    v13 = v36;
                    v12 = v35;
                    v28 = (unsigned __int64) (1374389535LL * (v34 + 100) * v36) >> 32;
                    v18 = ((signed int)v28 >> 5) +(v28 >> 31);
                }
            }

            v29 = *(_DWORD*)(v12 + 280);

            if (v29)
            {
                v30 = *(_DWORD*)(v29 + 4);
                fctx.call_site = -1;
                if (LogicMath::max(0, v30 / 15) >= 1)
                {
                    v31 = (unsigned __int64) (1374389535LL * (*(_DWORD*)(v12 + 284) + 100) * v13) >> 32;
                    v18 = ((signed int)v31 >> 5) +(v31 >> 31);
                }
            }

            return v18;
        } */
    }
}
