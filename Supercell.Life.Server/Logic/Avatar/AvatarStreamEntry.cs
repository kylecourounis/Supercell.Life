namespace Supercell.Life.Server.Logic.Avatar
{
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
            // Encode.
        }
    }
}
