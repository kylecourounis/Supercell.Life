namespace Supercell.Life.Titan.Logic.Math
{
    public static class LogicMath
    {
        private static readonly byte[] SqrtTable =
        {
            0x00, 0x10, 0x16, 0x1B, 0x20, 0x23, 0x27, 0x2A, 0x2D,
            0x30, 0x32, 0x35, 0x37, 0x39, 0x3B, 0x3D, 0x40, 0x41,
            0x43, 0x45, 0x47, 0x49, 0x4B, 0x4C, 0x4E, 0x50, 0x51,
            0x53, 0x54, 0x56, 0x57, 0x59, 0x5A, 0x5B, 0x5D, 0x5E,
            0x60, 0x61, 0x62, 0x63, 0x65, 0x66, 0x67, 0x68, 0x6A,
            0x6B, 0x6C, 0x6D, 0x6E, 0x70, 0x71, 0x72, 0x73, 0x74,
            0x75, 0x76, 0x77, 0x78, 0x79, 0x7A, 0x7B, 0x7C, 0x7D,
            0x7E, 0x80, 0x80, 0x81, 0x82, 0x83, 0x84, 0x85, 0x86,
            0x87, 0x88, 0x89, 0x8A, 0x8B, 0x8C, 0x8D, 0x8E, 0x8F,
            0x90, 0x90, 0x91, 0x92, 0x93, 0x94, 0x95, 0x96, 0x96,
            0x97, 0x98, 0x99, 0x9A, 0x9B, 0x9B, 0x9C, 0x9D, 0x9E,
            0x9F, 0xA0, 0xA0, 0xA1, 0xA2, 0xA3, 0xA3, 0xA4, 0xA5,
            0xA6, 0xA7, 0xA7, 0xA8, 0xA9, 0xAA, 0xAA, 0xAB, 0xAC,
            0xAD, 0xAD, 0xAE, 0xAF, 0xB0, 0xB0, 0xB1, 0xB2, 0xB2,
            0xB3, 0xB4, 0xB5, 0xB5, 0xB6, 0xB7, 0xB7, 0xB8, 0xB9,
            0xB9, 0xBA, 0xBB, 0xBB, 0xBC, 0xBD, 0xBD, 0xBE, 0xBF,
            0xC0, 0xC0, 0xC1, 0xC1, 0xC2, 0xC3, 0xC3, 0xC4, 0xC5,
            0xC5, 0xC6, 0xC7, 0xC7, 0xC8, 0xC9, 0xC9, 0xCA, 0xCB,
            0xCB, 0xCC, 0xCC, 0xCD, 0xCE, 0xCE, 0xCF, 0xD0, 0xD0,
            0xD1, 0xD1, 0xD2, 0xD3, 0xD3, 0xD4, 0xD4, 0xD5, 0xD6,
            0xD6, 0xD7, 0xD7, 0xD8, 0xD9, 0xD9, 0xDA, 0xDA, 0xDB,
            0xDB, 0xDC, 0xDD, 0xDD, 0xDE, 0xDE, 0xDF, 0xE0, 0xE0,
            0xE1, 0xE1, 0xE2, 0xE2, 0xE3, 0xE3, 0xE4, 0xE5, 0xE5,
            0xE6, 0xE6, 0xE7, 0xE7, 0xE8, 0xE8, 0xE9, 0xEA, 0xEA,
            0xEB, 0xEB, 0xEC, 0xEC, 0xED, 0xED, 0xEE, 0xEE, 0xEF,
            0xF0, 0xF0, 0xF1, 0xF1, 0xF2, 0xF2, 0xF3, 0xF3, 0xF4,
            0xF4, 0xF5, 0xF5, 0xF6, 0xF6, 0xF7, 0xF7, 0xF8, 0xF8,
            0xF9, 0xF9, 0xFA, 0xFA, 0xFB, 0xFB, 0xFC, 0xFC, 0xFD,
            0xFD, 0xFE, 0xFE, 0xFF
        };

        /// <summary>
        /// Returns the absolute value of the specified integer.
        /// </summary>
        public static int Abs(int value)
        {
            if (value < 0)
            {
                return -value;
            }

            return value;
        }

        /// <summary>
        /// Clamps the specified number.
        /// </summary>
        public static int Clamp(int a1, int a2, int a3)
        {
            if (a1 > a2)
            {
                return a3;
            }
            if (a1 < a3)
            {
                return a2;
            }

            return a2;
        }

        /// <summary>
        /// Square roots the specified value.
        /// </summary>
        public static int Sqrt(int value)
        {
            if (value >= 0x10000)
            {
                int num;

                if (value >= 0x1000000)
                {
                    if (value >= 0x10000000)
                    {
                        if (value >= 0x40000000)
                        {
                            if (value >= 0x7FFFFFFF)
                            {
                                return 0xFFFF;
                            }

                            num = LogicMath.SqrtTable[value >> 24] << 8;
                        }
                        else
                        {
                            num = LogicMath.SqrtTable[value >> 22] << 7;
                        }
                    }
                    else if (value >= 0x4000000)
                    {
                        num = LogicMath.SqrtTable[value >> 20] << 6;
                    }
                    else
                    {
                        num = LogicMath.SqrtTable[value >> 18] << 5;
                    }

                    num = num + 1 + value / num >> 1;
                    num = num + 1 + value / num >> 1;

                    return num * num <= value ? num : num - 1;
                }
                if (value >= 0x100000)
                {
                    if (value >= 0x400000)
                    {
                        num = LogicMath.SqrtTable[value >> 16] << 4;
                    }
                    else
                    {
                        num = LogicMath.SqrtTable[value >> 14] << 3;
                    }
                }
                else if (value >= 0x40000)
                {
                    num = LogicMath.SqrtTable[value >> 12] << 2;
                }
                else
                {
                    num = LogicMath.SqrtTable[value >> 10] << 1;
                }

                num = num + 1 + value / num >> 1;

                return num * num <= value ? num : num - 1;
            }
            else
            {
                if (value >= 0x100)
                {
                    int num;

                    if (value >= 0x1000)
                    {
                        if (value >= 0x4000)
                        {
                            num = LogicMath.SqrtTable[value >> 8] + 1;
                        }
                        else
                        {
                            num = (LogicMath.SqrtTable[value >> 6] >> 1) + 1;
                        }
                    }
                    else if (value >= 0x400)
                    {
                        num = (LogicMath.SqrtTable[value >> 4] >> 2) + 1;
                    }
                    else
                    {
                        num = (LogicMath.SqrtTable[value >> 2] >> 3) + 1;
                    }

                    return num * num <= value ? num : num - 1;
                }

                if (value >= 0)
                {
                    return LogicMath.SqrtTable[value] >> 4;
                }

                return -1;
            }
        }

        /// <summary>
        /// Returns the greater of two int values.
        /// </summary>
        public static int Max(int valueA, int valueB)
        {
            if (valueA >= valueB)
            {
                return valueA;
            }

            return valueB;
        }

        /// <summary>
        /// Returns the smaller of two int values.
        /// </summary>
        public static int Min(int valueA, int valueB)
        {
            if (valueA <= valueB)
            {
                return valueA;
            }

            return valueB;
        }
    }
}