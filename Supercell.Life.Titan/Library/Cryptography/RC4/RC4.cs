namespace Supercell.Life.Titan.Library.Cryptography.RC4
{
    using Supercell.Life.Titan.Logic.Utils;

    public class RC4
    {
        private byte I;
        private byte J;

        private readonly byte[] Key;

        /// <summary>
        /// Initializes a new instance of the <see cref="RC4"/> class.
        /// </summary>
        internal RC4(byte[] Key)
        {
            this.Key = RC4.KSA(Key);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RC4"/> class.
        /// </summary>
        internal RC4(string Key)
        {
            this.Key = RC4.KSA(LogicStringUtil.GetBytes(Key));
        }

        /// <summary>
        /// Modifies the state and returns a byte of the keystream.
        /// </summary>
        internal byte PRGA()
        {
            this.I = (byte)((this.I + 1) % 256);
            this.J = (byte)((this.J + this.Key[this.I]) % 256);

            byte Temp = this.Key[this.I];
            this.Key[this.I] = this.Key[this.J];
            this.Key[this.J] = Temp;

            return this.Key[(this.Key[this.I] + this.Key[this.J]) % 256];
        }

        /// <summary>
        /// Returns the key as a byte array using the key-scheduling algorithm.
        /// </summary>
        private static byte[] KSA(byte[] key)
        {
            byte[] rc4Key = new byte[256];

            for (int i = 0; i != 256; i++)
            {
                rc4Key[i] = (byte)i;
            }

            byte j = 0;

            for (int i = 0; i != 256; i++)
            {
                j = (byte)((j + rc4Key[i] + key[i % key.Length]) % 256);

                byte swap = rc4Key[i];
                rc4Key[i] = rc4Key[j];
                rc4Key[j] = swap;
            }

            return rc4Key;
        }
    }
}