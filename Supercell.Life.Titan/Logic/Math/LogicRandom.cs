namespace Supercell.Life.Titan.Logic.Math
{
    using System;

    public class LogicRandom
    {
        private int Seed;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicRandom"/> class.
        /// </summary>
        public LogicRandom()
        {
            // LogicRandom.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicRandom"/> class.
        /// </summary>
        public LogicRandom(int seed)
        {
            this.SetIteratedRandomSeed(seed);
        }

        /// <summary>
        /// Generates a pseudo-random number between zero and one less than the specified maximum value.
        /// </summary>
        public int Rand(int max)
        {
            int result = 0;
            
            if (max >= 1)
            {
                this.Seed = this.IterateRandomSeed();

                int v6;

                if (this.Seed <= -1)
                {
                    v6 = -this.Seed;
                }
                else
                {
                    v6 = this.Seed;
                }

                result = v6 % max;
            }

            return result;
        }

        /// <summary>
        /// Generates a pseudo-random number between the specified minimum and maximum values.
        /// </summary>
        public int Rand(int min, int max)
        {
            return min + this.Rand(max - min);
        }

        /// <summary>
        /// Iterates the random seed.
        /// </summary>
        public int IterateRandomSeed()
        {
            int v3 = this.Seed;

            if (v3 == 0)
            {
                v3 = -1;
            }

            int v4 = v3 ^ (v3 << 13) ^ ((v3 ^ (v3 << 13)) >> 17);
            int v5 = v4 ^ (32 * v4);

            return v5;
        }

        /// <summary>
        /// Gets the iterated random seed.
        /// </summary>
        public int GetIteratedRandomSeed()
        {
            return this.Seed;
        }

        /// <summary>
        /// Sets the iterated random seed.
        /// </summary>
        public void SetIteratedRandomSeed(int randSeed)
        {
            this.Seed = randSeed;
        }

        /// <summary>
        /// Generates a random string with the specified length.
        /// </summary>
        public static string GenerateRandomString(int length = 40)
        {
            var random    = new Random();
            var returnVal = string.Empty;

            for (int i = 0; i < length; i++)
            {
                returnVal += (char)random.Next('A', 'Z');
            }

            return returnVal;
        }
    }
}
