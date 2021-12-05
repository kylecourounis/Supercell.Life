namespace Supercell.Life.Server.Logic.Game.Objects.Quests
{
    using Supercell.Life.Titan.Logic.Math;

    internal class LogicCollisionHelper
    {
        /**
         * This file is by no means complete.
         * There is still a load of stuff to implement in this file. 
         */

        /// <summary>
        /// Checks whether the hero collides with the edge of the map.
        /// </summary>
        internal static int CollideWithLevel(int a1, int a2, int a3, int a4)
        {
            int v1;
            int v2 = -1;
            long v3;
            LogicLong v4;

            int v5 = a2;
            int v6 = a3;
            int v7 = v2;

            int v9;

            if (v5 - (v7 << 15) <= -1)
            {
                v9 = ((v7 << 15) - v5) >> 15;
                v4 = v9;
                v1 = 0;
                v2 = v6;
                v3 = 256;

                goto Result;
            }

            int v8 = v7 << 15;

            if (v6 - v8 <= -1)
            {
                v9 = (v8 - v6) >> 15;
                v4 = v9;
                v1 = v5;
                v2 = 0;
                v3 = 256;

                goto Result;
            }
            if (v8 + v5 <= 16056320)
            {
                int v11 = v8 + v6;

                if (v11 <= 25559040)
                    return 0;

                v9 = (v11 - 25559040) >> 15;

                v4 = v9;
                v1 = v5;
                v3 = 25559040;
                v2 = -256;
            }

            v9 = (v8 + v5 - 16056320) >> 15;

            v4 = v9;
            v3 = 16056320;
            v1 = v6;
            v2 = -256;

        Result:
            int v13 = v9 + 1;
            v4 = v13;

            Debugger.Debug($"{v1}, {v2}, {v3}, {v4}");

            if (v13 <= -1)
                Debugger.Warning("0 distance to level edge in collideWithLevel");

            return 1;
        }

        /* internal static int sub_E6BC8(int a1, int* a2)
        {
            int v2;            // r4
            int* v3;           // r10
            signed int v4;     // r6
            signed int v5;     // r5
            signed __int64 v6; // r0
            int v7;            // r8
            int* v8;           // r0
            signed int v9;     // r1
            _DWORD* v10;       // r2
            int v11;           // t1
            int result;        // r0

            v2 = a1;
            v3 = a2;
            v4 = *(_DWORD*)(a1 + 8);
            if (v4 == *(_DWORD*)(a1 + 4))
            {
                v5 = 2 * v4;
                if (!(2 * v4))
                    v5 = 5;
                if (v4 < v5)
                {
                    v6 = 4LL * (unsigned int)v5;
                    if (!is_mul_ok(4u, v5))
                        HIDWORD(v6) = 1;
                    if (HIDWORD(v6))
                        LODWORD(v6) = -1;
                    v7 = operator new[](v6);
                    v8 = *(int**)v2;
                    if (v4 >= 1)
                    {
                        v9 = v4;
                        v10 = (_DWORD*)v7;
                        do
                        {
                            v11 = *v8;
                            ++v8;
                            --v9;
                            *v10 = v11;
                            ++v10;
                        }
                        while (v9 > 0);
                        v8 = *(int**)v2;
                    }
                    if (v8)
                    {
                        operator delete[](v8);
                        v4 = *(_DWORD*)(v2 + 8);
                    }
                    *(_DWORD*)v2 = v7;
                    *(_DWORD*)(v2 + 4) = v5;
                }
            }
            result = *v3;
            *(_DWORD*)(v2 + 8) = v4 + 1;
            *(_DWORD*)(*(_DWORD*)v2 + 4 * v4) = result;
            return result;
        }

        internal static int sub_E5820(_DWORD* a1, int a2, int a3, _DWORD** a4, int a5, int a6, int a7, int a8, _DWORD** a9)
        {
            _DWORD** v9; // r8
            _DWORD* v10; // r4
            int v11; // r6
            int v12; // r11
            int v13; // r0
            int v14; // r5
            int v15; // r1
            bool v16; // zf
            bool v17; // zf
            _DWORD** v18; // r0
            int v19; // r10
            int v20; // r2
            int v21; // r4
            int v22; // r0
            int v23; // r5
            int v24; // r2
            int v25; // r4
            int v26; // r8
            signed int v27; // r0
            int v28; // r0
            int v29; // r10
            signed int v30; // r5
            int v31; // r0
            _DWORD* v32; // r4
            int v33; // r6
            int v34; // r0
            signed int v35; // r0
            int v36; // r8
            __int64 v37; // r0
            int v38; // r0
            int v39; // r10
            int v40; // r11
            int v41; // r0
            unsigned __int64 v42; // r0
            int v43; // r6
            signed int result; // r0
            signed int v45; // r0
            signed int v46; // r2
            int v47; // r9
            signed int v48; // r6
            signed int v49; // r3
            bool v50; // zf
            int v51; // r2
            _DWORD** v52; // r4
            int v53; // r3
            int v54; // r0
            int v55; // r5
            int v56; // r9
            int v57; // r11
            int v58; // r6
            int v59; // r11
            int v60; // r6
            int v61; // r5
            int v62; // r5
            int v63; // r0
            int v64; // r4
            int v65; // r0
            int v66; // r10
            signed int v67; // r6
            int v68; // r5
            int v69; // r4
            int v70; // r0
            int v71; // r6
            int v72; // r8
            int v73; // r0
            int v74; // r2
            _DWORD* v75; // r4
            int v76; // r4
            int v77; // [sp+0h] [bp-38h]
            int v78; // [sp+4h] [bp-34h]
            int v79; // [sp+8h] [bp-30h]
            int v80; // [sp+Ch] [bp-2Ch]
            int v81; // [sp+Ch] [bp-2Ch]
            int v82; // [sp+10h] [bp-28h]
            int v83; // [sp+10h] [bp-28h]
            _DWORD** v84; // [sp+14h] [bp-24h]
            int v85; // [sp+14h] [bp-24h]
            _DWORD* v86; // [sp+18h] [bp-20h]
            int v87; // [sp+18h] [bp-20h]
            int v88; // [sp+18h] [bp-20h]
            int v89; // [sp+1Ch] [bp-1Ch]
            int v90; // [sp+1Ch] [bp-1Ch]

            v9 = a4;
            v10 = a1;
            v11 = a2;
            v89 = a3;
            v12 = ((int(__fastcall *)(_DWORD * *))(*a4)[3])(a4);
            v13 = ((int(__fastcall *)(_DWORD * *))(*a9)[3])(a9);
            v14 = a7;
            v15 = v13;
            v16 = v12 == 0;
            if (!v12)
                v16 = v13 == 1;
            if (v16)
                goto LABEL_89;
            v17 = v12 == 1;
            if (v12 == 1)
                v17 = v13 == 0;
            if (v17)
            {
                LABEL_89:
                v18 = a9;
                v86 = v10;
                if (!v12)
                    v18 = v9;
                v84 = v18;
                v19 = ((int(*)(void))(*v18)[4])();
                v20 = v11;
                v21 = a7;
                v79 = v19;
                if (!v12)
                    v9 = a9;
                if (!v12)
                    v20 = a7;
                if (!v12)
                    v21 = v11;
                v80 = v21;
                v22 = LogicMath::clamp(v21, v20 + (*v9[1] << 15), v20 + (*v9[2] << 15));
                v23 = a8;
                v24 = v89;
                if (!v12)
                    v24 = a8;
                if (!v12)
                    v23 = v89;
                v25 = (v22 - v21) >> 15;
                v26 = (LogicMath::clamp(v23, v24 + (v9[1][1] << 15), v24 + (v9[2][1] << 15)) - v23) >> 15;
                v27 = v25 * v25 + v26 * v26;
                if (v27 >= v19 * v19)
                    return 0;
                v28 = LogicMath::sqrt(v27);
                v29 = v28;
                if (v28)
                {
                    v90 = v23;
                    v30 = 0;
                    v31 = -256 * v25 / v28;
                    v32 = v86;
                    v33 = v31;
                    v82 = v29;
                    v86[5] = v31;
                    v34 = -256 * v26 / v29;
                    v78 = -256 * v26 / v29;
                    v86[6] = v34;
                    v16 = v34 == 0;
                    v35 = 0;
                    if (v16)
                        v35 = 1;
                    if (!v33)
                        v30 = 1;
                    v77 = v35 | v30;
                    if (!(v35 | v30))
                    {
                        v36 = LogicMath::abs(v33);
                        if (v36 <= LogicMath::abs(v86[6]))
                        {
                            v86[5] = 0;
                            v86[6] = ((v86[6] >> 31) & 0xFFFFFE02) + 255;
                        }
                        else
                        {
                            v37 = (v86[5] >> 31) & 0xFFFFFE02;
                            LODWORD(v37) = v37 + 255;
                            *(_QWORD*)(v86 + 5) = v37;
                        }
                    }
                    v39 = a6 >> 15;
                    v40 = a5 >> 15;
                    v41 = LogicMath::sqrt(v40 * v40 + v39 * v39);
                    if (v41)
                    {
                        v40 = (v40 << 8) / v41;
                        v39 = (v39 << 8) / v41;
                    }
                    v42 = *(_QWORD*)(v86 + 5);
                    if ((signed int)v42* v40 +HIDWORD(v42) * v39 >= 1 )
      {
                        v86[5] = v33;
                        v86[6] = v78;
                        if (v77)
                        {
                            v42 = __PAIR__(v78, v33);
                        }
                        else
                        {
                            v43 = LogicMath::abs(v33);
                            if (v43 <= LogicMath::abs(v86[6]))
                            {
                                v42 = (v86[5] >> 31) & 0xFFFFFE02;
                                LODWORD(v42) = v42 + 255;
                                *(_QWORD*)(v86 + 5) = v42;
                            }
                            else
                            {
                                LODWORD(v42) = 0;
                                v86[5] = 0;
                                HIDWORD(v42) = ((v86[6] >> 31) & 0xFFFFFE02) + 255;
                                v86[6] = HIDWORD(v42);
                            }
                        }
                    }
                    LODWORD(v42) = v80 - (v79 << 7) * v42;
                    HIDWORD(v42) = v90 - (v79 << 7) * HIDWORD(v42);
                    *(_QWORD*)(v86 + 3) = v42;
                    v38 = ((int(*)(void))(*v84)[4])() + 1 - v82;
                }
                else
                {
                    v32 = v86;
                    v86[3] = v11;
                    v86[4] = v89;
                    v86[5] = 255;
                    v86[6] = 0;
                    v38 = ((int(*)(void))(*v84)[4])();
                }
                v32[7] = v38;
                result = 1;
            }
            else
            {
                v45 = 0;
                v46 = 0;
                v47 = v11;
                v48 = 0;
                v49 = 0;
                if (v15 == 2)
                    v45 = 1;
                if (!v12)
                    v46 = 1;
                if (!v15)
                    v48 = 1;
                if (v12 == 2)
                    v49 = 1;
                if (v49 & v48 || (v45 & v46) == 1)
                {
                    v51 = v47;
                    v87 = (int)v10;
                    v52 = a9;
                    v53 = v47;
                    if (v15 == 2)
                    {
                        v53 = a7;
                        v14 = v47;
                    }
                    v54 = v14 - v53;
                    v55 = a8;
                    v56 = v54 >> 15;
                    v57 = v89;
                    if (v15 == 2)
                        v57 = a8;
                    if (v15 == 2)
                        v55 = v89;
                    if (v15 == 2)
                    {
                        v52 = v9;
                        v9 = a9;
                    }
                    v58 = (v55 - v57) >> 15;
                    switch ((int)v9[2] )
                    {
                        case 0u: 
                            if ((v56 | v58) < 0)
                                return 0;
                            goto def_E5B04;
                        case 1u:
                            result = 0;
                            if (v56 > 0 || v58 < 0)
                                return result;
                            goto def_E5B04;
                        case 2u:
                            result = 0;
                            if (v56 > 0)
                              return result;
                            goto LABEL_78;
                        case 3u:
                            result = 0;
                        if (v56 < 0)
                            return result;
                        LABEL_78:
                        if (v58 <= 0)
                            goto def_E5B04;
                        return result;
                        default:
                    def_E5B04:
                        v81 = (v55 - v57) >> 15;
                        v85 = v53;
                        v83 = v51;
                        v67 = v58 * v58 + v56 * v56;
                        v68 = ((int(__fastcall *)(_DWORD * *))(*v9)[4])(v9);
                        v69 = ((int(__fastcall *)(_DWORD * *))(*v52)[4])(v52);
                        if (v67 - (v68 - v69) * (v68 - v69) < 1)
                            return 0;
                        v70 = LogicMath::sqrt(v67);
                        v71 = v70;
                        if (v70)
                        {
                            v72 = -256 * v56 / v70;
                            *(_DWORD*)(v87 + 20) = v72;
                            v73 = -256 * v81 / v70;
                            *(_DWORD*)(v87 + 24) = v73;
                            *(_DWORD*)(v87 + 12) = v85 - (v68 << 7) * v72;
                            *(_DWORD*)(v87 + 16) = v57 - (v68 << 7) * v73;
                            *(_DWORD*)(v87 + 28) = 1 - v68 + v69 + v71;
                        }
                        else
                        {
                            v74 = v68 + 1 - v69;
                            *(_DWORD*)(v87 + 12) = v83;
                            v75 = (_DWORD*)(v87 + 20);
                            *(_DWORD*)(v87 + 16) = v89;
                            *v75 = 255;
                            v75[1] = 0;
                            v75[2] = v74;
                            Debugger::hudPrint((int)"INV CIRCLE BUG", -1);
                        }
                        result = 1;
                        break;
                    }
                }
                else
                {
                    if (v15 | v12)
                    {
                        v50 = v12 == 1;
                        result = 0;
                        if (v12 == 1)
                            v50 = v15 == 1;
                        if (!v50)
                            return result;
                        Debugger::warning((int)"AABB -> AABB collisions are not implemented");
                        return 0;
                    }
                    v88 = (int)v10;
                    v59 = (v89 - a8) >> 15;
                    v60 = (v47 - a7) >> 15;
                    v61 = ((int(__fastcall *)(_DWORD * *))(*v9)[4])(v9);
                    v62 = v61 + ((int(__fastcall *)(_DWORD * *))(*a9)[4])(a9);
                    if (v60 * v60 + v59 * v59 >= v62 * v62)
                        return 0;
                    v63 = LogicMath::sqrt(v60 * v60 + v59 * v59);
                    v64 = v63;
                    if (v63)
                    {
                        v65 = (v60 << 8) / v63;
                        *(_DWORD*)(v88 + 20) = v65;
                        *(_DWORD*)(v88 + 24) = (v59 << 8) / v64;
                        *(_DWORD*)(v88 + 12) = v47 - (v65 * ((int(__fastcall *)(_DWORD * *))(*v9)[4])(v9) << 7);
                        v66 = *(_DWORD*)(v88 + 24);
                        *(_DWORD*)(v88 + 16) = v89 - (v66 * ((int(__fastcall *)(_DWORD * *))(*v9)[4])(v9) << 7);
                        *(_DWORD*)(v88 + 28) = v62 + 1 - v64;
                    }
                    else
                    {
                        *(_DWORD*)(v88 + 12) = v47;
                        v76 = v88 + 20;
                        *(_DWORD*)(v88 + 16) = v89;
                        *(_DWORD*)v76 = 255;
                        *(_DWORD*)(v76 + 4) = 0;
                        *(_DWORD*)(v76 + 8) = v62;
                    }
                    result = 1;
                }
            }
            return result;
        }

        private static int sub_E5D6C(int a1, int a2, int a3, int a4, int a5, int a6)
        {
            int v6; // r4
            int v7; // r6
            int v8; // r8
            int v9; // r5
            _DWORD* v10; // r0
            _QWORD* v11; // r1
            __int64 v12; // d17
            int result; // r0
            int v14; // r2
            char v15; // r3
            int v16; // r3
            int* v17; // r0
            int v18; // r6
            _QWORD* v19; // r1
            int v20; // r3
            __int64 v21; // r2
            int v22; // ST00_4
            int v23; // ST00_4
            _DWORD* v24; // [sp+1Ch] [bp-1Ch]

            v6 = a1;
            v7 = a4;
            v8 = a3;
            v9 = a2;
            v10 = (_DWORD*)operator new (0x24u);
            v24 = v10;
            *v10 = &off_355C6C;
            v11 = v10 + 1;
            *(_DWORD*)((char*)v10 + 21) = 0;
            *(_DWORD*)((char*)v10 + 17) = 0;
            *v11 = 0LL;
            v11[1] = 0LL;
            v12 = *(_QWORD*)(v6 + 20);
            *v11 = *(_QWORD*)(v6 + 12);
            v11[1] = v12;
            v10[5] = *(_DWORD*)(v6 + 28);
            v10[7] = v9;
            v10[8] = 0;
            sub_E6BC8(*(_DWORD*)(v6 + 48), (int*)&v24);
            result = a6;

            if (!a6)
            {
                v14 = *(_DWORD*)(v6 + 20);
                v15 = v8;
                if (v8)
                    v15 = 1;
                v16 = *(_DWORD*)(v6 + 28) << v15;
                v17 = *(int**)(v9 + 12);
                *v17 = v7 + (v14 * v16 << 7);
                v18 = *(_DWORD*)(v6 + 24);
                v17[1] = a5 + (v18 * v16 << 7);
                v19 = *(_QWORD**)(v9 + 16);
                v20 = v14 * ((signed int)*v19 >> 8) +((signed int)(*v19 >> 32) >> 8) *v18;
                LODWORD(v21) = *v19 - 2 * (v20 * v14 >> 8);
                HIDWORD(v21) = (*v19 >> 32) - 2 * (v18 * v20 >> 8);
                *v19 = v21;
                if (v8)
                {
                    if (sub_E5820(
                           (_DWORD*)v6,
                           *v17,
                           v17[1],
                           *(_DWORD***)(v9 + 24),
                           0,
                           0,
                           **(_DWORD**)(v8 + 12),
                           *(_DWORD*)(*(_DWORD*)(v8 + 12) + 4),
                           *(_DWORD***)(v8 + 24)) == 1)
                        Debugger::warning((int)"COLLISION AFTER PUSH OUT (WALL!)");
                }
                v22 = *(_DWORD*)(v6 + 24);
                (*(void(__cdecl * *)(_DWORD))(**(_DWORD**)(v9 + 4) + 148))(*(_DWORD*)(v9 + 4));
                v23 = *(_DWORD*)(v6 + 24);
                result = (*(int(__cdecl * *)(_DWORD))(**(_DWORD**)(*(_DWORD*)(v9 + 4) + 12) + 20))(*(_DWORD*)(*(_DWORD*)(v9 + 4) + 12));
            }

            return result;
        } */

        /// <summary>
        /// Some collision stuff.
        /// Still a lot to do here.
        /// </summary>
        internal static void Collision(int a1, int a2, int a3, int a4, int a5, int a6, int a7, int a8, int a9)
        {
            int v32 = LogicMath.Abs(a5);
            int v33 = LogicMath.Abs(a6);

            int v34 = LogicMath.Max(v32, v33);
            int v35 = LogicMath.Clamp(v34 >> 15, 1, 780);

            if (v35 >= 1)
            {
                int v38 = 0;

                while (true)
                {
                    int x = a3 + a5 / v35 * (v38 + 1);
                    int y = a4 + a6 / v35 * (v38 + 1);

                    if (LogicCollisionHelper.CollideWithLevel(a1, x, y, a2) == 1)
                    {
                        // LogicCollisionHelper.sub_E5D6C(a1, 0, 0, x, y, a7);
                    }

                    if (++v38 >= v35)
                    {
                        break;
                    }
                }
            }
        }
    }
}
