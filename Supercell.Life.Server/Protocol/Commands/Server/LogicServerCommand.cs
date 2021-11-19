namespace Supercell.Life.Server.Protocol.Commands.Server
{
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Titan.DataStream;

    internal class LogicServerCommand : LogicCommand
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogicServerCommand"/> class.
        /// </summary>
        internal LogicServerCommand(Connection connection) : base(connection)
        {
            // LogicServerCommand.
        }
        
        /// <summary>
        /// Writes the header.
        /// </summary>
        internal void WriteHeader(ChecksumEncoder encoder)
        {
            encoder.WriteInt(this.Subtick);
            encoder.WriteInt(this.Subtick);
            encoder.WriteLogicLong(this.Connection.GameMode.Avatar.Identifier);
        }
    }
}
