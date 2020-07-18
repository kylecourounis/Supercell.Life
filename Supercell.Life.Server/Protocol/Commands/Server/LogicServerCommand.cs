namespace Supercell.Life.Server.Protocol.Commands.Server
{
    using Supercell.Life.Titan.Logic.Math;

    using Supercell.Life.Server.Network;

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
        /// Encodes this instance.
        /// </summary>
        internal virtual void Encode()
        {
        }

        /// <summary>
        /// Writes the header.
        /// </summary>
        internal void WriteHeader()
        {
            this.Stream.WriteInt(this.Subtick);
            this.Stream.WriteInt(this.Subtick);
            new LogicLong(this.Connection.Avatar.HighID, this.Connection.Avatar.LowID).Encode(this.Stream);
        }
    }
}
