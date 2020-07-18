namespace Supercell.Life.Titan.Logic.Math
{
    using System.Runtime.InteropServices;
    
    using Supercell.Life.Titan.DataStream;

    [ComVisible(true)]
    [StructLayout(LayoutKind.Sequential, Size = 8)]
    public struct LogicLong
    {
        /// <summary>
        /// Gets the higher int of long.
        /// </summary>
        public int High
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the lower int of long.
        /// </summary>
        public int Low
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets if this instance is equal at 0.
        /// </summary>
        public bool IsZero
        {
            get
            {
                return this.High == 0 && this.Low == 0;
            }
        }

        /// <summary>
        /// Gets a value indicating the long value.
        /// </summary>
        public long Value
        {
            get
            {
                return (long)this.High << 32 | (uint)this.Low;
            }
        }
        
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicLong"/> struct.
        /// </summary>
        public LogicLong(long value)
        {
            this.High = (int)(value >> 32);
            this.Low  = (int)value;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicLong"/> struct.
        /// </summary>
        public LogicLong(int high, int low)
        {
            this.High = high;
            this.Low  = low;
        }
        
        /// <summary>
        /// Encodes this instance.
        /// </summary>
        public void Encode(ByteStream stream)
        {
            stream.WriteInt(this.High);
            stream.WriteInt(this.Low);
        }

        /// <summary>
        /// Decodes this instance.
        /// </summary>
        public LogicLong Decode(ByteStream stream)
        {
            this.High = stream.ReadInt();
            this.Low  = stream.ReadInt();

            return this;
        }

        public static LogicLong operator +(LogicLong logicLong, long value)
        {
            return new LogicLong(logicLong.Value + value);
        }

        public static LogicLong operator -(LogicLong logicLong, long value)
        {
            return new LogicLong(logicLong.Value - value);
        }

        public static LogicLong operator /(LogicLong logicLong, long value)
        {
            return new LogicLong(logicLong.Value / value);
        }

        public static LogicLong operator %(LogicLong logicLong, long value)
        {
            return new LogicLong(logicLong.Value % value);
        }

        public static LogicLong operator *(LogicLong logicLong, long value)
        {
            return new LogicLong(logicLong.Value * value);
        }

        public static implicit operator LogicLong(long value)
        {
            return new LogicLong(value);
        }

        public static implicit operator long(LogicLong logicLong)
        {
            return logicLong.Value;
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        public override string ToString()
        {
            return $"{this.High}-{this.Low}";
        }
    }
}
