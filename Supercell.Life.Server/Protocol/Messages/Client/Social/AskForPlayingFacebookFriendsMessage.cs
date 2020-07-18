namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class AskForPlayingFacebookFriendsMessage : PiranhaMessage
    {
        internal LogicArrayList<string> Friends;

        /// <summary>
        /// The service node for this message.
        /// </summary>
        internal override ServiceNode Node
        {
            get
            {
                return ServiceNode.Account;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AskForPlayingFacebookFriendsMessage"/> class.
        /// </summary>
        public AskForPlayingFacebookFriendsMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            this.Friends = new LogicArrayList<string>();
        }

        internal override void Decode()
        {
            for (int i = 0; i < this.Stream.ReadInt(); i++)
            {
                this.Friends.Add(this.Stream.ReadString());
            }
        }

        internal override void Handle()
        {
        }
    }
}