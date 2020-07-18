namespace Supercell.Life.Server.Protocol.Messages.Client
{
    using Supercell.Life.Titan.DataStream;
    
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class SetDeviceTokenMessage : PiranhaMessage
    {
        private string Password;

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
        /// Initializes a new instance of the <see cref="SetDeviceTokenMessage"/> class.
        /// </summary>
        public SetDeviceTokenMessage(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // SetDeviceTokenMessage.
        }

        internal override void Decode()
        {
            this.Password = this.Stream.ReadString();
        }
    }
}