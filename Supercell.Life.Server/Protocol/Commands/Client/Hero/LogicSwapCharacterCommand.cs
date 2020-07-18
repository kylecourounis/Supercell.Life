namespace Supercell.Life.Server.Protocol.Commands.Client
{
    using Supercell.Life.Titan.DataStream;
    
    using Supercell.Life.Server.Network;

    internal class LogicSwapCharacterCommand : LogicCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicSwapCharacterCommand"/> class.
        /// </summary>
        public LogicSwapCharacterCommand(Connection connection, ByteStream stream) : base(connection, stream)
        {
            // LogicSwapCharacterCommand.
        }

        internal override void Decode()
        {
            this.ReadHeader();
        }

        internal override void Execute()
        {
        }
    }
}
