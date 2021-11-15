namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Titan.DataStream;
    using Supercell.Life.Titan.Logic.Utils;

    using Supercell.Life.Server.Logic.Alliance;
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class ChatToAllianceStreamMessage : PiranhaMessage
    {
        internal string Message;

        /// <summary>
        /// The service node for this message.
        /// </summary>
        internal override ServiceNode Node
        {
            get
            {
                return ServiceNode.Alliance;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChatToAllianceStreamMessage"/> class.
        /// </summary>
        public ChatToAllianceStreamMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // ChatToAllianceStreamMessage.
        }

        internal override void Decode()
        {
            this.Message = this.Stream.ReadString();
        }

        internal override void Handle()
        {
            if (this.Connection.GameMode.Avatar.IsInAlliance)
            {
                if (!this.Message.IsNullOrEmptyOrWhitespace())
                {
                    if (this.Message.Length <= 128)
                    {
                        if (this.Message.StartsWith(" "))
                        {
                            this.Message = this.Message.Remove(0, 1);
                        }

                        if (this.Message.Length > 0)
                        {
                            AllianceMember sender = this.Connection.GameMode.Avatar.Alliance.Members.Find(member => member.Identifier == this.Connection.GameMode.Avatar.Identifier);
                            this.Connection.GameMode.Avatar.Alliance.AddEntry(new AllianceStreamEntry(sender, this.Message));
                        }
                    }
                }
            }
        }
    }
}