namespace Supercell.Life.Titan.Library.Cryptography.RC4
{
    public class RC4Encrypter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RC4Encrypter"/> class.
        /// </summary>
        public RC4Encrypter(string Key) : this(Key, "nonce")
        {
            // RC4Encrypter.
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RC4Encrypter"/> class.
        /// </summary>
        public RC4Encrypter(string Key, string Nonce)
        {
            this.InitializeCiphers(Key + Nonce);
        }

        /// <summary>
        /// Initializes the ciphers.
        /// </summary>
        private void InitializeCiphers(string key)
        {
            this.Encrypter = new RC4(key);
            this.Decrypter = new RC4(key);

            for (int i = 0; i < key.Length; i++)
            {
                this.Encrypter.PRGA();
                this.Decrypter.PRGA();
            }
        }

        /// <summary>
        /// Encrypts the specified buffer.
        /// </summary>
        public byte[] Encrypt(byte[] buffer)
        {
            for (int k = 0; k < buffer.Length; k++)
            {
                buffer[k] ^= this.Encrypter.PRGA();
            }

            return buffer;
        }

        /// <summary>
        /// Decrypts the specified buffer.
        /// </summary>
        public byte[] Decrypt(byte[] buffer)
        {
            for (int k = 0; k < buffer.Length; k++)
            {
                buffer[k] ^= this.Decrypter.PRGA();
            }

            return buffer;
        }

        /// <summary>
        /// Gets or sets an instance of the <see cref="RC4"/> class as the encrypter.
        /// </summary>
        private RC4 Encrypter
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets an instance of the <see cref="RC4"/> class as the decrypter.
        /// </summary>
        private RC4 Decrypter
        {
            get;
            set;
        }
    }
}