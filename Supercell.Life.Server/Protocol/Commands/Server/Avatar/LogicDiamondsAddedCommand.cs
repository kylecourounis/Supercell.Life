namespace Supercell.Life.Server.Protocol.Commands.Server
{
    using Supercell.Life.Titan.DataStream;

    using Supercell.Life.Server.Network;
    using Supercell.Life.Server.Protocol.Enums;

    internal class LogicDiamondsAddedCommand : LogicServerCommand
    {
        internal int Diamonds;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogicDiamondsAddedCommand"/> class.
        /// </summary>
        public LogicDiamondsAddedCommand(Connection connection) : base(connection)
        {
            this.Type = Command.DiamondsAdded;
        }

        internal override void Encode(ChecksumEncoder encoder)
        {
            encoder.WriteBoolean(true);
            encoder.WriteInt(this.Diamonds);
            encoder.WriteString(string.Empty); // Likely a transaction ID of some sort

            base.Encode(encoder);
        }
    }
}
