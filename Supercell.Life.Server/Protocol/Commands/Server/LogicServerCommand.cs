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
        internal void WriteHeader(ByteStream stream)
        {
            stream.WriteInt(this.Subtick);
            stream.WriteInt(this.Subtick);
            new LogicLong(this.Connection.GameMode.Avatar.HighID, this.Connection.GameMode.Avatar.LowID).Encode(stream);
        }
    }
}
