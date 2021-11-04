namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;
    using Supercell.Life.Server.Protocol.Commands.Server;
    using Supercell.Life.Server.Protocol.Messages.Server;

    internal class ChangeNameMessage : PiranhaMessage
    {
        private string Name;

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
        /// Initializes a new instance of the <see cref="ChangeNameMessage"/> class.
        /// </summary>
        public ChangeNameMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // ChangeNameMessage.
        }

        internal override void Decode()
        {
            this.Name = this.Stream.ReadString();
        }
        
        internal override void Handle()
        {
            this.Connection.GameMode.Avatar.Name          = this.Name;
            this.Connection.GameMode.Avatar.NameSetByUser = true;

            this.Connection.GameMode.Avatar.Save();

            new AvailableServerCommandMessage(this.Connection, new LogicChangeNameCommand(this.Connection)).Send();
        }
    }
}
