namespace Supercell.Life.Server.Logic.Avatar
{
    using Supercell.Life.Server.Files.CsvHelpers;
    using Supercell.Life.Server.Helpers;
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Math;

    internal class AvatarStreamEntry
    {
        internal LogicClientAvatar Avatar;

        internal LogicLong SenderAvatarID;

        internal string SenderName;

        internal int AgeSeconds;

        internal bool Removed;

        internal bool New;

        internal bool Dismissed;

        internal int StreamType;

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarStreamEntry"/> class.
        /// </summary>
        internal AvatarStreamEntry(LogicClientAvatar avatar)
        {
            this.Avatar = avatar;
        }

        /// <summary>
        /// Encodes this instance.
        /// </summary>
        internal void Encode(ChecksumEncoder encoder)
        {
            this.EncodeHeader(encoder); // sub_C06A4

            switch (this.StreamType)
            {
                case 0: // sub_13668
                {
                    encoder.WriteString("");
                    
                    encoder.WriteInt(0);
                    encoder.WriteInt(0);
                    encoder.WriteInt(0);
                    encoder.WriteInt(0);

                    encoder.WriteByte(0);

                    // Only writes if above byte is equal to 1
                    encoder.WriteInt(0);
                    encoder.WriteLong(0);

                    break;
                }
                case 2: // sub_97F54
                {
                    encoder.WriteLogicLong(0);
                    encoder.WriteString("");

                    encoder.WriteDataReference(null);
                    encoder.WriteString("");

                    encoder.WriteByte(0);

                    // Only writes if above byte is equal to 1
                    encoder.WriteLogicLong(0);

                    break;
                }
                case 3: // sub_2F344
                {
                    encoder.WriteLogicLong(0);
                    encoder.WriteString("");
                    encoder.WriteDataReference(null);

                    break;
                }
                case 6: // sub_135394
                {
                    encoder.WriteInt(0);
                    encoder.WriteInt(0);

                    break;
                }
            }
        }

        /// <summary>
        /// Encodes the header for this instance.
        /// </summary>
        private void EncodeHeader(ChecksumEncoder encoder)
        {
            encoder.WriteInt(this.StreamType);

            encoder.WriteLogicLong(this.Avatar.Identifier);
            encoder.WriteLogicLong(this.Avatar.Identifier);

            encoder.WriteString(this.Avatar.Name);

            encoder.WriteInt(0);
            encoder.WriteInt(0);
            encoder.WriteInt(0);

            encoder.WriteByte(0);
            encoder.WriteByte(0);
        }
    }
}
