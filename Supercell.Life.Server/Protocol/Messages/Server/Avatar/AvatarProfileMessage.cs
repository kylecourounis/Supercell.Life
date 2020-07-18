namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Logic.Avatar;
    using Supercell.Life.Server.Logic.Slots;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class AvatarProfileMessage : PiranhaMessage
    {
        private readonly LogicClientAvatar Avatar;

        /// <summary>
        /// The service node for this message.
        /// </summary>
        internal override ServiceNode Node
        {
            get
            {
                return ServiceNode.Avatar;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvatarProfileMessage"/> class.
        /// </summary>
        internal AvatarProfileMessage(Connection connection, LogicLong visitId) : base(connection)
        {
            this.Type   = Message.AvatarProfile;
            this.Avatar = Avatars.Get(visitId);
        }

        internal override void Encode()
        {
            this.Avatar.Encode(this.Stream);

            this.Stream.WriteInt(0);
            this.Stream.WriteInt(0);
            this.Stream.WriteInt(0);
            this.Stream.WriteInt(0);
        }
    }
}
