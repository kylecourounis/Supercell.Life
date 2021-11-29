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

                goto LABEL_11;
            }

            int v8 = v7 << 15;

            if (v6 - v8 <= -1)
            {
                v9 = (v8 - v6) >> 15;
                v4 = v9;
                v1 = v5;
                v2 = 0;
                v3 = 256;

                goto LABEL_11;
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

        LABEL_11:
            int v13 = v9 + 1;
            v4 = v13;

            Debugger.Debug($"{v1}, {v2}, {v3}, {v4}");

            if (v13 <= -1)
                Debugger.Warning("0 distance to level edge in collideWithLevel");

            return 1;
        }
        
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
                        // TODO
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
