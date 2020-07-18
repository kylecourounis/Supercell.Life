namespace Supercell.Life.Titan.Library.Compression.LZMA
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Security.Cryptography;

    using SevenZip.SDK;
    using SevenZip.SDK.Compress.LZMA;

    using Supercell.Life.Titan.Helpers;

    public static class LZMA
    {
        private static readonly Encoder Encoder = new Encoder();
        private static readonly Decoder Decoder = new Decoder();

        /// <summary>
        /// Compresses the specified source file.
        /// </summary>
        public static void Compress(string Source, string Out, FileType Type)
        {
            using (FileStream InputStream = new FileStream(Source, FileMode.Open))
            {
                using (FileStream OutputStream = new FileStream(Out, FileMode.Create))
                {
                    switch (Type)
                    {
                        case FileType.CSV:
                        {
                            CoderPropID[] CoderPropIDs =
                            {
                                CoderPropID.DictionarySize,
                                CoderPropID.PosStateBits,
                                CoderPropID.LitContextBits,
                                CoderPropID.LitPosBits,
                                CoderPropID.Algorithm,
                                CoderPropID.NumFastBytes,
                                CoderPropID.MatchFinder,
                                CoderPropID.EndMarker
                            };

                            object[] Properties =
                            {
                                262144,
                                2,
                                3,
                                0,
                                2,
                                32,
                                "bt4",
                                false
                            };

                            LZMA.Encoder.SetCoderProperties(CoderPropIDs, Properties);
                            LZMA.Encoder.WriteCoderProperties(OutputStream);

                            OutputStream.Write(BitConverter.GetBytes(InputStream.Length), 0, 4);

                            LZMA.Encoder.Code(InputStream, OutputStream, InputStream.Length, -1L, null);

                            break;
                        }
                        case FileType.SC:
                        {
                            int Version = 1;

                            byte[] Header = new byte[26];

                            using (BinaryWriter Writer = new BinaryWriter(new MemoryStream(Header)))
                            {
                                Writer.Write("SC".ToCharArray());
                                Writer.Write(BitConverter.GetBytes(Version).Reverse().ToArray());

                                using (var MD5Hash = MD5.Create())
                                {
                                    byte[] MD5 = MD5Hash.ComputeHash(InputStream);

                                    Writer.Write(BitConverter.GetBytes(MD5.Length).Reverse().ToArray());
                                    Writer.Write(MD5);
                                }
                            }

                            FileInfo File = new FileInfo(Out);

                            byte[] OldData = File.ReadAllBytes();
                            byte[] NewData = Header.Concat(OldData).ToArray();

                            File.Delete();
                            File.WriteAllBytes(NewData);

                            break;
                        }
                        default:
                        {
                            break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Decompresses the specified source file.
        /// </summary>
        public static void Decompress(string Source, string Out, FileType Type)
        {
            using (FileStream InputStream = new FileStream(Source, FileMode.Open))
            {
                using (FileStream OutputStream = new FileStream(Out, FileMode.Create))
                {
                    switch (Type)
                    {
                        case FileType.CSV:
                        {
                            byte[] Properties = new byte[5];
                            InputStream.Read(Properties, 0, 5);

                            byte[] Buffer = new byte[4];
                            InputStream.Read(Buffer, 0, 4);

                            LZMA.Decoder.SetDecoderProperties(Properties);
                            LZMA.Decoder.Code(InputStream, OutputStream, InputStream.Length, BitConverter.ToInt32(Buffer, 0), null);

                            break;
                        }
                        case FileType.SC:
                        {
                            InputStream.Seek(26, SeekOrigin.Current);

                            byte[] Properties = new byte[5];
                            InputStream.Read(Properties, 0, 5);

                            byte[] Buffer = new byte[4];
                            InputStream.Read(Buffer, 0, 4);

                            LZMA.Decoder.SetDecoderProperties(Properties);
                            LZMA.Decoder.Code(InputStream, OutputStream, InputStream.Length, BitConverter.ToInt32(Buffer, 0), null);

                            break;
                        }
                        default:
                        {
                            break;
                        }
                    }
                }
            }
        }
    }
}
