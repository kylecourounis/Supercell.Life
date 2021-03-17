namespace Supercell.Life.Titan.Logic.Math
{
    using System;

    public class LogicRandom : Random
    {
        private int Seed = -237100689;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicRandom"/> class.
        /// </summary>
        public LogicRandom()
        {
        }

        /// <summary>
        /// Creates a random integer value.
        /// </summary>
        public int Rand(int max)
        {
            return this.Next(max);
        }

        /// <summary>
        /// Creates a random integer value in the specified range.
        /// </summary>
        public int Rand(int min, int max)
        {
            return this.Next(min, max);
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
        public string GenerateRandomString(int length = 40)
        {
            var returnVal = string.Empty;

            for (int i = 0; i < length; i++)
            {
                returnVal += (char)this.Rand('A', 'Z');
            }

            return returnVal;
        }
    }
}
