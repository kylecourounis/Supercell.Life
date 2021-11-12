namespace Supercell.Life.Titan.DataStream
{
    using Supercell.Life.Titan.Logic.Math;

    public class ChecksumEncoder
    {
        private int Checksum;

        /// <summary>
        /// Gets a value indicating whether this <see cref="ChecksumEncoder"/> is in checksum only mode.
        /// </summary>
        public virtual bool IsCheckSumOnlyMode
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChecksumEncoder"/> class.
        /// </summary>
        public ChecksumEncoder()
        {
            // ChecksumEncoder.
        }

        /// <summary>
        /// Writes the byte.
        /// </summary>
        public virtual void WriteByte(byte value)
        {
            this.Checksum = value + this.RotateRight(this.Checksum, 31) + 11;
        }

        /// <summary>
        /// Writes the boolean.
        /// </summary>
        public virtual void WriteBoolean(bool value)
        {
            var v2 = 7;

            if (value)
            {
                v2 = 13;
            }
            
            this.Checksum = v2 + this.RotateRight(this.Checksum, 31);
        }

        /// <summary>
        /// Writes the Int16.
        /// </summary>
        public virtual void WriteShort(short value)
        {
            this.Checksum = value + this.RotateRight(this.Checksum, 31) + 19;
        }

        /// <summary>
        /// Writes the Int32.
        /// </summary>
        public virtual void WriteInt(int value)
        {
            this.Checksum = value + this.RotateRight(this.Checksum, 31) + 9;
        }

        /// <summary>
        /// Writes the long.
        /// </summary>
        public virtual void WriteLong(long value)
        {
            int high = (int)(value >> 32);
            int low  = (int)value;

            this.Checksum = high + this.RotateRight(low + this.RotateRight(this.Checksum, 31) + 67, 31) + 91;
        }

        /// <summary>
        /// Writes the <see cref="LogicLong"/>.
        /// </summary>
        public virtual void WriteLogicLong(LogicLong value)
        {
            value.Encode(this);
        }

        /// <summary>
        /// Writes the bytes.
        /// </summary>
        public virtual void WriteBytes(byte[] value, int length)
        {
            int len = 27;

            if (value != null)
            {
                len = length + 28;
            }

            this.Checksum = (len + (this.Checksum >> 31)) | (this.Checksum << (32 - 31));
        }

        /// <summary>
        /// Writes the string.
        /// </summary>
        public virtual void WriteString(string value)
        {
            int len = 27;

            if (value != null)
            {
                len = value.Length + 28;
            }

            this.Checksum = len + this.RotateRight(this.Checksum, 31);
        }

        /// <summary>
        /// Resets the checksum.
        /// </summary>
        public void ResetCheckSum()
        {
            this.Checksum = 0;
        }

        /// <summary>
        /// Rotates the integer value.
        /// </summary>
        private int RotateRight(int value, int count)
        {
            return (value >> count) | (value << (32 - count));
        }
    }
}
