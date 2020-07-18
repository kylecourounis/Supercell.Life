namespace Supercell.Life.Titan.DataStream
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Supercell.Life.Titan.Core;
    using Supercell.Life.Titan.Library.Compression.ZLib;
    using Supercell.Life.Titan.Logic.Math;
    using Supercell.Life.Titan.Logic.Utils;

    public class ByteStream : IDisposable
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
        public void WriteByte(byte value)
        {
            this.Buffer[this.Offset++] = value;
        }

        /// <summary>
        /// Writes the boolean.
        /// </summary>
        public void WriteBoolean(bool value)
        {
            this.WriteByte((byte)(value ? 1 : 0));
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
        /// Writes the short.
        /// </summary>
        public void WriteShort(short value)
        {
            this.Write(BitConverter.GetBytes(value).Reverse());
        }
        
        /// <summary>
        /// Writes the int24.
        /// </summary>
        public void WriteInt24(int value)
        {
            this.Write(BitConverter.GetBytes(value).Reverse().Skip(1));
        }
        
        /// <summary>
        /// Writes the int.
        /// </summary>
        public void WriteInt(int value)
        {
            this.Write(BitConverter.GetBytes(value).Reverse());
        }

        /// <summary>
        /// Writes the int endian.
        /// </summary>
        public void WriteIntEndian(int value)
        {
            this.Write(BitConverter.GetBytes(value));
        }

        /// <summary>
        /// Writes the long.
        /// </summary>
        public void WriteLong(long value)
        {
            this.Write(BitConverter.GetBytes(value).Reverse());
        }

        /// <summary>
        /// Writes the LogicLong.
        /// </summary>
        public void WriteLogicLong(int high, int low)
        {
            this.WriteLogicLong(new LogicLong(high, low));
        }

        /// <summary>
        /// Writes the <see cref="LogicLong"/>.
        /// </summary>
        public void WriteLogicLong(LogicLong value)
        {
            value.Encode(this);
        }
        
        /// <summary>
        /// Writes a VInt - Thank you to nameless for making this shorter as well! 
        /// </summary>
        public void WriteVInt(int value)
        {
            int tmp = (value >> 25) & 0x40;
            int flipped = value ^ (value >> 31);

            tmp |= value & 0x3F;
            value >>= 6;

            if ((flipped >>= 6) == 0)
            {
                this.WriteByte((byte)tmp);
                return;
            }

            this.WriteByte((byte)(tmp | 0x80));

            do
            {
                this.WriteByte((byte)((value & 0x7F) | ((flipped >>= 7) != 0 ? 0x80 : 0)));
                value >>= 7;
            } while (flipped != 0);
        }

        /// <summary>
        /// Writes the string.
        /// </summary>
        public void WriteString(string value)
        {
            if (value != null)
            {
                int length = value.Length;

                if (length > 0)
                {
                    this.WriteInt(length);
                    this.Write(LogicStringUtil.GetBytes(value), length);
                }
                else
                {
                    this.WriteInt(0);
                }
            }
            else
            {
                this.WriteInt(-1);
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

                    this.WriteInt(compressed.Length + 4);
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
        /// Writes the bytes.
        /// </summary>
        public void WriteBytes(byte[] buffer)
        {
            if (buffer != null)
            {
                int length = buffer.Length;

                if (length > 0)
                {
                    this.WriteInt(length);
                    this.Write(buffer);
                }
                else
                {
                    this.WriteInt(0);
                }
            }
            else
            {
                this.WriteInt(-1);
            }
        }

        /// <summary>
        /// Writes the hexadecimal string.
        /// </summary>
        public void WriteHex(string value)
        {
            this.Write(value.HexaToBytes());
        }

        /// <summary>
        /// Writes the specified buffer.
        /// </summary>
        public void Write(IEnumerable<byte> buffer)
        {
            byte[] bytes = buffer.ToArray();

            this.EnsureCapacity(bytes.Length);
            
            Array.Copy(bytes, 0, this.Buffer, this.Offset, bytes.Length);
            this.Offset += bytes.Length;
        }

        /// <summary>
        /// Writes the specified buffer.
        /// </summary>
        public void Write(IEnumerable<byte> buffer, int length)
        {
            byte[] bytes = buffer.ToArray();

            this.EnsureCapacity(length);

            Array.Copy(bytes, 0, this.Buffer, this.Offset, length);
            this.Offset += length;
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
            return this.ReadBytes(2).ToInt16();
        }
        
        /// <summary>
        /// Reads the Int24.
        /// </summary>
        public int ReadInt24()
        {
            return this.ReadBytes(3).ToInt24();
        }

        /// <summary>
        /// Reads the Int32.
        /// </summary>
        public int ReadInt()
        {
            return this.ReadBytes(4).ToInt32();
        }

        /// <summary>
        /// Reads the long.
        /// </summary>
        public long ReadLong()
        {
            return this.ReadBytes(8).ToInt64();
        }

        /// <summary>
        /// Reads the long.
        /// </summary>
        public LogicLong ReadLogicLong()
        {
            return new LogicLong().Decode(this);
        }

        /// <summary>
        /// Reads a VInt - Thanks to nameless for making this so much shorter!
        /// </summary>
        public int ReadVInt()
        {
            int b, sign = ((b = this.ReadByte()) >> 6) & 1, i = b & 0x3F, offset = 6;

            for (int j = 0; j < 4 && (b & 0x80) != 0; j++, offset += 7)
            {
                i |= ((b = this.ReadByte()) & 0x7F) << offset;
            }

            return (b & 0x80) != 0 ? -1 : i | (sign == 1 && offset < 32 ? i | (int)(0xFFFFFFFF << offset) : i);
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
