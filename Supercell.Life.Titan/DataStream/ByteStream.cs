namespace Supercell.Life.Titan.DataStream
{
    using System;
    using System.Text;

    using Supercell.Life.Titan.Core;
    using Supercell.Life.Titan.Library.Compression.ZLib;
    using Supercell.Life.Titan.Logic.Math;
    using Supercell.Life.Titan.Logic.Utils;

    public class ByteStream : ChecksumEncoder, IDisposable
    {
        private byte[] Buffer;
        private int Offset;

        /// <summary>
        /// Gets the length of this instance's buffer.
        /// </summary>
        public int Length
        {
            get
            {
                return this.Buffer.Length;
            }
        }

        /// <summary>
        /// Gets the number of bytes left in this instance.
        /// </summary>
        public int BytesLeft
        {
            get
            {
                return this.Buffer.Length - this.Offset;
            }
        }

        /// <summary>
        /// Gets a value indicating whether there are any bytes left in the stream.
        /// </summary>
        public bool IsAtEnd
        {
            get
            {
                return this.BytesLeft <= 0;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="ChecksumEncoder"/> is in checksum only mode.
        /// </summary>
        public override bool IsCheckSumOnlyMode
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ByteStream"/> class.
        /// </summary>
        public ByteStream(int size = Constants.BufferSize)
        {
            this.Buffer = new byte[size];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ByteStream"/> class.
        /// </summary>
        public ByteStream(byte[] buffer)
        {
            this.Buffer = buffer;
        }
        
        /// <summary>
        /// Writes the byte.
        /// </summary>
        public override void WriteByte(byte value)
        {
            base.WriteByte(value);
            this.EnsureCapacity(1);

            this.Buffer[this.Offset++] = value;
        }

        /// <summary>
        /// Writes the boolean.
        /// </summary>
        public override void WriteBoolean(bool value)
        {
            base.WriteBoolean(value);
            this.EnsureCapacity(1);

            this.Buffer[this.Offset++] = (byte)(value ? 1 : 0);
        }

        /// <summary>
        /// Writes the specified boolean values as one byte.
        /// </summary>
        public void WriteBools(params bool[] values)
        {
            byte boolean = 0;

            for (int i = 0; i < values.Length; i++)
            {
                bool value = values[i];

                if (value)
                {
                    boolean |= (byte)(1 << i);
                }
            }
            
            this.WriteByte(boolean);
        }

        /// <summary>
        /// Writes the Int16.
        /// </summary>
        public override void WriteShort(short value)
        {
            base.WriteShort(value);
            this.EnsureCapacity(2);

            this.Buffer[this.Offset++] = (byte)(value >> 8);
            this.Buffer[this.Offset++] = (byte)value;
        }
        
        /// <summary>
        /// Writes the Int32.
        /// </summary>
        public override void WriteInt(int value)
        {
            base.WriteInt(value);
            this.EnsureCapacity(4);

            this.Buffer[this.Offset++] = (byte)(value >> 24);
            this.Buffer[this.Offset++] = (byte)(value >> 16);
            this.Buffer[this.Offset++] = (byte)(value >> 8);
            this.Buffer[this.Offset++] = (byte)value;
        }

        /// <summary>
        /// Writes the Int32 endian.
        /// </summary>
        public void WriteIntEndian(int value)
        {
            base.WriteInt(value);
            this.EnsureCapacity(4);

            this.Buffer[this.Offset++] = (byte)value;
            this.Buffer[this.Offset++] = (byte)(value >> 8);
            this.Buffer[this.Offset++] = (byte)(value >> 16);
            this.Buffer[this.Offset++] = (byte)(value >> 24);
        }

        /// <summary>
        /// Writes the Int32 to byte array without modifying the checksum.
        /// </summary>
        public void WriteIntToByteArray(int value)
        {
            this.EnsureCapacity(4);

            this.Buffer[this.Offset++] = (byte)(value >> 24);
            this.Buffer[this.Offset++] = (byte)(value >> 16);
            this.Buffer[this.Offset++] = (byte)(value >> 8);
            this.Buffer[this.Offset++] = (byte)value;
        }

        /// <summary>
        /// Writes the Int64.
        /// </summary>
        public override void WriteLong(long value)
        {
            base.WriteLong(value);

            this.WriteIntToByteArray((int)(value >> 32));
            this.WriteIntToByteArray((int)value);
        }

        /// <summary>
        /// Writes the <see cref="LogicLong"/>.
        /// </summary>
        public void WriteLogicLong(int high, int low)
        {
            base.WriteLogicLong(new LogicLong(high, low));
        }

        /// <summary>
        /// Writes the string.
        /// </summary>
        public override void WriteString(string value)
        {
            base.WriteString(value);

            if (value != null)
            {
                byte[] bytes = LogicStringUtil.GetBytes(value);
                int length = bytes.Length;

                if (length <= 900000)
                {
                    this.EnsureCapacity(length + 4);

                    this.WriteIntToByteArray(length);
                    this.Write(bytes);

                    this.Offset += length;
                }
                else
                {
                    Debugger.Warning($"Invalid string length {length}");
                    this.WriteIntToByteArray(-1);
                }
            }
            else
            {
                this.WriteIntToByteArray(-1);
            }
        }

        /// <summary>
        /// Writes a compressed string.
        /// </summary>
        public void WriteCompressedString(string value)
        {
            if (value != null)
            {
                int length = value.Length;

                if (length > 100)
                {
                    this.WriteByte(1);

                    byte[] compressed = ZlibStream.CompressString(value);

                    this.WriteIntToByteArray(compressed.Length + 4);
                    this.WriteIntEndian(length);
                    this.Write(compressed);
                }
                else
                {
                    this.WriteByte(0);
                    this.WriteString(value);
                }
            }
            else
            {
                this.WriteByte(0);
                this.WriteInt(0);
            }
        }

        /// <summary>
        /// Writes the length of the byte array as an int, followed by the bytes.
        /// </summary>
        public override void WriteBytes(byte[] value, int length)
        {
            base.WriteBytes(value, length);

            if (value == null)
            {
                this.WriteIntToByteArray(-1);
            }
            else
            {
                this.EnsureCapacity(length + 4);

                this.WriteIntToByteArray(length);
                this.Write(value, length);

                this.Offset += length;
            }
        }

        /// <summary>
        /// Writes the bytes without writing the length as an int.
        /// </summary>
        public void WriteBytesWithoutLength(byte[] value, int length)
        {
            base.WriteBytes(value, length);

            if (value != null)
            {
                this.EnsureCapacity(length);
                this.Write(value);
                this.Offset += length;
            }
        }

        /// <summary>
        /// Writes the specified buffer.
        /// </summary>
        public void Write(byte[] bytes, int length = 0)
        {
            System.Buffer.BlockCopy(bytes, 0, this.Buffer, this.Offset, (length > 0 ? length : bytes.Length));
        }

        /// <summary>
        /// Ensures the capacity.
        /// </summary>
        private void EnsureCapacity(int count)
        {
            if (this.Buffer.Length < this.Offset + count)
            {
                byte[] nBuffer = new byte[this.Buffer.Length * 2 > this.Buffer.Length + count ? this.Buffer.Length * 2 : this.Buffer.Length + count];
                Array.Copy(this.Buffer, 0, nBuffer, 0, this.Offset);
                this.Buffer = nBuffer;
            }
        }

        /// <summary>
        /// Reads the byte.
        /// </summary>
        public byte ReadByte()
        {
            return this.Buffer[this.Offset++];
        }

        /// <summary>
        /// Reads the boolean.
        /// </summary>
        public bool ReadBoolean()
        {
            return Convert.ToBoolean(this.ReadByte());
        }

        /// <summary>
        /// Reads the Int16.
        /// </summary>
        public short ReadShort()
        {
            return (short)((this.ReadByte() << 8) | this.ReadByte());
        }
        
        /// <summary>
        /// Reads the Int32.
        /// </summary>
        public int ReadInt()
        {
            return (this.ReadByte() << 24) | (this.ReadByte() << 16) | (this.ReadByte() << 8) | this.ReadByte();
        }

        /// <summary>
        /// Reads the Int64.
        /// </summary>
        public long ReadLong()
        {
            LogicLong logicLong = new LogicLong();
            logicLong.Decode(this);
            return logicLong.Value;
        }

        /// <summary>
        /// Reads the <see cref="LogicLong"/>.
        /// </summary>
        public LogicLong ReadLogicLong()
        {
            return new LogicLong().Decode(this);
        }

        /// <summary>
        /// Reads the string.
        /// </summary>
        public string ReadString()
        {
            int length = this.ReadInt();

            if (length < 0)
            {
                if (length != -1)
                {
                    Debugger.Error($"String length is invalid. ({length})");
                }

                return null;
            }

            return Encoding.UTF8.GetString(this.ReadBytes(length));
        }

        /// <summary>
        /// Reads the bytes.
        /// </summary>
        public byte[] ReadBytes()
        {
            int length = this.ReadInt();
            
            if (length > 0)
            {
                return this.ReadBytes(length);
            }

            return new byte[0];
        }

        /// <summary>
        /// Reads the specified number of bytes.
        /// </summary>
        public byte[] ReadBytes(int count)
        {
            if (count <= this.BytesLeft)
            {
                byte[] array = new byte[count];

                System.Buffer.BlockCopy(this.Buffer, this.Offset, array, 0, count);
                this.Offset += count;

                return array;
            }

            return null;
        }

        /// <summary>
        /// Converts this instance to a byte array.
        /// </summary>
        public byte[] ToArray()
        {
            byte[] bytes = new byte[this.Length];

            Array.Copy(this.Buffer, 0, bytes, 0, this.Length);

            return bytes;
        }

        /// <summary>
        /// Clears and sets <see cref="ByteStream.Buffer"/> to the specified byte array.
        /// </summary>
        public void SetBuffer(byte[] buffer)
        {
            if (this.Buffer != null)
            {
                this.Buffer = null;
                this.Offset = 0;

                if (buffer != null)
                {
                    this.Buffer = buffer;
                }
            }
        }

        /// <summary>
        /// Reads all of the bytes left in this instance.
        /// </summary>
        public byte[] ReadAllBytes()
        {
            return this.ReadBytes(this.BytesLeft);
        }

        /// <summary>
        /// Gets a hexadecimal string for the bytes left in this instance.
        /// </summary>
        public string ToHexa()
        {
            return BitConverter.ToString(this.ReadAllBytes());
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Buffer = null;
            this.Offset = 0;

            GC.SuppressFinalize(this);
        }
    }
}
