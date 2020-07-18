namespace Supercell.Life.Server.Protocol.Messages.Server
{
    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;
    using Supercell.Life.Server.Protocol.Commands.Server;

    internal class AvailableServerCommandMessage : PiranhaMessage
    {
        private readonly LogicServerCommand Command;

        /// <summary>
        /// The service node for this message.
        /// </summary>
        internal override ServiceNode Node
        {
            get
            {
                return ServiceNode.Home;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AvailableServerCommandMessage"/> class.
        /// </summary>
        public AvailableServerCommandMessage(Connection connection, LogicServerCommand command) : base(connection)
        {
            this.Type    = Message.AvailableServerCommand;
            this.Command = command;
        }
        
        internal override void Encode()
        {
            this.Command.Encode();

            this.Stream.WriteInt((int)this.Command.Type);
            this.Stream.Write(this.Command.Stream.ToArray());

            this.Command.Execute();
        }
    }
}